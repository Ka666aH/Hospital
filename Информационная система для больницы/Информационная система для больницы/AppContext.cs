using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Информационная_система_для_больницы;
using System.Data.Entity;

namespace Информационная_система_для_больницы.Data
{
    class AppContext : DbContext
    {
        public DbSet<Status> Statuss { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeStatus> EmployeeStatuss { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<BedRegistration> BedRegistrations { get; set; }
        public DbSet<DrugProcedure> DrugProcedures { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Indicator> Indicators { get; set; }
        public DbSet<PatientCondition> PatientConditions { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        public AppContext() : base("DefaultConnection") 
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<AppContext>());
        }
    }
}
