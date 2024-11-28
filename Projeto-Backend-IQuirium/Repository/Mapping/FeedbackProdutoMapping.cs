using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Projeto_Backend_IQuirium.Model;

namespace Projeto_Backend_IQuirium.Repository.Mapping
{
    public class FeedbackProdutoMapping : IEntityTypeConfiguration<FeedbackProduto>
    {
        public void Configure(EntityTypeBuilder<FeedbackProduto> builder)
        {
            builder.ToTable("feedback_produto");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Tipo_feedback)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Conteudo)
                .IsRequired()
                .HasMaxLength(2048);

            builder.Property(x => x.Criado_em)
                .IsRequired()
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp with time zone");

            builder.HasOne(f => f.Usuario)
                .WithMany()
                .HasForeignKey(f => f.Id_usuario);


        }
    }
}