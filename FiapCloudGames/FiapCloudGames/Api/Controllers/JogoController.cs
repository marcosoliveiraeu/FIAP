using FiapCloudGames.Application.DTOs;
using FiapCloudGames.Application.Services.Interfaces;
using FiapCloudGames.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JogoController : Controller
    {
        private IJogoService _jogoService;

        public JogoController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }

        /// <summary>
        /// Cadastrar novo jogo
        /// </summary>
        /// <remarks>
        ///     Cadastro de um novo jogo   
        /// </remarks>
        
        [HttpPost("NovoJogo")]
        [Authorize(Policy = "ADMINISTRADOR")]
        public async Task<IActionResult> Criar([FromBody] CriarJogoDto criarJogoDto)
        {

            var jogo = await _jogoService.AdicionarAsync(criarJogoDto);

            var response = new 
            {
                Mensagem = "Jogo cadastrado com sucesso!",
                Id = jogo.Id,
                Titulo = jogo.Titulo,
                Categoria = jogo.Categoria,
                Preco = jogo.Preco,
                DataLancamento = jogo.DataLancamento
            };

            return Ok(response);
        }

        /// <summary>
        /// Obter jogo por ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<JogoResponseDto>> ObterPorId(Guid id)
        {
            var jogo = await _jogoService.ObterPorIdAsync(id);

            if (jogo == null)
                return NotFound(new { mensagem = "Jogo não encontrado." });

            return Ok(jogo);
        }

        /// <summary>
        /// Listar todos os jogos
        /// </summary>        
        [HttpGet("BuscarTodos")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<JogoResponseDto>>> ObterTodos()
        {
            var jogos = await _jogoService.ObterTodosAsync();

            return Ok(jogos);
        }

        /// <summary>
        /// Atualizar jogo
        /// </summary>        
        [HttpPut("{id}")]
        [Authorize(Policy = "ADMINISTRADOR")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarJogoDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id da URL diferente do body.");
            

            await _jogoService.AtualizarAsync(dto.Id, dto.Titulo, dto.Categoria,dto.Preco,dto.DataLancamento);

            var response = new
            {
                Mensagem = "Jogo atualizado com sucesso.",
                Id = dto.Id,
                Titulo = dto.Titulo,
                Categoria = dto.Categoria,
                Preco = dto.Preco,
                DataLancamento = dto.DataLancamento
            };


            return Ok(response);
        }

        /// <summary>
        /// Remover jogo
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "ADMINISTRADOR")]        
        public async Task<IActionResult> Remover(Guid id)
        {
            var jogo = await _jogoService.ObterPorIdAsync(id);
            if (jogo == null)
                return NotFound();

            await _jogoService.RemoverAsync(id);

            var response = new 
            {
                Mensagem = "Jogo excluído sucesso!",
                Id = jogo.Id,
                Titulo = jogo.Titulo
            };

            return Ok(response);
        }
    }
}
