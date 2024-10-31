using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_Backend_IQuirium.Model;
using Projeto_Backend_IQuirium.Repository;

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
            // Verificar se o parâmetro é um ID (Guid) ou nome
            if (Guid.TryParse(idOrNome, out var id))
            {
                // Obter usuário por ID
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound("Usuário não encontrado.");
                }
                return Ok(usuario);
            }
            else
            {
                // Obter usuário por nome
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nome == idOrNome);
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
            // Criar o novo usuário
            var novoUsuario = new Usuario
            {
                Nome = usuarioDTO.Nome,
                Email = usuarioDTO.Email,
                Criado_em = DateTime.UtcNow
            };
            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();

            // Use a different response type
            return Ok(novoUsuario);
            //return CreatedAtAction(nameof(GetUsuario), new { id = novoUsuario.Id }, novoUsuario);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(Guid id)
        {
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
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}
