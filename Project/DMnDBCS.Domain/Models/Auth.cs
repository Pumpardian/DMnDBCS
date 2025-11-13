using System.ComponentModel.DataAnnotations;

namespace DMnDBCS.Domain.Models
{
    public record LoginRequest(string Email, string Name, string Password);
    public record AuthResponse(string Token, int UserId, string UserName, string Email);
}
