﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OTAManager.Server.Data;

namespace OTAManager.Server.Migrations.FileStorage
{
    [DbContext(typeof(FileStorageContext))]
    [Migration("20210321085802_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.4");

            modelBuilder.Entity("OTAManager.Server.Model.FileStorageModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FileKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("FileLength")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Md5")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FileKey");

                    b.HasIndex("Md5");

                    b.ToTable("FileStorageData");
                });
#pragma warning restore 612, 618
        }
    }
}
