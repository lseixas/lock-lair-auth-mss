using shared.domain.entities;
namespace GetUser;

public class GetUserViewmodel
{

    private User User { get; }

    public GetUserViewmodel(User user)
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