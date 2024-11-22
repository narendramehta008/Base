using CoreLibrary.API.Domain.Entities.Base;

namespace CoreLibrary.Domain.Entities;

public partial class Query : BaseEntityData
{
    public int? UrlId { get; set; }
    public virtual Url? Url { get; set; }
}
