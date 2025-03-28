using Microsoft.AspNetCore.Mvc;
using SISTEMARH_BACKEND.Models;

namespace SISTEMARH_BACKEND.Controllers
{
    // Define o endpoint base como /api/Usuario
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Injeta o contexto do banco
        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        // GET /api/Usuario?status=true/false (ou sem filtro)
        // Lista todos os usuários ou filtra por status (ativo/inativo)
        [HttpGet]
        public IActionResult GetUsuarios([FromQuery] bool? status)
        {
            if (status == null)
            {
                var usuarios = _context.Usuarios
                    .Select(u => new { u.Id, u.Login, u.Ativo })
                    .ToList();

                return Ok(usuarios);
            }
            else
            {
                var usuariosFiltrados = _context.Usuarios
                    .Where(u => u.Ativo == status)
                    .Select(u => new { u.Id, u.Login, u.Ativo })
                    .ToList();

                return Ok(usuariosFiltrados);
            }
        }

        // POST /api/Usuario
        // Cria um novo usuário
        [HttpPost]
        public IActionResult CriarUsuario([FromBody] Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUsuarios), new { id = usuario.Id }, usuario);
        }

        // PUT /api/Usuario/{id}
        // Atualiza a senha e o status do usuário
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

        // DELETE /api/Usuario/{id}
        // Remove o usuário do banco (remoção física)
        [HttpDelete("{id}")]
        public IActionResult DeletarUsuario(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
                return NotFound("Usuário não encontrado");

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();

            return NoContent(); // 204
        }
    }
}
