using Projeto_Backend_IQuirium.Interfaces;
using Projeto_Backend_IQuirium.Model;

namespace Projeto_Backend_IQuirium.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProjetoBackendIQuiriumContext _context;
        private IRepository<FeedbackProduto> _feedbackProdutos;

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