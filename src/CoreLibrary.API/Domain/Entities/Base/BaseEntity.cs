using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CoreLibrary.API.Domain.Entities.Base;

[ExcludeFromCodeCoverage]
public abstract class BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int Id { get; set; }
}

[ExcludeFromCodeCoverage]
public abstract class BaseEntityDate : BaseEntity
{
    public void UpdateDateModified() => DateModified = DateTime.Now;
    public DateTime? DateCreated { get; private set; } = DateTime.UtcNow;

    public DateTime? DateModified { get; private set; } = DateTime.UtcNow;
}

[ExcludeFromCodeCoverage]
public abstract class BaseEntityData : BaseEntityDate
{
    public string Data { get; set; } = null!;
    public string? Description { get; set; }
}
