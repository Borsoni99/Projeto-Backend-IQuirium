using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto_Backend_IQuirium.Model;
using Projeto_Backend_IQuirium.Repository;

namespace Projeto_Backend_IQuirium.Controllers
{
    public class StatusFeedbacksController : Controller
    {
        private readonly ProjetoBackendIQuiriumContext _context;

        public StatusFeedbacksController(ProjetoBackendIQuiriumContext context)
        {
            _context = context;
        }

        // GET: StatusFeedbacks
        public async Task<IActionResult> Index()
        {
            var projetoBackendIQuiriumContext = _context.StatusFeedbacks.Include(s => s.Feedback);
            return View(await projetoBackendIQuiriumContext.ToListAsync());
        }

        // GET: StatusFeedbacks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusFeedback = await _context.StatusFeedbacks
                .Include(s => s.Feedback)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statusFeedback == null)
            {
                return NotFound();
            }

            return View(statusFeedback);
        }

        // GET: StatusFeedbacks/Create
        public IActionResult Create()
        {
            ViewData["Id_feedback"] = new SelectList(_context.Feedbacks, "Id", "Conteudo");
            return View();
        }

        // POST: StatusFeedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Id_feedback,Status,Atualizado_em")] StatusFeedback statusFeedback)
        {
            if (ModelState.IsValid)
            {
                statusFeedback.Id = Guid.NewGuid();
                _context.Add(statusFeedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id_feedback"] = new SelectList(_context.Feedbacks, "Id", "Conteudo", statusFeedback.Id_feedback);
            return View(statusFeedback);
        }

        // GET: StatusFeedbacks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusFeedback = await _context.StatusFeedbacks.FindAsync(id);
            if (statusFeedback == null)
            {
                return NotFound();
            }
            ViewData["Id_feedback"] = new SelectList(_context.Feedbacks, "Id", "Conteudo", statusFeedback.Id_feedback);
            return View(statusFeedback);
        }

        // POST: StatusFeedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Id_feedback,Status,Atualizado_em")] StatusFeedback statusFeedback)
        {
            if (id != statusFeedback.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statusFeedback);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusFeedbackExists(statusFeedback.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id_feedback"] = new SelectList(_context.Feedbacks, "Id", "Conteudo", statusFeedback.Id_feedback);
            return View(statusFeedback);
        }

        // GET: StatusFeedbacks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusFeedback = await _context.StatusFeedbacks
                .Include(s => s.Feedback)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statusFeedback == null)
            {
                return NotFound();
            }

            return View(statusFeedback);
        }

        // POST: StatusFeedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var statusFeedback = await _context.StatusFeedbacks.FindAsync(id);
            if (statusFeedback != null)
            {
                _context.StatusFeedbacks.Remove(statusFeedback);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatusFeedbackExists(Guid id)
        {
            return _context.StatusFeedbacks.Any(e => e.Id == id);
        }
    }
}
