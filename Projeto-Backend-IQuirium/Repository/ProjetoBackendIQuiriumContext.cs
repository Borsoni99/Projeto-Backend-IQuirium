using Microsoft.EntityFrameworkCore;
using Projeto_Backend_IQuirium.Model;
using Projeto_Backend_IQuirium.Repository.Mapping;

namespace Projeto_Backend_IQuirium.Repository
{
    public class ProjetoBackendIQuiriumContext : DbContext
    {
        public ProjetoBackendIQuiriumContext(DbContextOptions<ProjetoBackendIQuiriumContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioMapping());
            modelBuilder.ApplyConfiguration(new FeedbackMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}
