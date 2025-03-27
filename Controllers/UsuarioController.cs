using Microsoft.AspNetCore.Mvc;
using SISTEMARH_BACKEND.Models;

namespace SISTEMARH_BACKEND.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUsuarios()
        {
            var usuarios = _context.Usuarios
                .Select(u => new { u.Id, u.Login, u.Ativo })
                .ToList();

            return Ok(usuarios);
        }

        [HttpPost]
        public IActionResult CriarUsuario([FromBody] Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUsuarios), new { id = usuario.Id }, usuario);
        }
        [HttpPut("{id}")]
        public IActionResult AtualizarUsuario(int id, [FromBody] Usuario dados)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
                return NotFound("Usuário não encontrado");

            usuario.Senha = dados.Senha;
            usuario.Ativo = dados.Ativo;

            _context.SaveChanges();
            return Ok(usuario);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarUsuario(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
                return NotFound("Usuário não encontrado");

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
