namespace FiapCloudGames.Application.DTOs
{
   
    public class LoginRequestDto
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }

    public class LoginResponseDto
    {
        public string Token { get; set; }
    }

   
}
