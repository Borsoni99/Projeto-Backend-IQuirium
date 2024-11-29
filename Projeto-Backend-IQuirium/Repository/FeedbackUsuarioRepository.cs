using Microsoft.EntityFrameworkCore;
using Projeto_Backend_IQuirium.Model;
using System.Linq.Expressions;

namespace Projeto_Backend_IQuirium.Repository
{
    public class FeedbackUsuarioRepository : Repository<FeedbackUsuario>
    {
        private readonly ProjetoBackendIQuiriumContext _context;

        public FeedbackUsuarioRepository(ProjetoBackendIQuiriumContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<FeedbackUsuario> GetByIdAsync(Guid id)
        {
            return await _context.FeedbacksUsuarios
                .Include(f => f.Remetente)
                .Include(f => f.Destinatario)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public override async Task<IEnumerable<FeedbackUsuario>> GetAllAsync()
        {
            return await _context.FeedbacksUsuarios
                .Include(f => f.Remetente)
                .Include(f => f.Destinatario)
                .ToListAsync();
        }

        public override async Task<IEnumerable<FeedbackUsuario>> FindAsync(Expression<Func<FeedbackUsuario, bool>> predicate)
        {
            return await _context.FeedbacksUsuarios
                .Include(f => f.Remetente)
                .Include(f => f.Destinatario)
                .Where(predicate)
                .ToListAsync();
        }
    }
}