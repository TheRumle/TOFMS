using FluentAssertions;
using Tofms.Common.Move;

namespace Common.UnitTests.Move;

public class CountCollectionTest
{
    private readonly CountCollection<string> _sut = new();

    [Fact]
    public void WhenAddingNewKey_GivesValueOne()
    {
        var count = _sut.AddKey("key");
        count.Should().Be(1);
    }

    [Fact]
    public void WhenAddingExistingKey_IncrementsKey()
    {
        _sut.AddKey("key");
        var count = _sut.AddKey("key");
        count.Should().Be(2);
    }

    [Fact]
    public void WhenGettingNonExisting_GetsDefault()
    {
        _sut.GetCount("nonexisting").Should().Be(0);
    }
}