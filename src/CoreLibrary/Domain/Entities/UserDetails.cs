using CoreLibrary.API.Domain.Entities;

namespace CoreLibrary.Domain.Entities;

public class UserDetails
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}
