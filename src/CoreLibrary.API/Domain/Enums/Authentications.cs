namespace CoreLibrary.API.Domain.Enums;

public enum Authentications
{
    Bearer,
    Basic,
    Token
}

public enum HealthCheck
{
    None,
    Once,
    Retry
}