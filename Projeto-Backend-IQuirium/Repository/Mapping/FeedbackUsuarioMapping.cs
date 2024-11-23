using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto_Backend_IQuirium.Model;

namespace Projeto_Backend_IQuirium.Repository.Mapping
{
    public class FeedbackUsuarioMapping : IEntityTypeConfiguration<FeedbackUsuario>
    {
        public void Configure(EntityTypeBuilder<FeedbackUsuario> builder)
        {
            builder.ToTable("feedback");

            // Chave primária
            builder.HasKey(f => f.Id);
            builder.Property(f => f.Id).ValueGeneratedOnAdd();

            // Propriedades
            builder.Property(f => f.RemetenteId).IsRequired();

            builder.Property(f => f.DestinatarioId).IsRequired();

            builder.Property(f => f.Tipo);

            builder.Property(f => f.ConteudoFeedback).IsRequired().HasMaxLength(2048);

            builder.Property(f => f.DataHoraEnvio)
                .IsRequired()
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp with time zone");

            builder.Property(f => f.Status).HasMaxLength(255);

            builder.Property(f => f.Motivo).HasMaxLength(1024);

            builder.Property(f => f.ConteudoReport).HasMaxLength(1024);

            // Relacionamentos
            builder.HasOne(f => f.Remetente)
                .WithMany(u => u.FeedbacksEnviados)
                .HasForeignKey(f => f.RemetenteId);

            builder.HasOne(f => f.Destinatario)
                .WithMany(u => u.FeedbacksRecebidos)
                .HasForeignKey(f => f.DestinatarioId);
        }
    }
}
