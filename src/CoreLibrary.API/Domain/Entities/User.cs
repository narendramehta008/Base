using CoreLibrary.API.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CoreLibrary.API.Domain.Entities;

public class User : BaseEntityDate
{
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public string Email { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string ProfileUrl { get; set; }
    [JsonIgnore()]
    public byte[] PasswordHash { get; set; }
    [JsonIgnore]
    public byte[] PasswordSalt { get; set; }

    public int RoleId { get; set; }
    [JsonIgnore]
    public virtual Role Role { get; set; } = null!;
}