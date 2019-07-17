using System;

namespace WebApp_slib.StaticTypes {
    public struct SingularYield : 
        IEquatable<SingularYield> ,
        IEquatable<SingularYield?> ,
        IEquatable<GameResourceType>
    {
        public GameResourceType type;
        public int              value;

        public SingularYield(
            GameResourceType type,
            int              value
        ) {
            this.type  = type ?? throw new ArgumentNullException(paramName: nameof(type));
            this.value = value;
        }

        public bool Equals(SingularYield    other) => other.type == this.type; 
        public bool Equals(SingularYield?   other) => other != null && other.Value.type == this.type;
        public bool Equals(GameResourceType other) => this.type == other;

        public override bool Equals(object o) {
            switch (o) {
                case null                 : return false;
                case GameResourceType type: return this.Equals(type);
                case SingularYield  yield : return this.Equals(yield);
                default                   : return false;
            }
        }
        public override int GetHashCode() => this.type != null ? this.type.GetHashCode() : 0;
        
        
        public static bool operator == (
            SingularYield lhs,
            SingularYield rhs
        ) => lhs.Equals(rhs);
        public static bool operator != (
            SingularYield lhs,
            SingularYield rhs
        ) => !lhs.Equals(rhs);
        public static SingularYield operator + (
            SingularYield lhs,
            SingularYield rhs
        ) => combine(lhs, rhs, op: (lhv,rhv) => lhv+rhv);
        public static SingularYield operator + (
            SingularYield lhs,
            int rhs
        ) => combine(lhs, rhs, op: (lhv, rhv) => lhv + rhv);
        public static SingularYield operator - (
            SingularYield lhs,
            SingularYield rhs
        ) => combine(lhs, rhs, op: (lhv, rhv) => lhv - rhv);
        public static SingularYield operator - (
            SingularYield lhs,
            int           rhs
        ) => combine(lhs, rhs, op: (lhv, rhv) => lhv - rhv);
        public static SingularYield operator * (
            SingularYield lhs,
            int           rhs
        ) => combine(lhs, rhs, op: (lhv, rhv) => lhv * rhv);

        private delegate int combineOperator(int lhv, int rhv);

        private static SingularYield combine(
            SingularYield   lhs,
            SingularYield   rhs,
            combineOperator op
        ) => (lhs.type == rhs.type) 
            ? new SingularYield(type: lhs.type, value: op.Invoke(lhs.value, rhs.value))
            : throw new InvalidCastException();

        private static SingularYield combine(
            SingularYield   lhs,
            int             rhs,
            combineOperator op
        ) => new SingularYield(type: lhs.type, value: op.Invoke(lhs.value, rhs));
    }
}