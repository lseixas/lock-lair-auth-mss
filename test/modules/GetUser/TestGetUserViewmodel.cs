using GetUser;
using shared.domain.entities;

namespace test.modules.GetUser;

public class TestGetUserViewmodel
{
    [Fact]
    public void TestToDict()
    {
        var user = new User("John Doe");
        var viewModel = new GetUserViewmodel(user);
        
        var dict = viewModel.ToDict();

        var expected = new Dictionary<String, String>()
        {
            { "name", user.getName() }
        };
        
        Assert.Equal("John Doe", dict["name"]);
        Assert.Equal(expected, dict);
    }
}