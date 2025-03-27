using Microsoft.AspNetCore.Mvc;
using SISTEMARH_BACKEND.Models;

namespace SISTEMARH_BACKEND.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ColaboradorController : ControllerBase
    {
        private readonly AppDbContext _context;
    
        public ColaboradorController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetColaboradores()
        {
            var colaboradores = _context.Colaboradores
                .Select(c => new {c.Nome, c.Id, Unidade = c.Unidade.Nome})
                .ToList();

            return Ok(colaboradores);
        }

        [HttpPost]
        public IActionResult CriarColaboradores([FromBody] Colaborador colaborador)
        {
            _context.Colaboradores.Add(colaborador);
            _context.SaveChanges();

            return Ok(colaborador);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarColaborador(int id, [FromBody] Colaborador dados)
        {
            var colaborador = _context.Colaboradores.Find(id);
            if (colaborador == null)
                return NotFound("Usuário não encontrado");

            colaborador.Nome = dados.Nome;
            colaborador.UnidadeId = dados.UnidadeId;

            _context.SaveChanges();

            return Ok(colaborador);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarColaborador(int id)
        {
            var colaborador = _context.Colaboradores.Find(id);
            if (colaborador == null)
                return NotFound("Usuário não encontrado");

            _context.Colaboradores.Remove(colaborador);
            _context.SaveChanges();

            return NoContent();
        }
    }
}