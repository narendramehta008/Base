using CoreLibrary.API.Domain.Entities.Base;

namespace CoreLibrary.API.Domain.Entities;

public partial class Role : BaseEntity
{

    public string Code { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime? DateCreated { get; set; } = DateTime.UtcNow;

    public DateTime? DateModified { get; set; } = DateTime.UtcNow;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
