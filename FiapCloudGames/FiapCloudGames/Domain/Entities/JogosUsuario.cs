using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.Domain.Entities
{
    public class JogosUsuario
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O usuário é obrigatório.")]
        public Guid UsuarioId { get; set; }

        [Required(ErrorMessage = "O jogo é obrigatório.")]
        public Guid JogoId { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataAquisicao { get; set; } = DateTime.UtcNow;

        [Range(0.01, 10000, ErrorMessage = "O preço pago deve ser entre R$ 0,01 e R$ 10.000,00.")]
        public decimal PrecoPago { get; set; }


        public Jogo Jogo { get; set; }
        public Usuario Usuario { get; set; }

    }



}
