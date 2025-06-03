using FiapCloudGames.Api.Utils;
using FiapCloudGames.Application.DTOs;
using FiapCloudGames.Application.Services.Interfaces;
using FiapCloudGames.Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FiapCloudGames.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly TokenService _tokenService;
        private readonly ISenhaHasher _senhaHasher;
        private readonly IConfiguration _configuration;

        public LoginController(IUsuarioRepository usuarioRepository, TokenService tokenService, ISenhaHasher senhaHasher, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
            _senhaHasher = senhaHasher;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);

            if (usuario == null || usuario.SenhaHash != _senhaHasher.Hash(request.Senha)) 
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }

            var token = _tokenService.GenerateToken(usuario);

            return Ok(new{ token });
        }



        [HttpGet("publico")]
        [AllowAnonymous]
        public IActionResult TestePublico()
        {
            return Ok("Acesso público: qualquer um pode acessar.");
        }

        [HttpGet("autenticado")]
        [Authorize]
        public IActionResult TesteAutenticado()
        {
            return Ok($"Acesso autenticado: usuário {User.Identity?.Name} está autenticado.");
        }

        [HttpGet("usuario")]
        [Authorize(Roles = "USUARIO,ADMINISTRADOR")]
        public IActionResult TesteUsuario()
        {
            return Ok("Acesso restrito a usuários com perfil USUARIO ou ADMINISTRADOR");
        }

        [HttpGet("admin")]
        [Authorize(Policy = "ADMINISTRADOR")]
        public IActionResult TesteAdministrador()
        {
            return Ok("Acesso restrito: apenas usuários com perfil ADMINISTRADOR.");
        }


    }

}
