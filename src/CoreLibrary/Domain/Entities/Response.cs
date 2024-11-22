using CoreLibrary.API.Domain.Entities.Base;
using System;
using System.Collections.Generic;

namespace CoreLibrary.Domain.Entities;

public partial class Response : BaseEntityData
{
    public int? UrlId { get; set; }

    public string? Type { get; set; }

    public virtual Url? Url { get; set; }
}