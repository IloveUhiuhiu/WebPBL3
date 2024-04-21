﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebPBL3.Models;

#nullable disable

namespace WebPBL3.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebPBL3.Models.Account", b =>
                {
                    b.Property<string>("AccountID")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("RoleID")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("AccountID");

                    b.HasIndex("RoleID");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("WebPBL3.Models.Car", b =>
                {
                    b.Property<string>("CarID")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<double>("Capacity")
                        .HasMaxLength(50)
                        .HasColumnType("float");

                    b.Property<string>("CarName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Dimension")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Engine")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("Flag")
                        .HasColumnType("bit");

                    b.Property<string>("FuelConsumption")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("MakeID")
                        .HasColumnType("int");

                    b.Property<string>("Origin")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Photo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("Seat")
                        .HasColumnType("int");

                    b.Property<float>("Topspeed")
                        .HasColumnType("real");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("CarID");

                    b.HasIndex("MakeID");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("WebPBL3.Models.DetailOrder", b =>
                {
                    b.Property<string>("DetailOrderID")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("CarID")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("OrderID")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("DetailOrderID");

                    b.HasIndex("CarID");

                    b.HasIndex("OrderID");

                    b.ToTable("DetailOrders");
                });

            modelBuilder.Entity("WebPBL3.Models.District", b =>
                {
                    b.Property<int>("DistrictID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DistrictID"));

                    b.Property<string>("DistrictName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("ProvinceID")
                        .HasColumnType("int");

                    b.HasKey("DistrictID");

                    b.HasIndex("ProvinceID");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("WebPBL3.Models.Feedback", b =>
                {
                    b.Property<string>("FeedbackID")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("FeedbackID");

                    b.HasIndex("UserID");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("WebPBL3.Models.Make", b =>
                {
                    b.Property<int>("MakeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MakeID"));

                    b.Property<string>("MakeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("MakeID");

                    b.ToTable("Makes");
                });

            modelBuilder.Entity("WebPBL3.Models.News", b =>
                {
                    b.Property<string>("NewsID")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Photo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StaffID")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdateBy")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("NewsID");

                    b.HasIndex("StaffID");

                    b.ToTable("NewS");
                });

            modelBuilder.Entity("WebPBL3.Models.Order", b =>
                {
                    b.Property<string>("OrderID")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Flag")
                        .HasColumnType("bit");

                    b.Property<string>("StaffID")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Totalprice")
                        .HasColumnType("int");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("OrderID");

                    b.HasIndex("StaffID");

                    b.HasIndex("UserID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("WebPBL3.Models.Province", b =>
                {
                    b.Property<int>("ProvinceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProvinceID"));

                    b.Property<string>("ProvinceName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ProvinceID");

                    b.ToTable("Provinces");
                });

            modelBuilder.Entity("WebPBL3.Models.Role", b =>
                {
                    b.Property<int>("RoleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleID"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("RoleID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("WebPBL3.Models.Staff", b =>
                {
                    b.Property<string>("StaffID")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Salary")
                        .HasColumnType("int");

                    b.Property<string>("UserID")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("StaffID");

                    b.HasIndex("UserID")
                        .IsUnique()
                        .HasFilter("[UserID] IS NOT NULL");

                    b.ToTable("Staffs");
                });

            modelBuilder.Entity("WebPBL3.Models.User", b =>
                {
                    b.Property<string>("UserID")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("AccountID")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Address")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool?>("Gender")
                        .HasColumnType("bit");

                    b.Property<string>("IdentityCard")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WardID")
                        .HasColumnType("int");

                    b.HasKey("UserID");

                    b.HasIndex("AccountID")
                        .IsUnique()
                        .HasFilter("[AccountID] IS NOT NULL");

                    b.HasIndex("WardID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebPBL3.Models.Ward", b =>
                {
                    b.Property<int>("WardID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WardID"));

                    b.Property<int?>("DistrictID")
                        .HasColumnType("int");

                    b.Property<string>("WardName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("WardID");

                    b.HasIndex("DistrictID");

                    b.ToTable("Wards");
                });

            modelBuilder.Entity("WebPBL3.Models.Account", b =>
                {
                    b.HasOne("WebPBL3.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("WebPBL3.Models.Car", b =>
                {
                    b.HasOne("WebPBL3.Models.Make", "Make")
                        .WithMany("Cars")
                        .HasForeignKey("MakeID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Make");
                });

            modelBuilder.Entity("WebPBL3.Models.DetailOrder", b =>
                {
                    b.HasOne("WebPBL3.Models.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("WebPBL3.Models.Order", "Order")
                        .WithMany("DetailOrders")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("WebPBL3.Models.District", b =>
                {
                    b.HasOne("WebPBL3.Models.Province", "Province")
                        .WithMany("Districts")
                        .HasForeignKey("ProvinceID")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Province");
                });

            modelBuilder.Entity("WebPBL3.Models.Feedback", b =>
                {
                    b.HasOne("WebPBL3.Models.User", "User")
                        .WithMany("Feedbacks")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebPBL3.Models.News", b =>
                {
                    b.HasOne("WebPBL3.Models.Staff", "Staff")
                        .WithMany("News")
                        .HasForeignKey("StaffID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Staff");
                });

            modelBuilder.Entity("WebPBL3.Models.Order", b =>
                {
                    b.HasOne("WebPBL3.Models.Staff", "Staff")
                        .WithMany()
                        .HasForeignKey("StaffID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("WebPBL3.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Staff");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebPBL3.Models.Staff", b =>
                {
                    b.HasOne("WebPBL3.Models.User", "User")
                        .WithOne("Staff")
                        .HasForeignKey("WebPBL3.Models.Staff", "UserID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebPBL3.Models.User", b =>
                {
                    b.HasOne("WebPBL3.Models.Account", "Account")
                        .WithOne("User")
                        .HasForeignKey("WebPBL3.Models.User", "AccountID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebPBL3.Models.Ward", "Ward")
                        .WithMany()
                        .HasForeignKey("WardID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Ward");
                });

            modelBuilder.Entity("WebPBL3.Models.Ward", b =>
                {
                    b.HasOne("WebPBL3.Models.District", "District")
                        .WithMany("Wards")
                        .HasForeignKey("DistrictID")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("District");
                });

            modelBuilder.Entity("WebPBL3.Models.Account", b =>
                {
                    b.Navigation("User")
                        .IsRequired();
                });

            modelBuilder.Entity("WebPBL3.Models.District", b =>
                {
                    b.Navigation("Wards");
                });

            modelBuilder.Entity("WebPBL3.Models.Make", b =>
                {
                    b.Navigation("Cars");
                });

            modelBuilder.Entity("WebPBL3.Models.Order", b =>
                {
                    b.Navigation("DetailOrders");
                });

            modelBuilder.Entity("WebPBL3.Models.Province", b =>
                {
                    b.Navigation("Districts");
                });

            modelBuilder.Entity("WebPBL3.Models.Staff", b =>
                {
                    b.Navigation("News");
                });

            modelBuilder.Entity("WebPBL3.Models.User", b =>
                {
                    b.Navigation("Feedbacks");

                    b.Navigation("Orders");

                    b.Navigation("Staff")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
