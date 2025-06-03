using FiapCloudGames.Domain.Enuns;

namespace FiapCloudGames.Application.DTOs
{
    public class JogoResponseDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public CategoriaJogo Categoria { get; set; }
        public decimal Preco { get; set; }
        public DateTime DataLancamento { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public bool EstaEmPromocao { get; set; }
        public decimal PercentualDesconto { get; set; } = 0;
        public decimal ValorComDesconto { get; set; } = 0;
    }

}
