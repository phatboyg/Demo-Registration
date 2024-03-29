﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Registration.Data;

#nullable disable

namespace Registration.Service.Migrations
{
    [DbContext(typeof(RegistrationDbContext))]
    partial class RegistrationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Registration.Components.StateMachines.RegistrationState", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CardNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrentState")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParticipantCategory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParticipantEmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ParticipantLicenseExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ParticipantLicenseNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RaceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("RegistrationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("RetryAttempt")
                        .HasColumnType("int");

                    b.Property<Guid?>("ScheduleRetryToken")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CorrelationId");

                    b.ToTable("RegistrationState");
                });
#pragma warning restore 612, 618
        }
    }
}
