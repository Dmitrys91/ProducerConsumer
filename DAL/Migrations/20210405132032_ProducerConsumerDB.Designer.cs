﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProducerConsumer.DAL;

namespace DAL.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20210405132032_ProducerConsumerDB")]
    partial class ProducerConsumerDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ProducerConsumer.Model.Consumer", b =>
                {
                    b.Property<int>("ConsumerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("ConsumerId");

                    b.ToTable("Consumers");
                });

            modelBuilder.Entity("ProducerConsumer.Model.ConsumerTask", b =>
                {
                    b.Property<int>("ConsumerTaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ConsumerId")
                        .HasColumnType("int");

                    b.Property<string>("ConsumerTaskText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreationTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("ModificationTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ConsumerTaskId");

                    b.HasIndex("ConsumerId");

                    b.ToTable("ConsumerTasks");
                });

            modelBuilder.Entity("ProducerConsumer.Model.ConsumerTask", b =>
                {
                    b.HasOne("ProducerConsumer.Model.Consumer", "Consumer")
                        .WithMany("ConsumerTasks")
                        .HasForeignKey("ConsumerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Consumer");
                });

            modelBuilder.Entity("ProducerConsumer.Model.Consumer", b =>
                {
                    b.Navigation("ConsumerTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
