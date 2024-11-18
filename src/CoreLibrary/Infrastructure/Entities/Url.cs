using CoreLibrary.API.Domain.Entities.Base;

namespace CoreLibrary.Infrastructure.Entities;

public partial class Url : BaseEntity
{

    public string Data { get; set; } = null!;

    public string? Payload { get; set; }

    public string Description { get; set; } = null!;

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual ICollection<Query> Queries { get; set; } = new List<Query>();

    public virtual ICollection<Response> Responses { get; set; } = new List<Response>();
}
