using FiapCloudGames.Domain.Enuns;
using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.Domain.Entities
{
    public class Promocao
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres.")]
        [MinLength(3, ErrorMessage = "O título deve ter no mínimo 3 caracteres.")]
        public string Titulo { get; set; }

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O status é obrigatório.")]
        public StatusPromocao Status { get; set; }

        [Required(ErrorMessage = "O percentua de desconto é obrigatório.")]
        [Range(0.01, 100, ErrorMessage = "O percentual de desconto deve ser entre 0,01 e 100")]
        public decimal PercentualDesconto { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataInclusao { get; set; }

        [Required(ErrorMessage = "A data de validade é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime DataValidade { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataAtualizacao { get; set; }

        public List<PromocaoJogo> Jogos { get; set; } = new();
    }
}
