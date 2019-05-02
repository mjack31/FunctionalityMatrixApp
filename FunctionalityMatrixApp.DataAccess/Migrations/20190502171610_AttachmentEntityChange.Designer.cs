﻿// <auto-generated />
using System;
using FunctionalityMatrixApp.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FunctionalityMatrixApp.DataAccess.Migrations
{
    [DbContext(typeof(ProductsDbContext))]
    [Migration("20190502171610_AttachmentEntityChange")]
    partial class AttachmentEntityChange
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0-preview4.19216.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FunctionalityMatrixApp.Model.Attachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("ProductId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("FunctionalityMatrixApp.Model.Picture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("ProductId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("FunctionalityMatrixApp.Model.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<bool>("InAutomotive");

                    b.Property<bool>("InFashion");

                    b.Property<bool>("InFurniture");

                    b.Property<bool>("IsAllcomp");

                    b.Property<bool>("IsInDevelopment");

                    b.Property<bool>("IsInOffer");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("ParentId");

                    b.Property<string>("Producer")
                        .IsRequired();

                    b.Property<int>("ProductType");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("FunctionalityMatrixApp.Model.Attachment", b =>
                {
                    b.HasOne("FunctionalityMatrixApp.Model.Product", null)
                        .WithMany("Attachments")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("FunctionalityMatrixApp.Model.Picture", b =>
                {
                    b.HasOne("FunctionalityMatrixApp.Model.Product", null)
                        .WithMany("Pictures")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("FunctionalityMatrixApp.Model.Product", b =>
                {
                    b.HasOne("FunctionalityMatrixApp.Model.Product", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });
#pragma warning restore 612, 618
        }
    }
}
