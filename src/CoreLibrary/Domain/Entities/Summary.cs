using CoreLibrary.API.Domain.Entities.Base;

namespace CoreLibrary.Domain.Entities;

public class Summary : BaseParentEntity
{
    public string Header { get; set; } = null!;
    public string? SubHeader { get; set; }
    public string? SummaryHeader { get; set; }
    public List<string>? Lines { get; set; }
    public List<string>? Codes { get; set; }
    public virtual ICollection<Summary> Summaries { get; set; } = new List<Summary>();
}
