using Microsoft.AspNetCore.Mvc;
using Projeto_Backend_IQuirium.Model;
using System.ComponentModel.DataAnnotations;
using Projeto_Backend_IQuirium.Interfaces;

namespace Projeto_Backend_IQuirium.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbacksUsuarioController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public FeedbacksUsuarioController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("SolicitarFeedback/{idOrNome}")]
        public async Task<IActionResult> GetFeedback(string idOrNome)
        {
            if (string.IsNullOrWhiteSpace(idOrNome))
                return BadRequest("ID ou nome não pode estar vazio.");

            FeedbackUsuario feedback;
            if (Guid.TryParse(idOrNome, out var id))
            {
                feedback = await _unitOfWork.FeedbackUsuarios.GetByIdAsync(id);
            }
            else
            {
                feedback = (await _unitOfWork.FeedbackUsuarios
                    .FindAsync(f => f.Destinatario.Nome == idOrNome)).FirstOrDefault();
            }

            if (feedback == null)
                return NotFound("Feedback não encontrado.");

            var feedbackDTO = new FeedbackUsuarioResponseDTO
            {
                Id = feedback.Id,
                RemetenteId = feedback.RemetenteId,
                DestinatarioId = feedback.DestinatarioId,
                Tipo = feedback.Tipo,
                ConteudoFeedback = feedback.ConteudoFeedback,
                DataHoraEnvio = feedback.DataHoraEnvio,
                Status = feedback.Status,
                Motivo = feedback.Motivo ?? "Não reportado",
                ConteudoReport = feedback.ConteudoReport,
                RemetenteNome = feedback.Remetente?.Nome ?? "Usuário não encontrado",
                DestinatarioNome = feedback.Destinatario?.Nome ?? "Usuário não encontrado"
            };

            return Ok(feedbackDTO);
        }

        [HttpPost("EnviarFeedback")]
        public async Task<IActionResult> PostFeedback([FromBody] EnviarFeedbackUsuarioDTO feedbackDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var remetente = await _unitOfWork.Usuarios.GetByIdAsync(feedbackDTO.RemetenteId);
            var destinatario = await _unitOfWork.Usuarios.GetByIdAsync(feedbackDTO.DestinatarioId);

            if (remetente == null)
                return BadRequest("Usuário remetente não encontrado.");

            if (destinatario == null)
                return BadRequest("Usuário destinatário não encontrado.");

            var novoFeedback = new FeedbackUsuario
            {
                Id = Guid.NewGuid(),
                RemetenteId = feedbackDTO.RemetenteId,
                DestinatarioId = feedbackDTO.DestinatarioId,
                Tipo = feedbackDTO.Tipo,
                ConteudoFeedback = feedbackDTO.Conteudo,
                DataHoraEnvio = DateTime.UtcNow,
                Status = StatusFeedbackEnum.Pendente,
                Motivo = "Não reportado",
                ConteudoReport = string.Empty
            };

            await _unitOfWork.FeedbackUsuarios.AddAsync(novoFeedback);
            await _unitOfWork.SaveChangesAsync();

            var responseDTO = new FeedbackUsuarioResponseDTO
            {
                Id = novoFeedback.Id,
                RemetenteId = novoFeedback.RemetenteId,
                DestinatarioId = novoFeedback.DestinatarioId,
                Tipo = novoFeedback.Tipo,
                ConteudoFeedback = novoFeedback.ConteudoFeedback,
                DataHoraEnvio = novoFeedback.DataHoraEnvio,
                Status = novoFeedback.Status,
                Motivo = novoFeedback.Motivo,
                ConteudoReport = novoFeedback.ConteudoReport,
                RemetenteNome = remetente.Nome,
                DestinatarioNome = destinatario.Nome
            };

            return Ok(responseDTO);
        }

        [HttpPost("ReportarFeedback/{id}")]
        public async Task<IActionResult> ReportarFeedback(Guid id, [FromBody] ReportarFeedbackUsuarioDTO reportDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var feedback = await _unitOfWork.FeedbackUsuarios.GetByIdAsync(id);
            if (feedback == null)
                return NotFound("Feedback não encontrado.");

            if (string.IsNullOrWhiteSpace(reportDTO.Motivo))
                return BadRequest("Motivo é obrigatório.");

            feedback.Motivo = reportDTO.Motivo;
            feedback.ConteudoReport = string.IsNullOrWhiteSpace(reportDTO.Conteudo)
                ? "Sem detalhes adicionais."
                : reportDTO.Conteudo;
            feedback.Status = StatusFeedbackEnum.Reportado;

            _unitOfWork.FeedbackUsuarios.Update(feedback);
            await _unitOfWork.SaveChangesAsync();

            var responseDTO = new FeedbackUsuarioResponseDTO
            {
                Id = feedback.Id,
                RemetenteId = feedback.RemetenteId,
                DestinatarioId = feedback.DestinatarioId,
                Tipo = feedback.Tipo,
                ConteudoFeedback = feedback.ConteudoFeedback,
                DataHoraEnvio = feedback.DataHoraEnvio,
                Status = feedback.Status,
                Motivo = feedback.Motivo,
                ConteudoReport = feedback.ConteudoReport,
                RemetenteNome = feedback.Remetente?.Nome ?? "Usuário não encontrado",
                DestinatarioNome = feedback.Destinatario?.Nome ?? "Usuário não encontrado"
            };

            return Ok(responseDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("ID inválido.");

            var feedback = await _unitOfWork.FeedbackUsuarios.GetByIdAsync(id);
            if (feedback == null)
                return NotFound("Feedback não encontrado.");

            _unitOfWork.FeedbackUsuarios.Delete(feedback);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }

    public class FeedbackUsuarioResponseDTO
    {
        public Guid Id { get; set; }
        public Guid RemetenteId { get; set; }
        public Guid DestinatarioId { get; set; }
        public string RemetenteNome { get; set; }
        public string DestinatarioNome { get; set; }
        public TipoFeedbackEnum Tipo { get; set; }
        public string ConteudoFeedback { get; set; }
        public DateTime DataHoraEnvio { get; set; }
        public StatusFeedbackEnum Status { get; set; }
        public string Motivo { get; set; }
        public string? ConteudoReport { get; set; }
    }

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

    public class ReportarFeedbackUsuarioDTO
    {
        [Required(ErrorMessage = "Motivo é obrigatório")]
        public string Motivo { get; set; }

        [StringLength(1000, ErrorMessage = "Conteúdo do report deve ter no máximo 1000 caracteres")]
        public string? Conteudo { get; set; }
    }
}
