using Microsoft.AspNetCore.Mvc; // Usado para construir controllers e endpoints da API
using Microsoft.IdentityModel.Tokens; // Para trabalhar com tokens JWT
using System.IdentityModel.Tokens.Jwt; // Para criar e validar JWT
using System.Security.Claims; // Para definir as "claims" (informações) no token
using System.Text;
using SISTEMARH_BACKEND.Models; // Acesso ao modelo de dados e contexto do banco

namespace SISTEMARH_BACKEND.Controllers
{
    // Define que esse controller é uma API e responde por /api/Auth
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Injeta o contexto do banco de dados para buscar usuários
        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // Endpoint POST /api/Auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Busca um usuário ativo com login e senha correspondentes
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Login == request.Login && u.Senha == request.Senha && u.Ativo);

            // Se não encontrou, retorna 401 (não autorizado)
            if (usuario == null)
                return Unauthorized("Login inválido");

            // Cria o token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("chave-super-secreta-muito-forte-1234567890");

            // Define o conteúdo (claims) e validade do token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, usuario.Login) // Adiciona o login como claim
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expira em 1 hora
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature) // Algoritmo de assinatura
            };

            // Gera o token e transforma em string
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Retorna o token para o cliente
            return Ok(new { token = tokenString });
        }
    }

    // Classe usada para receber os dados de login no body da requisição
    public class LoginRequest
    {
        public string Login { get; set; }
        public string Senha { get; set; }
    }
}
