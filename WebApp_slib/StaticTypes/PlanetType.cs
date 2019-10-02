using static WebApp_slib.StaticTypes.GameResourceType;

namespace WebApp_slib.StaticTypes {
public class PlanetType : DeserializedElement {
    public AbstractImage icon        { get; }
    public string        name        { get; }
    public string        description { get; }
    
    public delegate ResourceYield YieldCalculator(uint planetCount);

    public ResourceYield getYield(uint planetCount) => this.yieldCalculator.Invoke(planetCount);
    private readonly YieldCalculator yieldCalculator;

    public PlanetType(
        ElementId       id,
        AbstractImage   icon,
        string          name,
        string          description,
        YieldCalculator yieldCalculator
    ) : base(id, ElementType.PlanetType) {
        this.icon            = icon;
        this.name            = name;
        this.description     = description;
        this.yieldCalculator = yieldCalculator;
    }
    
    public static readonly YieldCalculator NULL_YIELD = (planetCount) => MutableResourceYield.NOTHING_CONST;
}
}
