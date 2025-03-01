using HospitalManagementSystem.Entities;
using HospitalManagementSystemPhase2.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem
{
    public class HMSDBContext: DbContext
    {
        public HMSDBContext() { }
        public HMSDBContext(DbContextOptions<HMSDBContext> options) : base(options) {  }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=HospitalManagementSystem1;Integrated Security=True;Encrypt=False");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medication>()
                .HasMany(m => m.Prescriptions)
                .WithMany(p => p.Medications);

            modelBuilder.Entity<Medication>().HasData(
                new Medication { MedicationId = 10, Name = "Aspirin", Quantity = 1, Price = 9.99m },
                new Medication { MedicationId = 20, Name = "Paracetamol", Quantity = 2, Price = 4.49m },
                new Medication { MedicationId = 30, Name = "Ibuprofen", Quantity = 3, Price = 7.99m },
                new Medication { MedicationId = 40, Name = "Amoxicillin", Quantity = 4, Price = 12.99m }
            );

            //modelBuilder.Entity<Prescription>().ToTable("Prescriptions", t => t.HasTrigger("Insert_Bill_Automatically"));

        }

    }
}
