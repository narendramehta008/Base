namespace CoreLibrary.API.Domain.Services.TimeGuid.Enums;

[PublicAPI]
public enum GuidVersion
{
    TimeBased = 1,
    Dce,
    NameBased,
    Random
}
