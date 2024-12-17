﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Информационная_система_для_больницы.Data;

namespace Информационная_система_для_больницы.Pages.Doctor
{
    /// <summary>
    /// Логика взаимодействия для DoctorPatients.xaml
    /// </summary>
    public partial class DoctorPatients : Page
    {
        Data.AppContext db;

        public static MainWindow main = (MainWindow)Application.Current.MainWindow;

        bool appointmentAddMode = false;

        Registration selectedPatient = null;
        Indicator selectedIndicator = null;
        Appointment selectedAppointment = null;
        Schedule selectedSchedule = null;
        //dynamic selectedAppointment = null;

        string completed = "Выполнено";
        string uncompleted = "Не выполнено";

        public DoctorPatients()
        {
            InitializeComponent();
            //db = new Data.AppContext();
            doctorPatientsPatientsList.ItemsSource = GetPatients();
        }

        public List<string> GetPatients()
        {
            db = new Data.AppContext();

            var allRegistrations = db.Registrations.ToList();
            var currentDoctor = from d in db.Employees
                                where d.fullName == main.currentUser.Content.ToString()
                                select d.id;
            var currentRegistrations = from r in allRegistrations
                                  let endDate = string.IsNullOrEmpty(r.end) ? (DateTime?)null : Convert.ToDateTime(r.end)
                                  where (endDate == null || endDate > DateTime.Today) && r.doctorId == currentDoctor.FirstOrDefault().ToString()
                                  select r.patientId;
            var currentPatients = from p in db.Patients
                                  where currentRegistrations.ToList().Contains(p.id)
                                  select p.fullName;

            return currentPatients.OrderBy(x => x).ToList();
        }

        private void doctorPatientsPatientsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if(doctorPatientsPatientsList.SelectedItem != null)
            {
            var _selectedPatient = from p in db.Patients
                                   where p.fullName == doctorPatientsPatientsList.SelectedValue.ToString()
                                   select p.id;
            var _selectedRegistration = from r in db.Registrations
                                        where r.patientId == _selectedPatient.FirstOrDefault().ToString()
                                        select r;

            selectedPatient = _selectedRegistration.FirstOrDefault();

            doctorPatientsIndicatorsList.ItemsSource = GetIndicators();
            doctorPatientsAppointmentsDataGrid.ItemsSource = GetAppointments();

                doctorPatientAddIndicator.IsEnabled = true;
                doctorPatientAddAppointment.IsEnabled = true;
            }
            else
            {
                doctorPatientAddIndicator.IsEnabled = false;
                doctorPatientAddAppointment.IsEnabled = false;
            }

        }

        public List<string> GetIndicators()
        {
            db = new Data.AppContext();
            var collectingIndicators =  from ci in db.CollectingIndicators
                                        where ci.registrationId == selectedPatient.id
                                        select ci.indicatorId;
            var indicators = from i in db.Indicators
                             where collectingIndicators.ToList().Contains(i.id)
                             select i.name;
            return indicators.ToList();
        }

        public List<object> GetAppointments()
        {
            db = new Data.AppContext();

            var appointments = from a in db.Appointments
                               where a.registrationId == selectedPatient.id
                               select new
                               {
                                   Id = a.id,
                                   Patient = a.registration.patient.fullName,
                                   Doctor = a.registration.doctor.fullName,
                                   DrugOrProcedure = a.drugProcedure.name,
                                   Note = a.note
                               };

            return appointments.AsEnumerable().Cast<dynamic>().ToList();
        }
        private void doctorPatientsAppointmentsDataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            doctorPatientsAppointmentsDataGrid.Columns[0].Header = "Идентификатор";
            doctorPatientsAppointmentsDataGrid.Columns[1].Header = "Пациент";
            doctorPatientsAppointmentsDataGrid.Columns[2].Header = "Лечащий врач";
            doctorPatientsAppointmentsDataGrid.Columns[3].Header = "Препарат или процедура";
            doctorPatientsAppointmentsDataGrid.Columns[4].Header = "Дополнительная информация";

            doctorPatientsAppointmentsDataGrid.Columns[0].Visibility = Visibility.Collapsed;

            doctorPatientsAppointmentsDataGrid.Columns[1].Width = new DataGridLength(1, DataGridLengthUnitType.Auto); 
            doctorPatientsAppointmentsDataGrid.Columns[2].Width = new DataGridLength(1, DataGridLengthUnitType.Auto); 
            doctorPatientsAppointmentsDataGrid.Columns[3].Width = new DataGridLength(1, DataGridLengthUnitType.Auto); 
            doctorPatientsAppointmentsDataGrid.Columns[4].Width = new DataGridLength(1, DataGridLengthUnitType.Star); 
        }
        private void doctorPatientsAppointmentsDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if(doctorPatientsAppointmentsDataGrid.SelectedItem != null)
            {
                dynamic _selectedAppointment = doctorPatientsAppointmentsDataGrid.SelectedItem;
                string id = _selectedAppointment.Id;
                var q = from a in db.Appointments
                        where a.id == id
                        select a;

                selectedAppointment = q.FirstOrDefault();

                doctorPatientShowSchedule.IsEnabled = true;
                doctorPatientDeleteAppointment.IsEnabled = true;
            }
            else
            {
                doctorPatientShowSchedule.IsEnabled = false;
                doctorPatientDeleteAppointment.IsEnabled = false;
            }

        }


        private void doctorPatientsIndicatorsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(doctorPatientsIndicatorsList.SelectedItem != null)
            {
                var _selectedIndicator = from i in db.Indicators
                        where i.name == doctorPatientsIndicatorsList.SelectedValue.ToString()
                        select i;
                selectedIndicator = _selectedIndicator.FirstOrDefault();
                doctorPatientShowCondition.IsEnabled = true;
                doctorPatientDeleteIndicator.IsEnabled = true;
            }
            else
            {
                doctorPatientShowCondition.IsEnabled = false;
                doctorPatientDeleteIndicator.IsEnabled = false;
            }
        }

        private void doctorPatientShowCondition_Click(object sender, RoutedEventArgs e)
        {
            OpenConditionsForm();
        }

        public void OpenConditionsForm()
        {
            doctorPatientsMainPart.IsEnabled = false;
            doctorPatientsChartLabel.Content = $"Значения показателя \"{doctorPatientsIndicatorsList.SelectedValue.ToString()}\" у пациента \"{doctorPatientsPatientsList.SelectedValue.ToString()}\"";
            SetChart();
            doctorPatientsConditionForm.Visibility = Visibility.Visible;
        }

        public void SetChart()
        {
            db = new Data.AppContext();

            var pcs = db.PatientConditions.ToList();
            var cis = db.CollectingIndicators.ToList();
            var _dataPoints = from pc in pcs
                              join ci in cis on pc.collectingIndicatorId equals ci.id
                              where ci.indicatorId == selectedIndicator.id && ci.registrationId == selectedPatient.id
                              orderby Convert.ToDateTime(pc.dateTime)
                              select new DataPoint(DateTimeAxis.ToDouble(Convert.ToDateTime(pc.dateTime)), Convert.ToDouble(pc.value));

            PlotModel plotModel = new PlotModel();
            if(_dataPoints.Count()>0)
            {


            List<DataPoint> dataPoints = _dataPoints.ToList();


            var backgroundColor = OxyColor.Parse(((Color)Application.Current.FindResource("background")).ToString());
            plotModel.PlotAreaBackground = backgroundColor;

            var textColor = OxyColor.Parse(((Color)Application.Current.FindResource("text")).ToString());
            plotModel.PlotAreaBorderColor = textColor;
            plotModel.TextColor = textColor;
            plotModel.DefaultFontSize = 12;
            plotModel.AxisTierDistance = -5;
            plotModel.Padding = new OxyThickness(2.5, -2.5,-10,2.5);
            plotModel.DefaultFont = "Bahnschrift";

            LinearAxis xAxis = new DateTimeAxis
            {
                Title = "Дата",
                Position = AxisPosition.Bottom,
                TicklineColor = textColor,
                StringFormat = "dd.MM HH:mm",
                //StringFormat = "hh:mm",
                IntervalLength = 100,
                TitleFontSize = 15,
                AbsoluteMinimum = dataPoints.Min(x => x.X),
                AbsoluteMaximum = dataPoints.Max(x => x.X),
            };
            LinearAxis yAxis = new LinearAxis
            {
                Title = "Значение",
                Position = AxisPosition.Left,
                TicklineColor = textColor,
                TitleFontSize = 15,
                //StringFormat = "F",
                IntervalLength = 10,
                AbsoluteMinimum = dataPoints.Min(x => x.Y),
                AbsoluteMaximum = dataPoints.Max(x => x.Y),
            };

            plotModel.Axes.Add(xAxis);
            plotModel.Axes.Add(yAxis);

            LineSeries series = new LineSeries();
            var lineColor = OxyColor.Parse(((Color)Application.Current.FindResource("menu")).ToString());
            series.Color = lineColor;
            series.MarkerFill = lineColor;
            series.ItemsSource = dataPoints;
            plotModel.Series.Add(series);
            }

            doctorPatientsChart.Model = plotModel;
        }

        private void registrarDrugsDrugFormCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseConditionsForm();
        }

        public void CloseConditionsForm()
        {
            doctorPatientsConditionForm.Visibility = Visibility.Collapsed;
            doctorPatientsMainPart.IsEnabled = true;
        }
        private void doctorPatientAddIndicator_Click(object sender, RoutedEventArgs e)
        {
            OpenAddIndicatorForm();
            OpenAddIndicatorForm();
        }

        private void doctorPatientDeleteIndicator_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите безвозвратно удалить эту записи об этом показателе у пациента {selectedPatient.patient.fullName}?", "Удаление записи расписания", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var q = from ci in db.CollectingIndicators
                        where ci.indicatorId == selectedIndicator.id && ci.registrationId == selectedPatient.id
                        select ci;

                CollectingIndicator collectingIndicator = q.FirstOrDefault();

                db.CollectingIndicators.Attach(collectingIndicator);
                db.Entry(collectingIndicator).State = EntityState.Deleted;
                db.SaveChanges();
                doctorPatientsIndicatorsList.ItemsSource = GetIndicators();
            }
        }

        public void OpenAddIndicatorForm()
        {
            doctorPatientsMainPart.IsEnabled = false;
            
            doctorPatientsIndicatorFormName.ItemsSource = GetAvalibleCollectingIndicators();

            doctorPatientsAddIndicatorForm.Visibility = Visibility.Visible;
        }

        public List<string> GetAvalibleCollectingIndicators()
        {

        db = new Data.AppContext();


            List<string> currentCollectingIndicators = doctorPatientsIndicatorsList.Items.Cast<string>().ToList();

            var avaliableCollectingIndicators = from i in db.Indicators
                                                 where !currentCollectingIndicators.Contains(i.name)
                                                 select i.name;
            return avaliableCollectingIndicators.ToList();
        }
        public void CloseAddIndicatorForm()
        {
            doctorPatientsAddIndicatorForm.Visibility = Visibility.Collapsed;
            doctorPatientsMainPart.IsEnabled = true;
            doctorPatientsIndicatorsList.ItemsSource = GetIndicators();

        }

        private void doctorPatientsIndicatorFormAccept_Click(object sender, RoutedEventArgs e)
        {
            if(GetAvalibleCollectingIndicators().Contains(doctorPatientsIndicatorFormName.Text))
            {
                CollectingIndicator collectingIndicator = new CollectingIndicator();
                collectingIndicator.id = Guid.NewGuid().ToString();
                collectingIndicator.registrationId = selectedPatient.id;
                collectingIndicator.indicatorId = db.Indicators.Where(x => x.name == doctorPatientsIndicatorFormName.Text).Select(x => x.id).FirstOrDefault();

                db.CollectingIndicators.Add(collectingIndicator);
                db.SaveChanges();
                doctorPatientsIndicatorsList.ItemsSource = GetIndicators();
                CloseAddIndicatorForm();
            }
            else
            {
                MessageBox.Show("Невозможно добавить такой показатель");
            }
        }

        private void doctorPatientsIndicatorFormCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseAddIndicatorForm();
        }
        private void doctorPatientShowSchedule_Click(object sender, RoutedEventArgs e)
        {
            OpenScheduleDataGridForm();
        }

        private void doctorPatientsSchedulesFormAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenScheduleForm();
        }


        public void OpenScheduleForm()
        {
            doctorPatientsSchedulesForm.IsEnabled = false;
            doctorPatientsScheduleForm.Visibility = Visibility.Visible;
        }
        public void CloseScheduleForm()
        {
            doctorPatientsScheduleForm.Visibility = Visibility.Collapsed;
            doctorPatientsSchedulesForm.IsEnabled = true;
            SetSchedulesFormData();
        }
        private void doctorPatientsSchedulesFormDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите безвозвратно удалить эту запись расписания?", "Удаление записи расписания", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                db.Schedules.Attach(selectedSchedule);
                db.Entry(selectedSchedule).State = EntityState.Deleted;
                db.SaveChanges();
            }

            SetSchedulesFormData();
        }

        private void doctorPatientsSchedulesFormClose_Click(object sender, RoutedEventArgs e)
        {
            CloseScheduleDataGridForm();
        }

        public void OpenScheduleDataGridForm()
        {
            doctorPatientsMainPart.IsEnabled = false;

            SetSchedulesFormData();

            doctorPatientsScheduleLabel.Text = $"Расписание приёма(проведения) препарата(процедуры) {selectedAppointment.drugProcedure.name} у пациента {selectedPatient.patient.fullName}";

            doctorPatientsSchedulesForm.Visibility = Visibility.Visible;
        }

        public void SetSchedulesFormData()
        {
            db = new Data.AppContext();
            var _shedule = db.Schedules.ToList();
            var schedule = from s in _shedule
                           where s.appointmentId == selectedAppointment.id
                           orderby Convert.ToDateTime(s.dateTime)
                           select new
                           {
                               Id = s.id,
                               Patient = s.appointment.registration.patient.fullName,
                               Doctor = s.appointment.doctor.fullName,
                               DateAndTime = Convert.ToDateTime(s.dateTime).ToString("dd.MM.yyyy HH:mm"),
                               Status = s.status,
                               Note = s.note,
                           };
            doctorPatientsScheduleDataGrid.ItemsSource = schedule.ToList();
            doctorPatientsScheduleDataGrid.SelectedItem = null;
        }
        private void doctorPatientsScheduleDataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            doctorPatientsScheduleDataGrid.Columns[0].Header = "Идентификатор";
            doctorPatientsScheduleDataGrid.Columns[1].Header = "Пациент";
            doctorPatientsScheduleDataGrid.Columns[2].Header = "Врач";
            doctorPatientsScheduleDataGrid.Columns[3].Header = "Дата";
            doctorPatientsScheduleDataGrid.Columns[4].Header = "Статус";
            doctorPatientsScheduleDataGrid.Columns[5].Header = "Дополнительная информация";

            doctorPatientsScheduleDataGrid.Columns[0].Visibility = Visibility.Collapsed;

            doctorPatientsScheduleDataGrid.Columns[1].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            doctorPatientsScheduleDataGrid.Columns[2].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            doctorPatientsScheduleDataGrid.Columns[3].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            doctorPatientsScheduleDataGrid.Columns[4].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            doctorPatientsScheduleDataGrid.Columns[5].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }
        private void doctorPatientsScheduleDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if(doctorPatientsScheduleDataGrid.SelectedItem != null)
            {
                dynamic _selectedSchedule = doctorPatientsScheduleDataGrid.SelectedItem;
                string id = _selectedSchedule.Id;
                var q = from s in db.Schedules
                        where s.id == id
                        select s;

                selectedSchedule = q.FirstOrDefault();

                if(selectedSchedule != null && selectedSchedule.status != completed)
                {
                    doctorPatientsSchedulesFormDelete.IsEnabled = true;
                }

            }
            else
            {
                doctorPatientsSchedulesFormDelete.IsEnabled = false;
            }
        }

        private void doctorPatientsScheduleFormAccept_Click(object sender, RoutedEventArgs e)
        {
            if(Convert.ToDateTime(doctorPatientsScheduleFormDateTime.Text)>Convert.ToDateTime(selectedPatient.start) && Convert.ToDateTime(doctorPatientsScheduleFormDateTime.Text) < Convert.ToDateTime(selectedPatient.end))
            {
                
                Schedule schedule = new Schedule();
                schedule.id = Guid.NewGuid().ToString();
                schedule.appointmentId = selectedAppointment.id;
                schedule.dateTime = doctorPatientsScheduleFormDateTime.Text;
                if(Convert.ToDateTime(doctorPatientsScheduleFormDateTime.Text)<DateTime.Now)
                {
                    schedule.status = completed;
                }
                //else
                //{
                //    schedule.status = uncompleted;
                //}

                if(db.Schedules.Where(x => x.appointmentId == schedule.appointmentId && x.dateTime == schedule.dateTime).Count() == 0)
                {
                    db.Schedules.Add(schedule);
                    db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Запись на такое время уже существует.");
                }

            }

            CloseScheduleForm();
        }

        private void doctorPatientsScheduleFormCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseScheduleForm();
        }
        public void CloseScheduleDataGridForm()
        {
            doctorPatientsSchedulesForm.Visibility = Visibility.Collapsed;
            doctorPatientsMainPart.IsEnabled = true;
        }



        private void doctorPatientAddAppointment_Click(object sender, RoutedEventArgs e)
        {
            OpenAppointmentForm();
        }

        public void OpenAppointmentForm()
        {
            doctorPatientsMainPart.IsEnabled = false;

            doctorPatientsAddAppointmentFormDrugOrProcedure.ItemsSource = db.DrugsProcedures.Select(x => x.name).OrderBy(x => x).ToList();

            doctorPatientsAddAppointmentForm.Visibility = Visibility.Visible;
        }
        public void CloseAppointmentForm()
        {
            doctorPatientsAddAppointmentForm.Visibility = Visibility.Collapsed;
            doctorPatientsMainPart.IsEnabled = true;
        }


        //private void doctorPatientAlterAppointment_Click(object sender, RoutedEventArgs e)
        //{

        //}

        private void doctorPatientDeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите безвозвратно удалить это назначеине?\nВместе с этой записью будут удалены {db.Schedules.Where(x => x.appointmentId == selectedAppointment.id).Count()} связанных записей из таблицы Расписания", "Удаление назначения", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                using (var transaction = db.Database.BeginTransaction())
                {

                    foreach(Schedule schedule in db.Schedules.Where(x => x.appointmentId == selectedAppointment.id).ToList())
                    {
                        db.Schedules.Attach(schedule);
                        db.Entry(schedule).State = EntityState.Deleted;
                    }

                    db.Entry(selectedAppointment).State = EntityState.Detached;
                    db.Appointments.Attach(selectedAppointment);
                    db.Entry(selectedAppointment).State = EntityState.Deleted;

                    db.SaveChanges();
                    transaction.Commit();
                }
                
                doctorPatientsAppointmentsDataGrid.ItemsSource = GetAppointments();
            }
        }

        private void doctorPatientsAddAppointmentFormAccept_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(doctorPatientsAddAppointmentFormDrugOrProcedure.Text))
            {
                if (!string.IsNullOrEmpty(doctorPatientsAddAppointmentStart.Text) && (!string.IsNullOrEmpty(doctorPatientsAddAppointmentEnd.Text)) && (!string.IsNullOrEmpty(doctorPatientsAddAppointmentTimes.Text)))
                {

                if (Convert.ToDateTime(doctorPatientsAddAppointmentStart.Text) <= Convert.ToDateTime(doctorPatientsAddAppointmentEnd.Text))
                {
                    //добавить проверку для дат на вхождение в период регистрации
                    if (int.TryParse(doctorPatientsAddAppointmentTimes.Text, out int times) && times > 0 && times <= 720)
                    {
                        Appointment appointment = new Appointment();
                        appointment.id = Guid.NewGuid().ToString();
                        appointment.registrationId = selectedPatient.id;
                        appointment.doctorId = db.Employees.Where(x => x.fullName == main.currentUser.Content.ToString()).Select(x => x.id).FirstOrDefault();
                        appointment.drugProcedureId = db.DrugsProcedures.Where(x => x.name == doctorPatientsAddAppointmentFormDrugOrProcedure.Text).Select(x => x.id).FirstOrDefault();
                        appointment.note = doctorPatientsAddAppointmentNote.Text;

                        using (var transaction = db.Database.BeginTransaction())
                        {
                            db.Appointments.Add(appointment);

                            for (DateTime d = Convert.ToDateTime(doctorPatientsAddAppointmentStart.Text); d.Ticks <= Convert.ToDateTime(doctorPatientsAddAppointmentEnd.Text).Ticks; d = d.AddDays(1))
                            {
                                for (int i = 0; i < times; i++)
                                {
                                    Schedule schedule = new Schedule();
                                    schedule.id = Guid.NewGuid().ToString();
                                    schedule.appointmentId = appointment.id;


                                    if (times > 1)
                                    {
                                        int inverval = 720 / (times - 1);
                                        schedule.dateTime = $"{d.Date.ToString("dd.MM.yyyy")} {(8 + Math.Floor((decimal)i * inverval / 60)):00}:{(i * inverval) - (Math.Floor((decimal)i * inverval / 60) * 60):00}";
                                    }
                                    else
                                        schedule.dateTime = $"{d.Date.ToString("dd.MM.yyyy")} 14:00";

                                    if (Convert.ToDateTime(schedule.dateTime) < DateTime.Now)
                                    {
                                        schedule.status = completed;
                                    }

                                    db.Schedules.Add(schedule);
                                }
                            }
                            db.SaveChanges();
                            transaction.Commit();
                            doctorPatientsAppointmentsDataGrid.ItemsSource = GetAppointments();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Некорректные данные в поле \"Количество в день\"");

                    }
                }
                else
                {
                    MessageBox.Show("Дата начала должна быть раньше даты конца");
                }
            }
                else
                {
                    Appointment appointment = new Appointment();
                    appointment.id = Guid.NewGuid().ToString();
                    appointment.registrationId = selectedPatient.id;
                    appointment.doctorId = db.Employees.Where(x => x.fullName == main.currentUser.Content.ToString()).Select(x => x.id).FirstOrDefault();
                    appointment.drugProcedureId = db.DrugsProcedures.Where(x => x.name == doctorPatientsAddAppointmentFormDrugOrProcedure.Text).Select(x => x.id).FirstOrDefault();
                    appointment.note = doctorPatientsAddAppointmentNote.Text;

                        db.Appointments.Add(appointment);
                }
            }
            else
            {
                MessageBox.Show("Необходимо заполнить все необходимые поля");
            }


            CloseAppointmentForm();
        }

        private void doctorPatientsAddAppointmentFormCancel_Click(object sender, RoutedEventArgs e)
        {

            CloseAppointmentForm();
        }
    }
}