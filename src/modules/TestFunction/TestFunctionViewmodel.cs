using shared.domain.entities;
namespace TestFunction;

public class TestFunctionViewmodel
{

    private User User { get; }

    public TestFunctionViewmodel(User user)
    {
        User = user;
    }

    public Dictionary<string, string> ToDict()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>()
        {
            { "name", User.getName() }
        };
        return dict;
    }
}