using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;

namespace WebApp_slib.StaticTypes {
public class GameResourceType : 
    DeserializedElement,
    IEquatable<GameResourceType>,
    IComparable<GameResourceType> 
{
    public string name { get; }
    public string description { get; }
    public string units { get; }

    public GameResourceType (
        ElementId id,
        string name,
        string description,
        string units
    ) : base(id, ElementType.GameResource) {
        this.name        = name        ?? throw new ArgumentNullException(nameof(       name));
        this.description = description ?? throw new ArgumentNullException(nameof(      units));
        this.units       = units       ?? throw new ArgumentNullException(nameof(description));
    }

    public bool Equals(GameResourceType other) => other != null && this.id == other.id;
    
    public static bool operator == (
        GameResourceType lhs,
        GameResourceType rhs
    ) => Object.Equals(lhs, rhs);
    public static bool operator != (
        GameResourceType lhs,
        GameResourceType rhs
    ) => !Object.Equals(lhs, rhs);

    public int CompareTo(GameResourceType other) =>
        (other == null) ? 1 : String.Compare(
            name, 
            other.name, 
            comparisonType: StringComparison.Ordinal
        );
    
}
}
