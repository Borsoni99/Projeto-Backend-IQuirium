using Microsoft.AspNetCore.Mvc;
using Projeto_Backend_IQuirium.Model;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
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

            return Ok(feedback);
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
                Status = StatusFeedbackEnum.Pendente
            };

            await _unitOfWork.FeedbackUsuarios.AddAsync(novoFeedback);
            await _unitOfWork.SaveChangesAsync();
            return Ok(novoFeedback);
        }

        [HttpPost("ReportarFeedback/{id}")]
        public async Task<IActionResult> ReportarFeedback(Guid id, [FromBody] ReportarFeedbackUsuarioDTO reportDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var feedback = await _unitOfWork.FeedbackUsuarios.GetByIdAsync(id);
            if (feedback == null)
                return NotFound("Feedback não encontrado.");

            feedback.Motivo = reportDTO.Motivo;
            feedback.ConteudoReport = reportDTO.Conteudo;
            feedback.Status = StatusFeedbackEnum.Reportado;
            _unitOfWork.FeedbackUsuarios.Update(feedback);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new Dictionary<string, string> { { "Message", "Feedback reportado com sucesso." } });
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

    public class EnviarFeedbackUsuarioDTO
    {
        [Required(ErrorMessage = "ID do remetente é obrigatório")]
        [JsonRequired]
        public Guid RemetenteId { get; set; }

        [Required(ErrorMessage = "ID do destinatário é obrigatório")]
        [JsonRequired]
        public Guid DestinatarioId { get; set; }

        [Required(ErrorMessage = "Tipo de feedback é obrigatório")]
        [JsonRequired]
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
        public string Conteudo { get; set; }
    }
}
