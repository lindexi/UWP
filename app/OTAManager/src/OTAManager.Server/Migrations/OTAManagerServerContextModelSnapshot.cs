﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OTAManager.Server.Data;

namespace OTAManager.Server.Migrations
{
    [DbContext(typeof(OTAManagerServerContext))]
    partial class OTAManagerServerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.4");

            modelBuilder.Entity("OTAManager.Server.Model.ApplicationUpdateInfoModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ApplicationId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdateContext")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("LatestApplicationUpdateInfo");
                });
#pragma warning restore 612, 618
        }
    }
}