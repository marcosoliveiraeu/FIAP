using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Enuns;

namespace FiapCloudGames.Application.DTOs
{
    public class PromocaoResponseDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public StatusPromocao Status { get; set; }
        public decimal PercentualDesconto { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataValidade { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public List<PromocaoJogoResponseDto> Jogos { get; set; } = new();
    }
}
