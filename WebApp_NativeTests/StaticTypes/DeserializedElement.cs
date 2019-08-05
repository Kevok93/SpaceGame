using NUnit.Framework;
using WebApp_slib.StaticTypes;

namespace WebApp_NativeTests.StaticTypes {
	public class ElementIdTest {
		[Test]
		public static void init() {
			ElementId t1 = 3;
			Assert.AreEqual(3, t1.Value);
		}

		[Test]
		public static void equality() {
			ElementId  t1 = 3;
			ElementId  t2 = 3;
			ElementId  t3 = 5;
			ElementId? t4 = 3;
			ElementId? tn = null;

			Assert.AreEqual   (t1, t2);
			Assert.AreEqual   (t1, t4);
			Assert.AreEqual   (t1,  3);
			Assert.AreEqual   (t4,  3);
			Assert.AreEqual   (tn, tn);
			Assert.AreNotEqual(t1, t3);
			Assert.AreNotEqual(t1, tn);
			Assert.AreNotEqual(t1,  5);
			Assert.AreNotEqual(t4, tn);
			Assert.AreNotEqual(t4, "asdf");
			
			Assert.That(t1.Equals((object)t2));
			Assert.That(t1.Equals((object)t4));
			Assert.That(t1.Equals((object) 3));
			Assert.That(t4.Equals((object) 3));
			Assert.That(!t1.Equals((object)t3));
			Assert.That(!t1.Equals((object)tn));
			Assert.That(!t1.Equals((object) 5));
			Assert.That(!t4.Equals((object)tn));

			Assert.That(t1 == t2);
			Assert.That(t1 == t4);
			Assert.That(t1 ==  3);
			Assert.That(t4 ==  3);
			Assert.That(tn == tn);
			Assert.That(t1 != t3);
			Assert.That(t1 != tn);
			Assert.That(t1 !=  6);
			Assert.That(t4 !=  6);
			Assert.That(t4 != tn);
			Assert.That(t2 == t1);
			Assert.That(t4 == t1);
			Assert.That( 3 == t1);
			Assert.That( 3 == t4);
			Assert.That(t3 != t1);
			Assert.That(tn != t1);
			Assert.That( 6 != t1);
			Assert.That( 6 != t4);
			Assert.That(tn != t4);
		}

		[Test]
		public static void comparison() {
			ElementId tElem = 50;
			ElementId? tN = null;
			Assert.Greater (tElem.CompareTo(tN), 0);
			Assert.Greater (tElem.CompareTo(25), 0);
			Assert.AreEqual(tElem.CompareTo(50), 0);
			Assert.Less    (tElem.CompareTo(75), 0);
		}
		
	}
	public class DeserializedElementTest : DeserializedElement {

		public DeserializedElementTest() : base(0,ElementType.AbstractImage) {}
		private DeserializedElementTest(
			int         id,
			ElementType type
		) : base(id, type) { }

		[Test]
		public static void init() {
			var t1 = new DeserializedElementTest(
				id: 1,
				type: ElementType.AbstractImage
			);

			Assert.AreEqual(1, t1.id);
			Assert.AreEqual(ElementType.AbstractImage, t1.type);
		}

		[Test]
		public static void equality() {
			
			DeserializedElement t1 = new DeserializedElementTest(
				id: 1,
				type: ElementType.AbstractImage
			);
			
			DeserializedElement t2 = new DeserializedElementTest(
				id: 1,
				type: ElementType.AbstractImage
			);
			
			DeserializedElement t3 = new DeserializedElementTest(
				id: 1,
				type: ElementType.ClusterType
			);
			
			DeserializedElement t4 = new DeserializedElementTest(
				id: 2,
				type: ElementType.AbstractImage
			);

			DeserializedElement tN = null;

			Assert.AreEqual   (t1, t2);
			Assert.AreNotEqual(t1, t3);
			Assert.AreNotEqual(t1, t4);
			Assert.AreNotEqual(t1, tN);
			
			Assert.That(t1 == t2);
			Assert.That(t1 != t3);
			Assert.That(t1 != t4);
			Assert.That(t1 != tN);
			
			Assert.That(t2 == t1);
			Assert.That(t3 != t1);
			Assert.That(t4 != t1);
			Assert.That(tN != t1);
		}
		
		[Test]
		public static void comparison() {
			DeserializedElement t1 = new DeserializedElementTest(
				id: 50,
				type: ElementType.GameResource
			);
			DeserializedElement t2 = new DeserializedElementTest(
				id: 50,
				type: ElementType.GameResource
			);
			DeserializedElement t3 = new DeserializedElementTest(
				id: 25,
				type: ElementType.GameResource
			);
			DeserializedElement t4 = new DeserializedElementTest(
				id: 75,
				type: ElementType.GameResource
			);
			DeserializedElement t5 = new DeserializedElementTest(
				id: 50,
				type: ElementType.ClusterType
			);
			DeserializedElement tN = null;
			
			Assert.Greater (t1.CompareTo(tN), 0);
			Assert.Greater (t1.CompareTo(t3), 0);
			Assert.AreEqual(t1.CompareTo(t2), 0);
			Assert.Less    (t1.CompareTo(t4), 0);
			Assert.Less    (t1.CompareTo(t5), 0);
		}
	}
}