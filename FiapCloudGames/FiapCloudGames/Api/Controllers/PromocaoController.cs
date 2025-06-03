using FiapCloudGames.Application.DTOs;
using FiapCloudGames.Application.Services.Interfaces;
using FiapCloudGames.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromocaoController : Controller
    {
        private IPromocaoService _promocaoService;

        public PromocaoController(IPromocaoService promocaoService)
        {
            _promocaoService = promocaoService;
        }

       
        [HttpGet("BuscarTodos")]
        [Authorize(Policy = "ADMINISTRADOR")]
        public async Task<IActionResult> ObterTodos()
        {
            var promocoes = await _promocaoService.ObterTodosAsync();
            var response = promocoes.Select(p => new PromocaoResponseDto
            {
                Id = p.Id,
                Titulo = p.Titulo,
                Descricao = p.Descricao,
                Status = p.Status,
                PercentualDesconto = p.PercentualDesconto,
                DataInclusao = p.DataInclusao,
                DataValidade = p.DataValidade,
                DataAtualizacao = p.DataAtualizacao,
                Jogos = p.Jogos.Select(j => new PromocaoJogoResponseDto
                {
                    Id = j.Id,
                    JogoId = j.JogoId,
                    TituloJogo = j.Jogo.Titulo,
                    ValorOriginal = j.ValorOriginal,
                    ValorComDesconto = j.ValorComDesconto
                }).ToList()
            });

            return Ok(response);
        }

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var promocao = await _promocaoService.ObterPorIdAsync(id);
            if (promocao == null)
                return NotFound(new { mensagem = "Promoção não encontrada." });

            return Ok(promocao);
        }

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpPost("NovaPromocao")]
        public async Task<IActionResult> Criar([FromBody] CriarPromocaoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var promocao = new Promocao
            {
                Id = Guid.NewGuid(),
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Status = dto.Status,
                PercentualDesconto = dto.PercentualDesconto,
                DataInclusao = DateTime.UtcNow,
                DataValidade = dto.DataValidade,
                DataAtualizacao = DateTime.UtcNow
            };

            await _promocaoService.AdicionarAsync(promocao);

            return CreatedAtAction(nameof(ObterPorId), new { id = promocao.Id }, new
            {
                mensagem = "Promoção criada com sucesso.",
                promocao.Id,
                promocao.Titulo,
                promocao.Descricao, 
                promocao.Status,
                promocao.PercentualDesconto,
                promocao.DataInclusao,
                promocao.DataValidade
                
            });
        }

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] Promocao promocao)
        {
            if (id != promocao.Id)
                return BadRequest("O Id da URL não corresponde ao do body.");


            await _promocaoService.AtualizarAsync(promocao);

            return Ok(new
            {
                mensagem = "Promoção atualizada com sucesso.",
                promocao
            });
        }

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(Guid id)
        {
            var promocao = await _promocaoService.ObterPorIdAsync(id);
            if (promocao == null)
                return NotFound(new { mensagem = "Promoção não encontrada." });

            await _promocaoService.RemoverAsync(id);

            return Ok(new
            {
                mensagem = "Promoção removida com sucesso.",
                promocao.Id,
                promocao.Titulo
            });
        }




    }
}
