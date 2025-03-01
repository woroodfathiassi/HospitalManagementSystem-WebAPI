﻿// <auto-generated />
using System;
using HospitalManagementSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HospitalManagementSystemPhase2.Migrations
{
    [DbContext(typeof(HMSDBContext))]
    partial class HMSDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HospitalManagementSystem.Entities.Appointment", b =>
                {
                    b.Property<int>("AppointmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AppointmentId"));

                    b.Property<DateTime>("AppointmentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("AppointmentId");

                    b.HasIndex("DoctorId");

                    b.HasIndex("PatientId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("HospitalManagementSystem.Entities.Bill", b =>
                {
                    b.Property<int>("BillId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BillId"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("BillDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PrescriptionId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("BillId");

                    b.HasIndex("PrescriptionId")
                        .IsUnique();

                    b.ToTable("Bills");
                });

            modelBuilder.Entity("HospitalManagementSystem.Entities.Doctor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<string>("ContactNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.Property<string>("Specialty")
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("HospitalManagementSystem.Entities.Medication", b =>
                {
                    b.Property<int>("MedicationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MedicationId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("MedicationId");

                    b.ToTable("Medications");

                    b.HasData(
                        new
                        {
                            MedicationId = 10,
                            Name = "Aspirin",
                            Price = 9.99m,
                            Quantity = 1
                        },
                        new
                        {
                            MedicationId = 20,
                            Name = "Paracetamol",
                            Price = 4.49m,
                            Quantity = 2
                        },
                        new
                        {
                            MedicationId = 30,
                            Name = "Ibuprofen",
                            Price = 7.99m,
                            Quantity = 3
                        },
                        new
                        {
                            MedicationId = 40,
                            Name = "Amoxicillin",
                            Price = 12.99m,
                            Quantity = 4
                        });
                });

            modelBuilder.Entity("HospitalManagementSystem.Entities.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<string>("ContactNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("HospitalManagementSystem.Entities.Prescription", b =>
                {
                    b.Property<int>("PrescriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PrescriptionId"));

                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PrescriptionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("PrescriptionId");

                    b.HasIndex("DoctorId");

                    b.HasIndex("PatientId");

                    b.ToTable("Prescriptions");
                });

            modelBuilder.Entity("HospitalManagementSystemPhase2.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("HospitalManagementSystemPhase2.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MedicationPrescription", b =>
                {
                    b.Property<int>("MedicationsMedicationId")
                        .HasColumnType("int");

                    b.Property<int>("PrescriptionsPrescriptionId")
                        .HasColumnType("int");

                    b.HasKey("MedicationsMedicationId", "PrescriptionsPrescriptionId");

                    b.HasIndex("PrescriptionsPrescriptionId");

                    b.ToTable("MedicationPrescription");
                });

            modelBuilder.Entity("HospitalManagementSystem.Entities.Appointment", b =>
                {
                    b.HasOne("HospitalManagementSystem.Entities.Doctor", "Doctor")
                        .WithMany("Appointments")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HospitalManagementSystem.Entities.Patient", "Patient")
                        .WithMany("Appointments")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("HospitalManagementSystem.Entities.Bill", b =>
                {
                    b.HasOne("HospitalManagementSystem.Entities.Prescription", "Prescription")
                        .WithOne("Bill")
                        .HasForeignKey("HospitalManagementSystem.Entities.Bill", "PrescriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Prescription");
                });

            modelBuilder.Entity("HospitalManagementSystem.Entities.Doctor", b =>
                {
                    b.HasOne("HospitalManagementSystemPhase2.Entities.User", "User")
                        .WithOne("Doctor")
                        .HasForeignKey("HospitalManagementSystem.Entities.Doctor", "UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HospitalManagementSystem.Entities.Patient", b =>
                {
                    b.HasOne("HospitalManagementSystemPhase2.Entities.User", "User")
                        .WithOne("Patient")
                        .HasForeignKey("HospitalManagementSystem.Entities.Patient", "UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HospitalManagementSystem.Entities.Prescription", b =>
                {
                    b.HasOne("HospitalManagementSystem.Entities.Doctor", "Doctor")
                        .WithMany("Prescriptions")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HospitalManagementSystem.Entities.Patient", "Patient")
                        .WithMany("Prescriptions")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("HospitalManagementSystemPhase2.Entities.User", b =>
                {
                    b.HasOne("HospitalManagementSystemPhase2.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("MedicationPrescription", b =>
                {
                    b.HasOne("HospitalManagementSystem.Entities.Medication", null)
                        .WithMany()
                        .HasForeignKey("MedicationsMedicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HospitalManagementSystem.Entities.Prescription", null)
                        .WithMany()
                        .HasForeignKey("PrescriptionsPrescriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HospitalManagementSystem.Entities.Doctor", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Prescriptions");
                });

            modelBuilder.Entity("HospitalManagementSystem.Entities.Patient", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Prescriptions");
                });

            modelBuilder.Entity("HospitalManagementSystem.Entities.Prescription", b =>
                {
                    b.Navigation("Bill")
                        .IsRequired();
                });

            modelBuilder.Entity("HospitalManagementSystemPhase2.Entities.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("HospitalManagementSystemPhase2.Entities.User", b =>
                {
                    b.Navigation("Doctor");

                    b.Navigation("Patient");
                });
#pragma warning restore 612, 618
        }
    }
}
