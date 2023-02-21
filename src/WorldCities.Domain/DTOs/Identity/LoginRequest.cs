using System.ComponentModel.DataAnnotations;

namespace WorldCities.Domain.DTOs.Identity;

public class LoginRequest
{
    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = null!;
}