using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.Domain.Entities
{
    public class PromocaoJogo
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PromocaoId { get; set; }

        [Required]
        public Guid JogoId { get; set; }

        [Required(ErrorMessage = "O valor original é obrigatório.")]
        [Range(0.01, 10000, ErrorMessage = "O valor original deve ser entre R$ 0,01 e R$ 10.000,00.")]
        public decimal ValorOriginal { get; set; }

        [Required(ErrorMessage = "O valor com desconto é obrigatório.")]
        [Range(0.01, 10000, ErrorMessage = "O valor com desconto deve ser entre R$ 0,01 e R$ 10.000,00.")]
        public decimal ValorComDesconto { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataInclusao { get; set; }

        public Promocao Promocao { get; set; }
        public Jogo Jogo { get; set; }
    }
}
