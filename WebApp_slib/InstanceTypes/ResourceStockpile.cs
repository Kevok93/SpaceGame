using System;
using System.Diagnostics;
using WebApp_slib.StaticTypes;

namespace WebApp_slib.InstanceTypes {
public class ResourceStockpile {
    public GameResourceType type { get; }
    public int value { get; set; }

    public ResourceStockpile(
        GameResourceType type,
        int          value = 0
    ) {
        this.type = type;
        this.value = value;
    }
    
    public bool canCombine(ResourceStockpile other) => canCombine(this, other);
    
    public ResourceStockpile set(int value) { this.value = value; return this; }
    public ResourceStockpile set(ResourceStockpile other)
        => modifyStockpile(this, other, (lhv, rhv) => rhv);

    public ResourceStockpile add(int value) { this.value += value; return this; }
    public ResourceStockpile add(ResourceStockpile other)
        => modifyStockpile(this, other, (lhv, rhv) => lhv + rhv);
    
    public ResourceStockpile sub(int value) { this.value -= value; return this; }
    public ResourceStockpile sub(ResourceStockpile other)
        => modifyStockpile(this, other, (lhv, rhv) => lhv - rhv);
    
    
    
    
    
    
    
    
    public static bool canCombine(
        ResourceStockpile lhs,
        ResourceStockpile rhs
    ) {
        Debug.Assert(lhs != null, nameof(lhs) + " != null");
        Debug.Assert(rhs != null, nameof(rhs) + " != null");
        return (lhs.type == rhs.type);
    }

    public static ResourceStockpile operator + (
        ResourceStockpile lhs,
        ResourceStockpile rhs
    ) => combineStockpile(lhs, rhs, (lhv, rhv) => lhv + rhv);


    public static ResourceStockpile operator - (
        ResourceStockpile lhs,
        ResourceStockpile rhs
    ) => combineStockpile(lhs, rhs, (lhv, rhv) => lhv - rhv);

    private static ResourceStockpile combineStockpile(
        ResourceStockpile lhs,
        ResourceStockpile rhs,
        Func<int,int,int> valueCalculator
    ) {
        Debug.Assert(lhs != null, nameof(lhs) + " != null");
        Debug.Assert(rhs != null, nameof(rhs) + " != null");
        Debug.Assert(
            canCombine(lhs,rhs), 
            $"Cannot combine stockpiles of conflicting types! ({lhs.type} + {rhs.type})"
        );
        if (!canCombine(lhs,rhs)) throw new Exception(
            $"Cannot combine stockpiles of conflicting types! ({lhs.type} + {rhs.type})"
        );
        return new ResourceStockpile(
            type  : lhs.type, 
            value : valueCalculator.Invoke(lhs.value, rhs.value)
        );
    }
    
    private static ResourceStockpile modifyStockpile(
        ResourceStockpile lhs,
        ResourceStockpile rhs,
        Func<int,int,int> valueCalculator
    ) {
        Debug.Assert(lhs != null, nameof(lhs) + " != null");
        Debug.Assert(rhs != null, nameof(rhs) + " != null");
        Debug.Assert(
            canCombine(lhs, rhs), 
            $"Cannot combine stockpiles of conflicting types! ({lhs.type} + {rhs.type})"
        );
        if (!canCombine(lhs, rhs)) throw new Exception(
            $"Cannot combine stockpiles of conflicting types! ({lhs.type} + {rhs.type})"
        );
        lhs.value = valueCalculator.Invoke(lhs.value, rhs.value);
        return lhs;
    }

}
}
