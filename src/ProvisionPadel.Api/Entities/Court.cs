namespace ProvisionPadel.Api.Entities;

public class Court : Entity
{
    public string Description { get; private set; }


    public static Court Create(string description)
    {
        return new Court
        {
            Description = description
        };
    }

    public void Update(string description)
    {
        Description = description;
    }
}
