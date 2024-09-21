using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CoreLibrary.API.Dtos;

[ExcludeFromCodeCoverage]
public class UserForRegisterDto
{
    [Required]
    public required string Username { get; set; }

    [Required]
    [StringLength(8, MinimumLength = 4, ErrorMessage = "Password must be between 4 and 8")]
    public required string Password { get; set; }
}
