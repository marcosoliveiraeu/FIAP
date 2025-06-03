namespace FiapCloudGames.Application.DTOs
{
    public class PromocaoJogoResponseDto
    {
        public Guid Id { get; set; }
        public Guid PromocaoId { get; set; }
        public Guid JogoId { get; set; }
        public string TituloPromocao { get; set; }
        public string TituloJogo { get; set; }
        public decimal ValorOriginal { get; set; }
        public decimal ValorComDesconto { get; set; }
        public DateTime DataInclusao { get; set; }
    }
}
