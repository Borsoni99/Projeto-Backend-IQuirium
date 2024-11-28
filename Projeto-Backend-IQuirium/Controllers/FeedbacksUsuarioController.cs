using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_Backend_IQuirium.Model;
using Projeto_Backend_IQuirium.Repository;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Backend_IQuirium.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbacksUsuarioController : ControllerBase
    {
        private readonly ProjetoBackendIQuiriumContext _context;

        public FeedbacksUsuarioController(ProjetoBackendIQuiriumContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obter feedbacks de um usuário (por ID ou Nome)
        /// </summary>
        [HttpGet("SolicitarFeedback/{idOrNome}")]
        public async Task<IActionResult> GetFeedback(string idOrNome)
        {
            if (string.IsNullOrWhiteSpace(idOrNome))
            {
                return BadRequest("ID ou nome não pode estar vazio.");
            }

            if (Guid.TryParse(idOrNome, out var id))
            {
                var feedback = await _context.FeedbacksUsuarios
                    .Include(f => f.Remetente)
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
                var feedback = await _context.FeedbacksUsuarios
                    .Include(f => f.Remetente)
                    .Include(f => f.Destinatario)
                    .FirstOrDefaultAsync(f => f.Destinatario.Nome == idOrNome);

                if (feedback == null)
                {
                    return NotFound("Feedback não encontrado.");
                }
                return Ok(feedback);
            }
        }

        /// <summary>
        /// Enviar feedback a outro usuário
        /// </summary>
        [HttpPost("EnviarFeedback")]
        public async Task<IActionResult> PostFeedback([FromBody] EnviarFeedbackUsuarioDTO feedbackDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar se o remetente e destinatário existem
            var remetente = await _context.Usuarios.FindAsync(feedbackDTO.RemetenteId);
            var destinatario = await _context.Usuarios.FindAsync(feedbackDTO.DestinatarioId);

            if (remetente == null)
            {
                return BadRequest("Usuário remetente não encontrado.");
            }

            if (destinatario == null)
            {
                return BadRequest("Usuário destinatário não encontrado.");
            }

            // Criar o novo feedback
            var novoFeedback = new FeedbackUsuario
            {
                Id = Guid.NewGuid(),
                RemetenteId = feedbackDTO.RemetenteId,
                DestinatarioId = feedbackDTO.DestinatarioId,
                Tipo = feedbackDTO.Tipo,
                ConteudoFeedback = feedbackDTO.Conteudo,
                DataHoraEnvio = DateTime.UtcNow,
                Status = StatusFeedbackEnum.Pendente
            };

            _context.FeedbacksUsuarios.Add(novoFeedback);
            await _context.SaveChangesAsync();

            // Carregar dados do remetente e destinatário para o retorno
            await _context.Entry(novoFeedback)
                .Reference(f => f.Remetente)
                .LoadAsync();

            await _context.Entry(novoFeedback)
                .Reference(f => f.Destinatario)
                .LoadAsync();

            return Ok(novoFeedback);
        }

        /// <summary>
        /// Reportar um feedback recebido
        /// </summary>
        [HttpPost("ReportarFeedback/{id}")]
        public async Task<IActionResult> ReportarFeedback(Guid id, [FromBody] ReportarFeedbackUsuarioDTO reportDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feedback = await _context.FeedbacksUsuarios.FindAsync(id);
            if (feedback == null)
            {
                return NotFound("Feedback não encontrado.");
            }

            feedback.Motivo = reportDTO.Motivo;
            feedback.ConteudoReport = reportDTO.Conteudo;
            feedback.Status = StatusFeedbackEnum.Reportado;


            await _context.SaveChangesAsync();

            return Ok(new { Message = "Feedback reportado com sucesso." });
        }

        /// <summary>
        /// Deletar um feedback
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("ID inválido.");
            }

            var feedback = await _context.FeedbacksUsuarios.FindAsync(id);
            if (feedback == null)
            {
                return NotFound("Feedback não encontrado.");
            }

            _context.FeedbacksUsuarios.Remove(feedback);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    /// <summary>
    /// DTO para envio de feedback entre usuários
    /// </summary>
    public class EnviarFeedbackUsuarioDTO
    {
        [Required(ErrorMessage = "ID do remetente é obrigatório")]
        public Guid RemetenteId { get; set; }

        [Required(ErrorMessage = "ID do destinatário é obrigatório")]
        public Guid DestinatarioId { get; set; }

        [Required(ErrorMessage = "Tipo de feedback é obrigatório")]
        public TipoFeedbackEnum Tipo { get; set; }

        [Required(ErrorMessage = "Conteúdo é obrigatório")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Conteúdo deve ter entre 1 e 1000 caracteres")]
        public string Conteudo { get; set; }
    }

    /// <summary>
    /// DTO para reportar feedback entre usuários
    /// </summary>
    public class ReportarFeedbackUsuarioDTO
    {
        [Required(ErrorMessage = "Motivo é obrigatório")]
        public string Motivo { get; set; }

        [StringLength(1000, ErrorMessage = "Conteúdo do report deve ter no máximo 1000 caracteres")]
        public string Conteudo { get; set; }
    }
}
