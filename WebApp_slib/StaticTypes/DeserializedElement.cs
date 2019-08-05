using System;
using System.Diagnostics.Contracts;
using Newtonsoft.Json.Linq;

namespace WebApp_slib.StaticTypes {

    
    
    public abstract class DeserializedElement : 
        IEquatable<DeserializedElement>,
        IComparable<DeserializedElement> 
    {
        //Values are used as sort order, if you're into that kinda thing.
        protected enum ElementType {
            GameResource  = 1,
            PlanetType    = 2,
            ClusterType   = 3,
            AbstractImage = 4
        }
        
        //Type field used to classify elements without using reflection
        //Elements that are not of the same ElementType are equatable but never equal
        //Not to be used as type-safety
        protected readonly ElementType type;
        public    readonly ElementId   id;


        protected DeserializedElement(
            ElementId id,
            ElementType type    
        ) {
            this.id = id;
            this.type = type;
        }

        public override bool Equals(object obj) {
            switch (obj) {
                case null: default: return false;
                case DeserializedElement e: return this.Equals(e);
            }
        }

        public override int GetHashCode() =>
            this.type.GetHashCode() &
            this.id.GetHashCode();

        public bool Equals(DeserializedElement other) =>
            other      != null      &&
            other.type == this.type &&
            other.id   == this.id;

        public int CompareTo(DeserializedElement other) {
            if (other == null) return 1;
            if (this.type != other.type) 
                return this.type.CompareTo(other.type);
            return this.id.CompareTo(other.id);
        }

        public static bool operator ==(
            DeserializedElement lhs,
            DeserializedElement rhs
        ) => Equals(lhs, rhs);
        
        public static bool operator != (
            DeserializedElement lhs,
            DeserializedElement rhs
        ) => !Equals(lhs, rhs);
    }
    
    public struct ElementId : 
        IComparable<ElementId>, 
        IComparable<ElementId?>, 
        IEquatable<int>, 
        IEquatable<ElementId>,
        IEquatable<ElementId?>  
    {
        public int Value { get; }
        private ElementId(int value) { this.Value = value; }

        public static implicit operator ElementId(int value) => new ElementId(value);
        //public static implicit operator int(ElementId id)    => id.Value;
        
        //public static implicit operator JToken(ElementId         id)    => id.Value;
        public          int  CompareTo(ElementId  other) 
            => this.Value.CompareTo(other.Value);
        public          int  CompareTo(ElementId? other) 
            => other != null 
                ? this.Value.CompareTo(other.Value) 
                : 1;
        public          bool Equals(ElementId    other) 
            => Value == other.Value;
        public          bool Equals(ElementId?   other) 
            => other != null 
            && Value == other.Value;
        public          bool Equals(int          other) 
            => Value == other;
        public override bool Equals(object obj) {
            switch (obj) {
                case null         : return false;
                case ElementId id : return this.Equals(id );
                case int       val: return this.Equals(val);
                default           : return base.Equals(obj);
            }
        }

        public static bool operator == (
            ElementId lhs,
            ElementId rhs
        ) => lhs.Equals(rhs);
        public static bool operator == (
            ElementId lhs,
            int rhs
        ) => lhs.Equals(rhs);
        public static bool operator == (
            int lhs,
            ElementId rhs
        ) => rhs.Equals(lhs);
        public static bool operator == (
            ElementId  lhs,
            ElementId? rhs
        ) => lhs.Equals(rhs);
        public static bool operator == (
            ElementId? lhs,
            ElementId  rhs
        ) => rhs.Equals(lhs);
        public static bool operator == (
            ElementId? lhs,
            int        rhs
        ) => lhs.Equals(rhs);
        public static bool operator == (
            int        lhs,
            ElementId? rhs
        ) => rhs.Equals(lhs);

        public static bool operator != (
            ElementId lhs,
            ElementId rhs
        ) => !lhs.Equals(rhs);
        public static bool operator != (
            ElementId lhs,
            int rhs
        ) => !lhs.Equals(rhs);
        public static bool operator != (
            int lhs,
            ElementId rhs
        ) => !rhs.Equals(lhs);
        public static bool operator != (
            ElementId  lhs,
            ElementId? rhs
        ) => !lhs.Equals(rhs);
        public static bool operator != (
            ElementId? lhs,
            ElementId  rhs
        ) => !rhs.Equals(lhs);
        public static bool operator != (
            ElementId? lhs,
            int        rhs
        ) => !lhs.Equals(rhs);
        public static bool operator != (
            int        lhs,
            ElementId? rhs
        ) => !rhs.Equals(lhs);

        //These are unintuitive
        //If lhs is null, the ?. returns null.
        //If rhs is also null, then the two are equal (both null)
        //If rhs is not null, the two are not equal (obj != null)
        //If lhs is not null, ReferenceEquals will not be evaluated
        public static bool operator == (
            ElementId? lhs,
            ElementId? rhs
        ) => lhs?.Equals(rhs) ?? ReferenceEquals(rhs, null);
        public static bool operator != (
            ElementId? lhs,
            ElementId? rhs
        ) => !(lhs?.Equals(rhs) ?? ReferenceEquals(rhs, null));
        
        public override int GetHashCode() => Value.GetHashCode();
    }
    
    
}