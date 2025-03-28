using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISTEMARH_BACKEND.Models;

namespace SISTEMARH_BACKEND.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnidadeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UnidadeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUnidade()
        {
            var unidades = _context.Unidades
                .Include(u => u.Colaboradores)
                .ThenInclude(c => c.Usuario)
                .Select(u => new {
                    u.Id,
                    u.Nome,
                    u.Codigo,
                    u.Ativo,
                    Colaboradores = u.Colaboradores.Select(c => new {
                        c.Id,
                        c.Nome,
                        Usuario = c.Usuario.Login
                    })
                })
                .ToList();

            return Ok(unidades);
        }

        [HttpPost]
        public IActionResult CriarUnidade([FromBody] Unidade unidade)
        {
            _context.Unidades.Add(unidade);
            _context.SaveChanges();

            return Ok(unidade);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizaUnidade(int id, [FromBody] Unidade dados)
        {
            var unidade = _context.Unidades.Find(id);
            if (unidade == null)
                return NotFound("Unidade não encontrada");

            unidade.Nome = dados.Nome;
            unidade.Ativo = dados.Ativo;
            _context.SaveChanges();

            return Ok(unidade);
        }
    }
}