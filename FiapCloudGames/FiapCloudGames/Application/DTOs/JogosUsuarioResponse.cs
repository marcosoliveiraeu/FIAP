namespace FiapCloudGames.Application.DTOs
{
    public class JogosUsuarioResponse
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid JogoId { get; set; }
        public DateTime DataAquisicao { get; set; }
        public decimal PrecoPago { get; set; }
    }

}
