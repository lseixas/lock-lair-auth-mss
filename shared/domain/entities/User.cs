namespace shared.domain.entities;

public class User
{
    private String Name { get; set; }

    public User(string name)
    {
        this.Name = name;
    }
    
    public string getName()
    {
        return this.Name;
    }
}