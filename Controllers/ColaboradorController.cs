using Microsoft.AspNetCore.Mvc;
using SISTEMARH_BACKEND.Models;
using Microsoft.EntityFrameworkCore;

namespace SISTEMARH_BACKEND.Controllers
{
    // Define que esse controller é uma API e responde por /api/Colaborador
    [ApiController]
    [Route("api/[Controller]")]
    public class ColaboradorController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Injeta o contexto do banco para acessar os dados
        public ColaboradorController(AppDbContext context)
        {
            _context = context;
        }

        // GET /api/Colaborador
        // Retorna todos os colaboradores com dados resumidos (nome, id, usuário e unidade)
        [HttpGet]
        public IActionResult GetColaboradores()
        {
            var colaboradores = _context.Colaboradores
                .Select(c => new
                {
                    c.Nome,
                    c.Id,
                    UsuarioId = c.UsuarioId,
                    Unidade = c.Unidade.Nome
                })
                .ToList();

            return Ok(colaboradores);
        }

        // POST /api/Colaborador
        // Cadastra um novo colaborador, validando se a unidade está ativa
        [HttpPost]
        public IActionResult CriarColaboradores([FromBody] Colaborador colaborador)
        {
            var unidade = _context.Unidades.Find(colaborador.UnidadeId);

            // Se unidade não existe ou está inativa, retorna erro
            if (unidade == null || !unidade.Ativo)
                return BadRequest("Não é possível cadastrar colaboradores em unidade inativa");

            _context.Colaboradores.Add(colaborador);
            _context.SaveChanges();

            return Ok(colaborador);
        }

        // PUT /api/Colaborador/{id}
        // Atualiza nome e unidade de um colaborador existente
        [HttpPut("{id}")]
        public IActionResult AtualizarColaborador(int id, [FromBody] Colaborador dados)
        {
            // Valida se a nova unidade existe e está ativa
            var unidade = _context.Unidades.Find(dados.UnidadeId);
            if (unidade == null || !unidade.Ativo)
                return BadRequest("Não é possível cadastrar colaboradores em unidade inativa");

            // Busca o colaborador no banco
            var colaborador = _context.Colaboradores.Find(id);
            if (colaborador == null)
                return NotFound("Usuário não encontrado");

            // Atualiza os dados
            colaborador.Nome = dados.Nome;
            colaborador.UnidadeId = dados.UnidadeId;

            _context.SaveChanges();

            return Ok(colaborador);
        }

        // DELETE /api/Colaborador/{id}
        // Remove colaborador do banco (remoção física)
        [HttpDelete("{id}")]
        public IActionResult DeletarColaborador(int id)
        {
            var colaborador = _context.Colaboradores.Find(id);
            if (colaborador == null)
                return NotFound("Usuário não encontrado");

            _context.Colaboradores.Remove(colaborador);
            _context.SaveChanges();

            return NoContent(); // Retorna 204 (sem conteúdo)
        }
    }
}
