using Projeto_Backend_IQuirium.Interfaces;
using Projeto_Backend_IQuirium.Model;

namespace Projeto_Backend_IQuirium.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<FeedbackProduto> FeedbackProdutos { get; }
        IRepository<FeedbackUsuario> FeedbackUsuarios { get; }
        IRepository<Usuario> Usuarios { get; }
        Task<int> SaveChangesAsync();
    }
}

