using Projeto_Backend_IQuirium.Interfaces;
using Projeto_Backend_IQuirium.Model;

namespace Projeto_Backend_IQuirium.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProjetoBackendIQuiriumContext _context;
        private IRepository<FeedbackProduto> _feedbackProdutos;
        private IRepository<FeedbackUsuario> _feedbackUsuarios;
        private IRepository<Usuario> _usuarios;

        public UnitOfWork(ProjetoBackendIQuiriumContext context)
        {
            _context = context;
        }

        public IRepository<FeedbackProduto> FeedbackProdutos
        {
            get
            {
                _feedbackProdutos ??= new Repository<FeedbackProduto>(_context);
                return _feedbackProdutos;
            }
        }

        public IRepository<FeedbackUsuario> FeedbackUsuarios
        {
            get
            {
                _feedbackUsuarios ??= new FeedbackUsuarioRepository(_context);
                return _feedbackUsuarios;
            }
        }

        public IRepository<Usuario> Usuarios
        {
            get
            {
                _usuarios ??= new Repository<Usuario>(_context);
                return _usuarios;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}