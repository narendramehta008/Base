using CoreLibrary.API.Domain.Entities.Base;

namespace CoreLibrary.Infrastructure.Entities;

public partial class Query : BaseEntity
{

    public int? UrlId { get; set; }

    public string Data { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual Url? Url { get; set; }
}
