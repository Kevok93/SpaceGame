using System;
using System.Diagnostics.Contracts;
using Newtonsoft.Json.Linq;

namespace WebApp_slib.StaticTypes {
    
    public class DeserializedElement{
        public readonly ElementId id;

        protected DeserializedElement(int       id) { this.id = id; }
        protected DeserializedElement(ElementId id) { this.id = id; }
    }
    
    public struct ElementId : 
        IComparable<ElementId>, 
        IEquatable<int>, 
        IEquatable<ElementId>  
    {
        public int Value { get; }
        private ElementId(int value) { this.Value = value; }
        
        public static implicit operator ElementId(int value) => new ElementId(value);
        public static implicit operator int(ElementId id)    => id.Value;
        
        //public static implicit operator JToken(ElementId         id)    => id.Value;
        public          int  CompareTo(ElementId other) => this.Value.CompareTo(other.Value);
        public          bool Equals(ElementId    other) => Value == other.Value;
        public          bool Equals(int          other) => Value == other;
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

        public static bool operator !=(
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


        
        public override int GetHashCode() => Value.GetHashCode();
    }
    
}