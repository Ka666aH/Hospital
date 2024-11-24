using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Информационная_система_для_больницы;
using System.Data.Entity;
using System.Windows.Controls;

namespace Информационная_система_для_больницы.Data
{
    class AppContext : DbContext
    {
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeStatus> EmployeesStatuses { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<BedRegistration> BedsRegistrations { get; set; }
        public DbSet<DrugProcedure> DrugsProcedures { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Indicator> Indicators { get; set; }
        public DbSet<PatientCondition> PatientConditions { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        public AppContext() : base("DefaultConnection") 
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<AppContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Status>()
                .ToTable("Statuses");
            modelBuilder.Entity<Employee>()
                .ToTable("Employees");
            modelBuilder.Entity<EmployeeStatus>()
                .ToTable("EmployeesStatuses");
            //    .HasRequired(o => o.employee)
            //    .WithMany()
            //    .HasForeignKey(o => o.employeeId);
            //modelBuilder.Entity<EmployeeStatus>()
            //    .ToTable("EmployeesStatuses")
            //    .HasRequired(o => o.status)
            //    .WithMany()
            //    .HasForeignKey(o => o.statusId);

            // Настройка внешних ключей и других отношений, если необходимо
            //modelBuilder.Entity<Order>()
            //    .HasRequired(o => o.Product)
            //    .WithMany()
            //    .HasForeignKey(o => o.ProductId);
            modelBuilder.Entity<Bed>()
                .ToTable("Beds");
            modelBuilder.Entity<BedRegistration>()
                .ToTable("BedsRegistrations");
            modelBuilder.Entity<Patient>()
                .ToTable("Patients");
            modelBuilder.Entity<Indicator>()
                .ToTable("Indicators");
            modelBuilder.Entity<PatientCondition>()
                .ToTable("PatientsConditions");
            modelBuilder.Entity<Registration>()
                .ToTable("Registrations");
            modelBuilder.Entity<DrugProcedure>()
                .ToTable("DrugsProcedures");
            modelBuilder.Entity<Appointment>()
                .ToTable("Appointments");
            modelBuilder.Entity<Schedule>()
                .ToTable("Schedules");
           
            base.OnModelCreating(modelBuilder);
        }
    }
}
