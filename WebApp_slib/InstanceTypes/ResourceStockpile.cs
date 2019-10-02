using System;
using System.Diagnostics;
using JetBrains.Annotations;
using WebApp_slib.StaticTypes;

namespace WebApp_slib.InstanceTypes {
public struct ResourceStockpile :
    IEquatable<ResourceStockpile>,
    IComparable<ResourceStockpile> 
{
    public GameResourceType type { get; }
    public int value { get; set; }

    public ResourceStockpile(
        GameResourceType type,
        int          value = 0
    ) {
        this.type  = type ?? throw new ArgumentNullException(paramName: nameof(type));
        this.value = value;
    }
    
    public bool canCombine(ResourceStockpile other) => canCombine(this, other);

    public static bool canCombine(
        ResourceStockpile lhs,
        ResourceStockpile rhs
    ) => lhs.type == rhs.type;

    [Pure]
    public static ResourceStockpile operator + (
        ResourceStockpile lhs,
        ResourceStockpile rhs
    ) => combineStockpile(lhs, rhs, _addInt);

    [Pure]
    public static ResourceStockpile operator + (
        ResourceStockpile lhs,
        int rhv
    ) => combineStockpile(lhs, rhv, _addInt);
    
    [Pure]
    public static ResourceStockpile operator - (
        ResourceStockpile lhs,
        ResourceStockpile rhs
    ) => combineStockpile(lhs, rhs, _subInt);

    [Pure]
    public static ResourceStockpile operator - (
        ResourceStockpile lhs,
        int               rhv
    ) => combineStockpile(lhs, rhv, _subInt);
    [Pure]
    
    public static ResourceStockpile operator * (
        ResourceStockpile lhs,
        int               rhv
    ) => combineStockpile(lhs, rhv, _multInt);


    [Pure]
    private static ResourceStockpile combineStockpile(
        ResourceStockpile lhs,
        ResourceStockpile rhs,
        Func<int,int,int> valueCalculator
    ) {
        if (!canCombine(lhs,rhs)) throw new Exception(
            $"Cannot combine stockpiles of conflicting types! ({lhs.type} + {rhs.type})"
        );
        return combineStockpile(
            lhs,
            rhs.value, 
            valueCalculator
        );
    }
    
    [Pure]
    private static ResourceStockpile combineStockpile(
        ResourceStockpile   lhs,
        int                 rhv,
        Func<int, int, int> valueCalculator
    ) {
        return new ResourceStockpile(
            type  : lhs.type, 
            value : valueCalculator.Invoke(lhs.value, rhv)
        );
    }

    public bool Equals(ResourceStockpile other) 
        => this.type == other.type && this.value == other.value;

    public int CompareTo(ResourceStockpile other) 
        => this.type.CompareTo(other.type);

    private static int _addInt (int lhv, int rhv) => lhv + rhv;
    private static int _subInt (int lhv, int rhv) => lhv - rhv;
    private static int _multInt(int lhv, int rhv) => lhv * rhv;

    public override string ToString() {
        return $"RS:{{{type}: {value}}}";
    }
}
}
