using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.Application.DTOs
{
    public class AdicionarJogoNaPromocaoDto
    {
        [Required(ErrorMessage = "O Id da promoção é obrigatório.")]
        public Guid PromocaoId { get; set; }

        [Required(ErrorMessage = "O Id do jogo é obrigatório.")]
        public Guid JogoId { get; set; }

        public bool EnviarNotificacao { get; set; } = true; 
    }
}
