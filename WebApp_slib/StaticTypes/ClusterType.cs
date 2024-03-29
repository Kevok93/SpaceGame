using static WebApp_slib.StaticTypes.GameResourceType;
using static WebApp_slib.StaticTypes.MutableResourceYieldInternal;

namespace WebApp_slib.StaticTypes {
    public class ClusterType : DeserializedElement {
        public AbstractImage icon        { get; }
        public string        name        { get; }
        public string        description { get; }
        
        public delegate ResourceYield YieldModifier(MutableResourceYield baseYield);

        public ResourceYield modifyYield(ResourceYield baseYield) => 
            this.yieldModifier.Invoke(baseYield.cloneUnlocked());
        
        private readonly YieldModifier yieldModifier;

        public ClusterType(
            ElementId       id,
            AbstractImage   icon,
            string          name,
            string          description,
            YieldModifier   yieldModifier
        ) : base(id, ElementType.ClusterType) {
            this.icon          = icon;
            this.name          = name;
            this.description   = description;
            this.yieldModifier = yieldModifier;
        }
        
        public static readonly YieldModifier NOOP_MODIFIER = baseYield => baseYield.cloneLocked();
    }
}