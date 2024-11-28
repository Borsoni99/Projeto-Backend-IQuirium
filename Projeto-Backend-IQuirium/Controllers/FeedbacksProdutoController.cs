using Microsoft.AspNetCore.Mvc;
using Projeto_Backend_IQuirium.Interfaces;
using Projeto_Backend_IQuirium.Model;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Backend_IQuirium.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbacksProdutoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public FeedbacksProdutoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                var feedback = await _unitOfWork.FeedbackProdutos.GetByIdAsync(id);
                if (feedback == null)
                {
                    return NotFound("Feedback não encontrado.");
                }
                return Ok(feedback);
            }
            else
            {
                var feedbacks = await _unitOfWork.FeedbackProdutos.FindAsync(f => f.Usuario.Nome == idOrNome);
                var feedback = feedbacks.FirstOrDefault();
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

            var novoFeedback = new FeedbackProduto
            {
                Id_usuario = feedbackDTO.Id_usuario,
                Tipo_feedback = feedbackDTO.Tipo_feedback,
                Conteudo = feedbackDTO.Conteudo,
                Criado_em = DateTime.UtcNow
            };

            await _unitOfWork.FeedbackProdutos.AddAsync(novoFeedback);
            await _unitOfWork.SaveChangesAsync();

            return Ok(novoFeedback);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("ID inválido.");
            }

            var feedback = await _unitOfWork.FeedbackProdutos.GetByIdAsync(id);
            if (feedback == null)
            {
                return NotFound("Feedback não encontrado.");
            }

            _unitOfWork.FeedbackProdutos.Delete(feedback);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }
    }

    public class EnviarFeedbackDTO
    {
        [Required(ErrorMessage = "ID do usuário é obrigatório")]
        public Guid Id_usuario { get; set; }

        [Required(ErrorMessage = "Tipo de feedback é obrigatório")]
        public TipoFeedbackEnum Tipo_feedback { get; set; }

        [Required(ErrorMessage = "Conteúdo é obrigatório")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Conteúdo deve ter entre 1 e 1000 caracteres")]
        public string Conteudo { get; set; }
    }
}