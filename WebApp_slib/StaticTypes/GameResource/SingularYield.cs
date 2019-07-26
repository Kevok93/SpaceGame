using System;
using JetBrains.Annotations;

namespace WebApp_slib.StaticTypes {
	public struct SingularYield : 
		IEquatable<SingularYield   > ,
		IEquatable<SingularYield?  > ,
		IEquatable<GameResourceType>
	{
		[NotNull]
		public GameResourceType type  { get; }
		public int              value { get; }

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
				case null : default       : return false;
				case GameResourceType type: return this.Equals(type);
				case SingularYield  yield : return this.Equals(yield);
			}
		}
		public override int GetHashCode() => this.type.GetHashCode();
		
		
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
		
		public static SingularYield operator + (
			int           rhs,
			SingularYield lhs
		) => combine(lhs, rhs, op: (lhv, rhv) => lhv + rhv);
		public static SingularYield operator - (
			SingularYield lhs,
			SingularYield rhs
		) => combine(lhs, rhs, op: (lhv, rhv) => lhv - rhv);
		public static SingularYield operator - (
			SingularYield lhs,
			int           rhs
		) => combine(lhs, rhs, op: (lhv, rhv) => lhv - rhv);
		public static SingularYield operator - (
			int           rhs,
			SingularYield lhs
		) => combine(lhs, rhs, op: (lhv, rhv) => rhv - lhv);
		public static SingularYield operator * (
			SingularYield lhs,
			int           rhs
		) => combine(lhs, rhs, op: (lhv, rhv) => lhv * rhv);

		private delegate int combineOperator(int lhv, int rhv);

		private static SingularYield combine(
			SingularYield   lhs,
			SingularYield   rhs,
			combineOperator op
		) { 
			if (lhs.type != rhs.type) throw new InvalidCastException();
			return new SingularYield(
				type : lhs.type,
				value: op.Invoke(lhs.value, rhs.value)
			);
		}

		private static SingularYield combine(
			SingularYield   lhs,
			int             rhs,
			combineOperator op
		) => new SingularYield(
			type: lhs.type, 
			value: op.Invoke(lhs.value, rhs)
		);
	}
}