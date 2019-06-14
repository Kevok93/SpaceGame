namespace WebApp_slib.StaticTypes {
public class GameResource {
    public string name { get; }
    public string description { get; }
    public string units { get; }

    public GameResource(
        string name,
        string description,
        string units
    ) {
        this.name = name;
        this.description = description;
        this.units = units;
    }
}
}
