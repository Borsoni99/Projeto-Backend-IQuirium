using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto_Backend_IQuirium.Model;
using Microsoft.EntityFrameworkCore.Design;

namespace Projeto_Backend_IQuirium.Repository.Mapping
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuario");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Nome).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(128);

            //builder.Property(x => x.Criado_em).IsRequired().HasColumnType("timestamp").HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(x => x.Criado_em)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp with time zone");
        }
    }
}
