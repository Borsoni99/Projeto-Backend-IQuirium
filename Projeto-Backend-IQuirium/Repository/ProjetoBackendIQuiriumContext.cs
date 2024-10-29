using Microsoft.EntityFrameworkCore;

namespace Projeto_Backend_IQuirium.Repository
{
    public class ProjetoBackendIQuiriumContext : DbContext
    {
        public ProjetoBackendIQuiriumContext(DbContextOptions<ProjetoBackendIQuiriumContext> options)
            : base(options) { }

        public DbSet<Todo> Todos { get; set; }
        public DbSet<Fabricante> Fabricantes { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TodoMapping());
            modelBuilder.ApplyConfiguration(new FabricanteMapping());
            modelBuilder.ApplyConfiguration(new VeiculoMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}
