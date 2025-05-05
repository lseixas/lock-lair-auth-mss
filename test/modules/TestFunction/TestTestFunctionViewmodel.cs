using TestFunction;
using shared.domain.entities;

namespace test.modules.TestFunction;

public class TestTestFunctionViewmodel
{
    [Fact]
    public void TestToDict()
    {
        var user = new User("John Doe");
        var viewModel = new TestFunctionViewmodel(user);
        
        var dict = viewModel.ToDict();

        var expected = new Dictionary<String, String>()
        {
            { "name", user.getName() }
        };
        
        Assert.Equal("John Doe", dict["name"]);
        Assert.Equal(expected, dict);
    }
}