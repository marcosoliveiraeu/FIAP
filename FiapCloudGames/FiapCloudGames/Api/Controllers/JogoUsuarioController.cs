using FiapCloudGames.Application.DTOs;
using FiapCloudGames.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JogosUsuarioController : Controller
    {
        private IJogosUsuarioService _jogosUsuarioService;

        public JogosUsuarioController(IJogosUsuarioService jogosUsuarioService)
        {
            _jogosUsuarioService = jogosUsuarioService;
        }

        /// <summary>
        /// Adiciona um jogo para um usuário
        /// </summary>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] JogosUsuarioAdicionarRequest request)
        {
            await _jogosUsuarioService.AdicionarAsync(request.UsuarioId, request.JogoId);
            return Ok("Jogo atribuído ao usuário");
        }

        /// <summary>
        /// Retorna um registro de jogo do usuário por ID
        /// </summary>
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<JogosUsuarioResponse>> ObterPorId(Guid id)
        {
            var entidade = await _jogosUsuarioService.ObterPorIdAsync(id);

            var response = new JogosUsuarioResponse
            {
                Id = entidade.Id,
                UsuarioId = entidade.UsuarioId,
                JogoId = entidade.JogoId,
                DataAquisicao = entidade.DataAquisicao,
                PrecoPago = entidade.PrecoPago
            };

            return Ok(response);
        }

        /// <summary>
        /// Retorna todos os registros de jogos de usuários
        /// </summary>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogosUsuarioResponse>>> ObterTodos()
        {
            var lista = await _jogosUsuarioService.ObterTodosAsync();

            var response = lista.Select(entidade => new JogosUsuarioResponse
            {
                Id = entidade.Id,
                UsuarioId = entidade.UsuarioId,
                JogoId = entidade.JogoId,
                DataAquisicao = entidade.DataAquisicao,
                PrecoPago = entidade.PrecoPago
            });

            return Ok(response);
        }

        /// <summary>
        /// Retorna todos os jogos adquiridos por um usuário
        /// </summary>
        [Authorize]
        [HttpGet("usuario/{usuarioId:guid}")]
        public async Task<ActionResult<IEnumerable<JogosUsuarioResponse>>> ObterPorUsuarioId(Guid usuarioId)
        {
            var lista = await _jogosUsuarioService.ObterPorUsuarioIdAsync(usuarioId);

            var response = lista.Select(entidade => new JogosUsuarioResponse
            {
                Id = entidade.Id,
                UsuarioId = entidade.UsuarioId,
                JogoId = entidade.JogoId,
                DataAquisicao = entidade.DataAquisicao,
                PrecoPago = entidade.PrecoPago
            });

            return Ok(response);
        }

        /// <summary>
        /// Retorna todos os usuários que possuem determinado jogo
        /// </summary>
        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpGet("jogo/{jogoId:guid}")]
        public async Task<ActionResult<IEnumerable<JogosUsuarioResponse>>> ObterPorJogoId(Guid jogoId)
        {
            var lista = await _jogosUsuarioService.ObterPorJogoIdAsync(jogoId);

            var response = lista.Select(entidade => new JogosUsuarioResponse
            {
                Id = entidade.Id,
                UsuarioId = entidade.UsuarioId,
                JogoId = entidade.JogoId,
                DataAquisicao = entidade.DataAquisicao,
                PrecoPago = entidade.PrecoPago
            });

            return Ok(response);
        }

        /// <summary>
        /// Remove um jogo do usuário
        /// </summary>
        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Remover(Guid id)
        {
            await _jogosUsuarioService.RemoverAsync(id);
            return NoContent();
        }
    }
}
