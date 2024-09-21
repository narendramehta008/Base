using JetBrains.Annotations;

namespace CoreLibrary.API.Domain.Services.TimeGuid;

[MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
internal sealed class PublicAPIAttribute : Attribute
{
    [CanBeNull]
    public string Comment { get; }

    public PublicAPIAttribute()
    {
    }

    public PublicAPIAttribute([NotNull] string comment)
    {
        Comment = comment;
    }
}