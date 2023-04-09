using PWBS;
using PWBS.ConfigFile;

namespace PWBSTests.ConfigFile;

[TestFixture]
public class PWBSConfigurationFileUnitTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void BasicCommandTest()
    {
        var test = PWBSConfigurationFile.createPWBSConfigurationFileBasedOnJson(@"
            {
                ""commands"": {
                    ""hello"": ""echo hello""
                }
            }");
        Assert.That(test, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(test.commands, Is.Not.Null);
            Assert.That(test.commands, Has.Count.EqualTo(1));
            Assert.That(test.commands, Contains.Key("hello"));
            Assert.That(test.commands["hello"], Is.Not.Null);
            Assert.That(test.commands["hello"].CommandStrings, Has.Count.EqualTo(1));
            Assert.That(test.commands["hello"].CommandStrings[0], Is.Not.Empty);
            Assert.That(test.commands["hello"].CommandStrings[0], Is.EqualTo("echo hello"));
        });
    }

    [Test]
    public void MultiCommandTest()
    {
        var test = PWBSConfigurationFile.createPWBSConfigurationFileBasedOnJson(@"
            {
                ""commands"": {
                    ""hello"": [
                        ""echo hello"",
                        ""echo world"",
                    ]
                }
            }");
        Assert.That(test, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(test.commands, Is.Not.Null);
            Assert.That(test.commands, Has.Count.EqualTo(1));
            Assert.That(test.commands, Contains.Key("hello"));
            Assert.That(test.commands["hello"], Is.Not.Null);
            Assert.That(test.commands["hello"].CommandStrings, Has.Count.EqualTo(2));
            Assert.That(test.commands["hello"].CommandStrings[0], Is.Not.Empty);
            Assert.That(test.commands["hello"].CommandStrings[0], Is.EqualTo("echo hello"));
            Assert.That(test.commands["hello"].CommandStrings[1], Is.Not.Empty);
            Assert.That(test.commands["hello"].CommandStrings[1], Is.EqualTo("echo world"));
        });
    }

    [Test]
    public void SerializationBasicCommandTest()
    {
        var json = @"
            {
                ""commands"":{
                    ""hello"":""echo hello""
                }
            }";
        // This is a hack to get the json to be formatted correctly
        var tjson = json
            .Split("\n") // Split into lines
            .Select(line => line.Trim())
            // .Select(line => line.Split("            ")) // Split into parts if have sequence
            // .Select(line => line.Skip(1)) // Skip first split
            // .Select(line => string.Join("", line)) // Join parts of the line
            .ToList();
        json = string.Join("", tjson);
        var test = PWBSConfigurationFile.createPWBSConfigurationFileBasedOnJson(json);
        Assert.That(test, Is.Not.Null);
        if (test is null) return;
        var test2 = PWBSConfigurationFile.createJsonBasedOnPWBSConfigurationFile(test);
        Assert.That(test2, Is.EqualTo(json));
    }

    [Test]
    public void SerializationMultiCommandTest()
    {
        var json = @"
            {
                ""commands"":{
                    ""hello"":[
                        ""echo hello"",
                        ""echo world""
                    ]
                }
            }";
        // This is a hack to get the json to be formatted correctly
        var tjson = json
            .Split("\n") // Split into lines
            .Select(line => line.Trim())
            .ToList();
        json = string.Join("", tjson);
        var test = PWBSConfigurationFile.createPWBSConfigurationFileBasedOnJson(json);
        Assert.That(test, Is.Not.Null);
        if (test is null) return;
        var test2 = PWBSConfigurationFile.createJsonBasedOnPWBSConfigurationFile(test);
        Assert.That(test2, Is.EqualTo(json));
    }
}