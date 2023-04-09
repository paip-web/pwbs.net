using NUnit.Framework;
using PWBS.NetFrameworkUtils;

namespace PWBSTests.NetFrameworkUtils;

[TestFixture]
public class UnionType
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void BasicCommandTest()
    {
        UnionType<string> test = new("Hello");
        
        Assert.That(test.getValue(), Is.Not.Null);
        Assert.That(test.getValue(), Is.EqualTo("Hello"));
        
        UnionType<string, int> test2 = new("Hello");
        
        Assert.That(test2.getValue(), Is.Not.Null);
        Assert.That(test2.getValue(), Is.EqualTo("Hello"));
        
        test2 = new(1);
        
        Assert.That(test2.getValue(), Is.Not.Null);
        Assert.That(test2.getValue(), Is.Not.EqualTo("Hello"));
        Assert.That(test2.getValue(), Is.EqualTo(1));
    }
}