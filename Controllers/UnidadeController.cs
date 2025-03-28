using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISTEMARH_BACKEND.Models;

namespace SISTEMARH_BACKEND.Controllers
{
    // Define o endpoint base como /api/Unidade
    [ApiController]
    [Route("api/[controller]")]
    public class UnidadeController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Injeta o contexto do banco
        public UnidadeController(AppDbContext context)
        {
            _context = context;
        }

        // GET /api/Unidade
        // Retorna todas as unidades com seus colaboradores e logins
        [HttpGet]
        public IActionResult GetUnidade()
        {
            var unidades = _context.Unidades
                .Include(u => u.Colaboradores)               // Inclui os colaboradores da unidade
                .ThenInclude(c => c.Usuario)                 // Inclui os usuários dos colaboradores
                .Select(u => new {
                    u.Id,
                    u.Nome,
                    u.Codigo,
                    u.Ativo,
                    Colaboradores = u.Colaboradores.Select(c => new {
                        c.Id,
                        c.Nome,
                        Usuario = c.Usuario.Login           // Mostra apenas o login do usuário
                    })
                })
                .ToList();

            return Ok(unidades);
        }

        // POST /api/Unidade
        // Cria uma nova unidade com nome, código e status ativo
        [HttpPost]
        public IActionResult CriarUnidade([FromBody] Unidade unidade)
        {
            _context.Unidades.Add(unidade);
            _context.SaveChanges();

            return Ok(unidade);
        }

        // PUT /api/Unidade/{id}
        // Atualiza nome e status ativo da unidade (o código permanece fixo)
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
