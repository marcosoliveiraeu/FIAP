using FiapCloudGames.Application.DTOs;
using FiapCloudGames.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromocaoJogoController : Controller
    {
        private IPromocaoJogoService _promocaoJogoService;

        public PromocaoJogoController(IPromocaoJogoService promocaoJogoService)
        {
            _promocaoJogoService = promocaoJogoService;
        }

        /// <summary>
        /// Adiciona um jogo em uma promoção.
        /// </summary>
        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] AdicionarJogoNaPromocaoDto dto)
        {
            await _promocaoJogoService.AdicionarAsync(dto);

            return Ok(new
            {
                Mensagem = "Jogo adicionado à promoção com sucesso."
            });
        }

        /// <summary>
        /// Remove um jogo de uma promoção.
        /// </summary>
        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(Guid id)
        {
            await _promocaoJogoService.RemoverAsync(id);

            return Ok(new
            {
                Mensagem = "Jogo removido da promoção com sucesso."
            });
        }

        /// <summary>
        /// Obtém os detalhes de uma associação por Id.
        /// </summary>
        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var entidade = await _promocaoJogoService.ObterPorIdAsync(id);

            var response = new PromocaoJogoResponseDto
            {
                Id = entidade.Id,
                PromocaoId = entidade.PromocaoId,
                JogoId = entidade.JogoId,
                TituloPromocao = entidade.Promocao.Titulo,
                TituloJogo = entidade.Jogo?.Titulo,
                ValorOriginal = entidade.ValorOriginal,
                ValorComDesconto = entidade.ValorComDesconto,
                DataInclusao = entidade.DataInclusao
            };

            return Ok(response);
        }

        /// <summary>
        /// Lista todos os jogos vinculados a uma promoção.
        /// </summary>
        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpGet("promocao/{promocaoId}")]
        public async Task<IActionResult> ObterPorPromocaoId(Guid promocaoId)
        {
            var lista = await _promocaoJogoService.ObterPorPromocaoIdAsync(promocaoId);

            var response = lista.Select(e => new PromocaoJogoResponseDto
            {
                Id = e.Id,
                PromocaoId = e.PromocaoId,
                JogoId = e.JogoId,
                TituloPromocao = e.Promocao.Titulo,
                TituloJogo = e.Jogo?.Titulo,
                ValorOriginal = e.ValorOriginal,
                ValorComDesconto = e.ValorComDesconto,
                DataInclusao = e.DataInclusao
            });

            return Ok(response);
        }

        /// <summary>
        /// Lista todas as promoções vinculadas a um jogo.
        /// </summary>
        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpGet("jogo/{jogoId}")]
        public async Task<IActionResult> ObterPorJogoId(Guid jogoId)
        {
            var lista = await _promocaoJogoService.ObterPorJogoIdAsync(jogoId);

            var response = lista.Select(e => new PromocaoJogoResponseDto
            {
                Id = e.Id,
                PromocaoId = e.PromocaoId,
                JogoId = e.JogoId,
                TituloPromocao = e.Promocao.Titulo,
                TituloJogo = e.Jogo?.Titulo,
                ValorOriginal = e.ValorOriginal,
                ValorComDesconto = e.ValorComDesconto,
                DataInclusao = e.DataInclusao
            });

            return Ok(response);
        }

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpGet("Todos")]
        public async Task<IActionResult> ObterTodos()
        {
            var promocaoJogos = await _promocaoJogoService.ObterTodosAsync();

            var response = promocaoJogos.Select(pj => new PromocaoJogoResponseDto
            {
                Id = pj.Id,
                PromocaoId = pj.PromocaoId,
                JogoId = pj.JogoId,
                ValorOriginal = pj.ValorOriginal,
                ValorComDesconto = pj.ValorComDesconto,
                DataInclusao = pj.DataInclusao,
                TituloJogo = pj.Jogo?.Titulo,
                TituloPromocao = pj.Promocao?.Titulo
            });

            return Ok(response);
        }
    }
}
