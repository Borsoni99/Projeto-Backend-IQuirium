﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Projeto_Backend_IQuirium.Repository;

#nullable disable

namespace Projeto_Backend_IQuirium.Migrations
{
    [DbContext(typeof(ProjetoBackendIQuiriumContext))]
    [Migration("20241030022916_createDatabase")]
    partial class createDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Projeto_Backend_IQuirium.Model.Feedback", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Conteudo")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<DateTime>("Criado_em")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTime(2024, 10, 29, 23, 29, 15, 691, DateTimeKind.Local).AddTicks(2211));

                    b.Property<Guid>("Id_usuario")
                        .HasColumnType("uuid");

                    b.Property<int>("Tipo_feedback")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id_usuario");

                    b.ToTable("feedback", (string)null);
                });

            modelBuilder.Entity("Projeto_Backend_IQuirium.Model.StatusFeedback", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Atualizado_em")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTime(2024, 10, 29, 23, 29, 15, 691, DateTimeKind.Local).AddTicks(3994));

                    b.Property<Guid>("Id_feedback")
                        .HasColumnType("uuid");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Id_feedback");

                    b.ToTable("status_feedback", (string)null);
                });

            modelBuilder.Entity("Projeto_Backend_IQuirium.Model.Usuario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Criado_em")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("10/29/2024 23:29:15");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.HasKey("Id");

                    b.ToTable("usuario", (string)null);
                });

            modelBuilder.Entity("Projeto_Backend_IQuirium.Model.Feedback", b =>
                {
                    b.HasOne("Projeto_Backend_IQuirium.Model.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("Id_usuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Projeto_Backend_IQuirium.Model.StatusFeedback", b =>
                {
                    b.HasOne("Projeto_Backend_IQuirium.Model.Feedback", "Feedback")
                        .WithMany()
                        .HasForeignKey("Id_feedback")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Feedback");
                });
#pragma warning restore 612, 618
        }
    }
}
