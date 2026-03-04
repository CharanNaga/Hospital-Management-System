namespace Hospital.AuthService.DTOs
{
    public class AuthDtos
    {
        public record LoginRequestDto(string Username, string Password);
        public record RegisterRequestDto(string Username, string Password, string? Role);
    }
}
