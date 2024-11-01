using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_Backend_IQuirium.Model;
using Projeto_Backend_IQuirium.Repository;
using System.ComponentModel.DataAnnotations;

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
            if (string.IsNullOrWhiteSpace(idOrNome))
            {
                return BadRequest("ID ou nome não pode estar vazio.");
            }

            if (Guid.TryParse(idOrNome, out var id))
            {
                var feedback = await _context.Feedbacks
                    .Include(f => f.Usuario)
                    .Include(f => f.Destinatario)
                    .FirstOrDefaultAsync(f => f.Id == id);

                if (feedback == null)
                {
                    return NotFound("Feedback não encontrado.");
                }
                return Ok(feedback);
            }
            else
            {
                var feedback = await _context.Feedbacks
                    .Include(f => f.Usuario)
                    .Include(f => f.Destinatario)
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (feedbackDTO.Id_usuario == feedbackDTO.Id_destinatario)
            {
                return BadRequest("Usuário não pode enviar feedback para si mesmo.");
            }

            // Verificar se os usuários existem (remetente e destinatário)
            var remetente = await _context.Usuarios.FindAsync(feedbackDTO.Id_usuario);
            var destinatario = await _context.Usuarios.FindAsync(feedbackDTO.Id_destinatario);

            if (remetente == null)
            {
                return BadRequest("Usuário remetente não encontrado.");
            }

            if (destinatario == null)
            {
                return BadRequest("Usuário destinatário não encontrado.");
            }

            // Criar o novo feedback
            var novoFeedback = new Feedback
            {
                Id_usuario = feedbackDTO.Id_usuario,
                Usuario = remetente,  
                Id_destinatario = feedbackDTO.Id_destinatario,
                Destinatario = destinatario,  
                Tipo_feedback = feedbackDTO.Tipo_feedback,
                Conteudo = feedbackDTO.Conteudo,
                Criado_em = DateTime.UtcNow
            };

            _context.Feedbacks.Add(novoFeedback);
            await _context.SaveChangesAsync();


            await _context.Entry(novoFeedback)
                .Reference(f => f.Usuario)
                .LoadAsync();

            await _context.Entry(novoFeedback)
                .Reference(f => f.Destinatario)
                .LoadAsync();

            return Ok(novoFeedback);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("ID inválido.");
            }

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
        [Required(ErrorMessage = "ID do usuário é obrigatório")]
        public Guid Id_usuario { get; set; }

        [Required(ErrorMessage = "ID do destinatário é obrigatório")]
        public Guid Id_destinatario { get; set; }

        [Required(ErrorMessage = "Tipo de feedback é obrigatório")]
        public TipoFeedbackEnum Tipo_feedback { get; set; }

        [Required(ErrorMessage = "Conteúdo é obrigatório")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Conteúdo deve ter entre 1 e 1000 caracteres")]
        public string Conteudo { get; set; }
    }
}