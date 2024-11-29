using Microsoft.EntityFrameworkCore;
using Projeto_Backend_IQuirium.Model;
using Projeto_Backend_IQuirium.Repository.Mapping;

namespace Projeto_Backend_IQuirium.Repository
{
    public class ProjetoBackendIQuiriumContext : DbContext
    {
        public ProjetoBackendIQuiriumContext(DbContextOptions<ProjetoBackendIQuiriumContext> options)
            : base(options) { }

        public virtual DbSet<Usuario> Usuarios { get; set; }
        public DbSet<FeedbackProduto> FeedbacksProdutos { get; set; }
        public virtual DbSet<FeedbackUsuario> FeedbacksUsuarios { get; set; }
        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioMapping());
            modelBuilder.ApplyConfiguration(new FeedbackProdutoMapping());
            modelBuilder.ApplyConfiguration(new FeedbackUsuarioMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}
