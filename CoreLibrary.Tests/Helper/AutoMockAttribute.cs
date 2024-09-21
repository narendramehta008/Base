using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace CoreLibrary.Tests.Helper;

public class AutoMockDataAttribute : AutoDataAttribute
{
    public AutoMockDataAttribute() : base(GetDefaultFixture)
    {

    }
    public static IFixture GetDefaultFixture()
    {
        var autoMoqCustomization = new AutoMoqCustomization();
        var fixture = new Fixture().Customize(autoMoqCustomization);
        fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        return fixture;
    }
}
