using CoreLibrary.API.Domain.Entities.Base;
using System;
using System.Collections.Generic;

namespace CoreLibrary.Infrastructure.Entities;

public partial class Response : BaseEntity
{

    public int? UrlId { get; set; }

    public string Data { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Type { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual Url? Url { get; set; }
}
