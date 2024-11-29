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
    [Migration("20241129042211_UpdateMotivoNullable")]
    partial class UpdateMotivoNullable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Projeto_Backend_IQuirium.Model.FeedbackProduto", b =>
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
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("Id_usuario")
                        .HasColumnType("uuid");

                    b.Property<int>("Tipo_feedback")
                        .HasMaxLength(100)
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id_usuario");

                    b.ToTable("feedback_produto", (string)null);
                });

            modelBuilder.Entity("Projeto_Backend_IQuirium.Model.FeedbackUsuario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ConteudoFeedback")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<string>("ConteudoReport")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<DateTime>("DataHoraEnvio")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("DestinatarioId")
                        .HasColumnType("uuid");

                    b.Property<string>("Motivo")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<Guid>("RemetenteId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasMaxLength(255)
                        .HasColumnType("integer");

                    b.Property<int>("Tipo")
                        .HasMaxLength(100)
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DestinatarioId");

                    b.HasIndex("RemetenteId");

                    b.ToTable("feedback_usuario", (string)null);
                });

            modelBuilder.Entity("Projeto_Backend_IQuirium.Model.Usuario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Criado_em")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.ToTable("usuario", (string)null);
                });

            modelBuilder.Entity("Projeto_Backend_IQuirium.Model.FeedbackProduto", b =>
                {
                    b.HasOne("Projeto_Backend_IQuirium.Model.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("Id_usuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Projeto_Backend_IQuirium.Model.FeedbackUsuario", b =>
                {
                    b.HasOne("Projeto_Backend_IQuirium.Model.Usuario", "Destinatario")
                        .WithMany("FeedbacksRecebidos")
                        .HasForeignKey("DestinatarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Projeto_Backend_IQuirium.Model.Usuario", "Remetente")
                        .WithMany("FeedbacksEnviados")
                        .HasForeignKey("RemetenteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Destinatario");

                    b.Navigation("Remetente");
                });

            modelBuilder.Entity("Projeto_Backend_IQuirium.Model.Usuario", b =>
                {
                    b.Navigation("FeedbacksEnviados");

                    b.Navigation("FeedbacksRecebidos");
                });
#pragma warning restore 612, 618
        }
    }
}
