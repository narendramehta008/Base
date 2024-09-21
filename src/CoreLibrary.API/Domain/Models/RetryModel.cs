using CoreLibrary.API.Domain.Constants;
using System.Diagnostics.CodeAnalysis;

namespace CoreLibrary.API.Domain.Models;

[ExcludeFromCodeCoverage]
public class RetryModel
{
    public Cycle Cycle { get; set; } = new();
    public Api Api { get; set; } = new();
}
[ExcludeFromCodeCoverage]
public class Cycle
{
    public int InitialDelay { get; set; } = GlobalConstants.CYCLEINITIALDELAY;
    public int Increment { get; set; } = GlobalConstants.CYCLERETRYINCREMENT;//i.e. Attempts in Api they must be same
    public int TotalCycles { get; set; } = GlobalConstants.RETRYCYCLES;
    public int Threshold => Increment * TotalCycles;
    public bool FastFirst { get; set; } = true;// Recommended to true since for first time we don't need delay,
    public double Factor { get; set; } = 1.0;
}
[ExcludeFromCodeCoverage]
public class Api
{
    public int InitialDelay { get; set; } = GlobalConstants.INITIALDELAY;
    public int Attempts { get; set; } = GlobalConstants.CYCLERETRYINCREMENT;
    public double Factor { get; set; } = 2.0;//Exponential factor
    public bool FastFirst { get; set; } = false;
}
