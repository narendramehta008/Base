using CoreLibrary.API.Domain.Entities.Base;

namespace CoreLibrary.Domain.Entities;

public partial class Url : BaseEntityData
{
    public string? Payload { get; set; }

    public virtual ICollection<Query> Queries { get; set; } = new List<Query>();

    public virtual ICollection<Response> Responses { get; set; } = new List<Response>();
}
