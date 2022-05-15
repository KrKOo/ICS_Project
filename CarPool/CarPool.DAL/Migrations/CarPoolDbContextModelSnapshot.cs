﻿// <auto-generated />
using System;
using CarPool.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CarPool.DAL.Migrations
{
    [DbContext(typeof(CarPoolDbContext))]
    partial class CarPoolDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CarPool.DAL.Entities.CarEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfRegistration")
                        .HasColumnType("datetime2");

                    b.Property<string>("LicensePlate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfSeats")
                        .HasColumnType("int");

                    b.Property<Guid>("OwnerID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PhotoUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerID");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("CarPool.DAL.Entities.RideEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CarID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DriverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<string>("Info")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RideDestination")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RideOrigin")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TimeOfStart")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CarID");

                    b.HasIndex("DriverId");

                    b.ToTable("Rides");
                });

            modelBuilder.Entity("CarPool.DAL.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Info")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CarPool.DAL.Entities.UserRideEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RideId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RideId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRideEntities");
                });

            modelBuilder.Entity("CarPool.DAL.Entities.CarEntity", b =>
                {
                    b.HasOne("CarPool.DAL.Entities.UserEntity", "Owner")
                        .WithMany("Cars")
                        .HasForeignKey("OwnerID")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("CarPool.DAL.Entities.RideEntity", b =>
                {
                    b.HasOne("CarPool.DAL.Entities.CarEntity", "Car")
                        .WithMany("Rides")
                        .HasForeignKey("CarID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarPool.DAL.Entities.UserEntity", "Driver")
                        .WithMany("RidesAsDriver")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("Driver");
                });

            modelBuilder.Entity("CarPool.DAL.Entities.UserRideEntity", b =>
                {
                    b.HasOne("CarPool.DAL.Entities.RideEntity", "Ride")
                        .WithMany("Passengers")
                        .HasForeignKey("RideId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarPool.DAL.Entities.UserEntity", "User")
                        .WithMany("RidesAsPassenger")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Ride");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CarPool.DAL.Entities.CarEntity", b =>
                {
                    b.Navigation("Rides");
                });

            modelBuilder.Entity("CarPool.DAL.Entities.RideEntity", b =>
                {
                    b.Navigation("Passengers");
                });

            modelBuilder.Entity("CarPool.DAL.Entities.UserEntity", b =>
                {
                    b.Navigation("Cars");

                    b.Navigation("RidesAsDriver");

                    b.Navigation("RidesAsPassenger");
                });
#pragma warning restore 612, 618
        }
    }
}
