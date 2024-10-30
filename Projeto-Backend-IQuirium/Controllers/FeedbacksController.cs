using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_Backend_IQuirium.Model;
using Projeto_Backend_IQuirium.Repository;

namespace Projeto_Backend_IQuirium.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbacksController : ControllerBase
    {
        private readonly ProjetoBackendIQuiriumContext _context;

        public FeedbacksController(ProjetoBackendIQuiriumContext context)
        {
            _context = context;
        }

        [HttpGet("SolicitarFeedback/{id}")]
        public async Task<IActionResult> SolicitarFeedback(Guid id)
        {
            // Verificar se o usuário existe
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            // Obter todos os feedbacks do usuário
            var feedbacks = await _context.Feedbacks
                .Where(f => f.Id_usuario == id || f.Id_destinatario == id)
                .ToListAsync();

            return Ok(feedbacks);
        }

        [HttpPost("EnviarFeedback")]
        public async Task<IActionResult> EnviarFeedback([FromBody] EnviarFeedbackDTO feedbackDTO)
        {
            // Verificar se os usuários existem (remetente e destinatário)
            var remetente = await _context.Usuarios.FindAsync(feedbackDTO.Id_usuario);
            var destinatario = await _context.Usuarios.FindAsync(feedbackDTO.Id_destinatario);

            if (remetente == null || destinatario == null)
            {
                return BadRequest("Usuário remetente ou destinatário inválido.");
            }

            // Criar o novo feedback
            var novoFeedback = new Feedback
            {
                Id_usuario = feedbackDTO.Id_usuario,
                Id_destinatario = feedbackDTO.Id_destinatario,
                Tipo_feedback = feedbackDTO.Tipo_feedback,
                Conteudo = feedbackDTO.Conteudo,
                Criado_em = DateTime.Now
            };

            _context.Feedbacks.Add(novoFeedback);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(EnviarFeedback), new { id = novoFeedback.Id }, novoFeedback);
        }
    }

    public class EnviarFeedbackDTO
    {
        public Guid Id_usuario { get; set; }
        public Guid Id_destinatario { get; set; }
        public TipoFeedbackEnum Tipo_feedback { get; set; }
        public string Conteudo { get; set; }
    }
}