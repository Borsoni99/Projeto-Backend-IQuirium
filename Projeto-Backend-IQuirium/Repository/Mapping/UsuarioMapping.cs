using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto_Backend_IQuirium.Model;

namespace Projeto_Backend_IQuirium.Repository.Mapping
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuario");

            // Chave primária
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            // Propriedades
            builder.Property(u => u.Nome)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Criado_em)
                .IsRequired()
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp with time zone");

        }
    }
}
