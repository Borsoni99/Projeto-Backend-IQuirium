using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_Backend_IQuirium.Model;
using Projeto_Backend_IQuirium.Repository;

namespace Projeto_Backend_IQuirium.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusFeedbacksController : ControllerBase
    {
        private readonly ProjetoBackendIQuiriumContext _context;

        public StatusFeedbacksController(ProjetoBackendIQuiriumContext context)
        {
            _context = context;
        }

        // GET: api/StatusFeedbacks
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var projetoBackendIQuiriumContext = _context.StatusFeedbacks.Include(s => s.Feedback);
            return Ok(await projetoBackendIQuiriumContext.ToListAsync());
        }

        // GET: api/StatusFeedbacks/Details/5
        [HttpGet("Details/{id}")]
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

            return Ok(statusFeedback);
        }

        // POST: api/StatusFeedbacks/Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create([Bind("Id,Id_feedback,Status,Atualizado_em")] StatusFeedback statusFeedback)
        {
            if (ModelState.IsValid)
            {
                statusFeedback.Id = Guid.NewGuid();
                _context.Add(statusFeedback);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Details), new { id = statusFeedback.Id }, statusFeedback);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/StatusFeedbacks/Edit/5
        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Id_feedback,Status,Atualizado_em")] StatusFeedback statusFeedback)
        {
            if (id != statusFeedback.Id)
            {
                return BadRequest();
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
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/StatusFeedbacks/Delete/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var statusFeedback = await _context.StatusFeedbacks.FindAsync(id);
            if (statusFeedback == null)
            {
                return NotFound();
            }

            _context.StatusFeedbacks.Remove(statusFeedback);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool StatusFeedbackExists(Guid id)
        {
            return _context.StatusFeedbacks.Any(e => e.Id == id);
        }
    }
}
