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

        [HttpGet("SolicitarFeedback/{idOrNome}")]
        public async Task<IActionResult> GetFeedback(string idOrNome)
        {
            // Verificar se o parâmetro é um ID (Guid) ou nome
            if (Guid.TryParse(idOrNome, out var id))
            {
                // Obter feedback por ID
                var feedback = await _context.Feedbacks.FindAsync(id);
                if (feedback == null)
                {
                    return NotFound("Feedback não encontrado.");
                }
                return Ok(feedback);
            }
            else
            {
                // Obter feedback por nome do usuário
                var feedback = await _context.Feedbacks
                    .FirstOrDefaultAsync(f => f.Usuario.Nome == idOrNome || f.Destinatario.Nome == idOrNome);
                if (feedback == null)
                {
                    return NotFound("Feedback não encontrado.");
                }
                return Ok(feedback);
            }
        }

        [HttpPost("EnviarFeedback")]
        public async Task<IActionResult> PostFeedback([FromBody] EnviarFeedbackDTO feedbackDTO)
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

            return CreatedAtAction(nameof(GetFeedback), new { id = novoFeedback.Id }, novoFeedback);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound("Feedback não encontrado.");
            }

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
            return NoContent();
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