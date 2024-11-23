using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto_Backend_IQuirium.Model;
using Microsoft.EntityFrameworkCore.Design;

namespace Projeto_Backend_IQuirium.Repository.Mapping
{
    public class FeedbackProdutoMapping : IEntityTypeConfiguration<FeedbackProduto>
    {
        public void Configure(EntityTypeBuilder<FeedbackProduto> builder)
        {
            builder.ToTable("feedback");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Tipo_feedback);
            builder.Property(x => x.Conteudo).IsRequired().HasMaxLength(2048);
            builder.Property(x => x.Criado_em)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp with time zone");

            builder.HasOne(f => f.Usuario).WithMany().HasForeignKey(f => f.Id_usuario);
        }
    }
}