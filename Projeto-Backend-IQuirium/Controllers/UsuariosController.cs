using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Projeto_Backend_IQuirium.Model;
using Projeto_Backend_IQuirium.Repository;
using Newtonsoft.Json;

namespace Projeto_Backend_IQuirium.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly ProjetoBackendIQuiriumContext _context;

        public UsuariosController(ProjetoBackendIQuiriumContext context)
        {
            _context = context;
        }

        [HttpGet("{idOrNome}")]
        public async Task<IActionResult> GetUsuario(string idOrNome)
        {
            if (string.IsNullOrWhiteSpace(idOrNome))
            {
                return BadRequest("ID ou nome não pode estar vazio.");
            }

            if (Guid.TryParse(idOrNome, out var id))
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound("Usuário não encontrado.");
                }
                return Ok(usuario);
            }
            else
            {
                if (idOrNome.Length < 2)
                {
                    return BadRequest("Nome deve ter pelo menos 2 caracteres.");
                }

                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Nome == idOrNome);

                if (usuario == null)
                {
                    return NotFound("Usuário não encontrado.");
                }
                return Ok(usuario);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostUsuario([FromBody] CriarUsuarioDTO usuarioDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validar se já existe um usuário com o mesmo email
            var emailExists = await _context.Usuarios
                .AnyAsync(u => u.Email.ToLower() == usuarioDTO.Email.ToLower());
            if (emailExists)
            {
                return BadRequest("Email já está em uso.");
            }

            // Validar se já existe um usuário com o mesmo nome
            var nomeExists = await _context.Usuarios
                .AnyAsync(u => u.Nome.ToLower() == usuarioDTO.Nome.ToLower());
            if (nomeExists)
            {
                return BadRequest("Nome já está em uso.");
            }

            // Criar o novo usuário
            var novoUsuario = new Usuario
            {
                Nome = usuarioDTO.Nome.Trim(),
                Email = usuarioDTO.Email.Trim().ToLower(),
                Criado_em = DateTime.UtcNow
            };

            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario),
                new { idOrNome = novoUsuario.Id },
                novoUsuario);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("ID inválido.");
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class CriarUsuarioDTO
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Nome deve conter apenas letras, números e espaços")]
        [JsonRequired]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(256, ErrorMessage = "Email não pode ter mais que 256 caracteres")]
        [JsonRequired]
        public string Email { get; set; }
    }
}