using System.ComponentModel.DataAnnotations;

namespace DMnDBCS.Domain.Models
{
    public record LoginRequest(string Name, string Email, string Password);
    public record AuthResponse(string Token, int UserId, string UserName, string Email);
}
