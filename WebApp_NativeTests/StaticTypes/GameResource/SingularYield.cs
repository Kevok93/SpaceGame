using System;
using NUnit.Framework;
using WebApp_slib.StaticTypes;
using static WebApp_NativeTests.StaticTypes.GameResource.GameResourceTest;

namespace WebApp_NativeTests.StaticTypes.GameResource {

	[TestFixture(TestName = "Game Singular Resource Yield Tests", TestOf = typeof(SingularYield))]
	public class SingularGameResourceYieldTest {

		public static SingularYield getTestSingularGameResourceYield(
			GameResourceType type  = null,
			int              value = 123
		) {
			type = type ?? getTestGameResourceType();
			return TestConstructor.testBuildObject(
				() => new SingularYield(type, value)
			);
		}

		[Test]
		public void init() {
			SingularYield?   yield = null;
			GameResourceType type  = getTestGameResourceType();
			Assert.DoesNotThrow(() => yield = getTestSingularGameResourceYield(
				type: type,
				value: 456
			));

			Assert.NotNull(yield);
			Assert.AreSame(type, yield.Value.type);
			Assert.AreEqual(456, yield.Value.value);
		}
		
		[Test]
		public void equality() {
			GameResourceType type1 = getTestGameResourceType(id: 1);
			GameResourceType type2 = getTestGameResourceType(id: 2);
			GameResourceType typeN = null;

			SingularYield yield1 = getTestSingularGameResourceYield(type1, 100);
			SingularYield yield2 = getTestSingularGameResourceYield(type1, 200);
			SingularYield yield3 = getTestSingularGameResourceYield(type2, 300);
			SingularYield yieldN = getTestSingularGameResourceYield(typeN, 400);

			Assert.AreEqual(yield1, yield1);
			Assert.AreEqual(yield1, yield2);
			Assert.AreNotEqual(yield1, yield3);
			Assert.AreNotEqual(yield1, yieldN);
			
			Assert.That(yield1 == yield1);
			Assert.That(yield1 == yield2);
			Assert.That(yield2 == yield1);
			Assert.That(yield1 != yield3);
			Assert.That(yield1 != yieldN);
			Assert.That(yield3 != yield1);
			Assert.That(yieldN != yield1);
			
			Assert.That( yield1.Equals((object)yield2));
			Assert.That( yield1.Equals((object) type1));
			Assert.That(!yield1.Equals((object)yield3));
			Assert.That(!yield1.Equals((object)  null));
			Assert.That(!yield1.Equals((SingularYield?) null));
		}
		
		[Test]
		public void math() {
			GameResourceType type1 = getTestGameResourceType(id: 1);
			GameResourceType type2 = getTestGameResourceType(id: 2);

			SingularYield yield1 = getTestSingularGameResourceYield(type: type1, value:  100);
			SingularYield yield2 = getTestSingularGameResourceYield(type: type1, value:  200);
			SingularYield yield3 = getTestSingularGameResourceYield(type: type2, value:  300);
			SingularYield yield4 = getTestSingularGameResourceYield(type: type1, value: -400);

			SingularYield    sum1 = yield1 + yield2;
			Assert.AreEqual( sum1.type     , type1);
			Assert.AreEqual( sum1.value    ,   300);
			
			SingularYield    sum2 = yield1 +    500;
			Assert.AreEqual( sum2.type     , type1);
			Assert.AreEqual( sum2.value    ,   600);
			
			SingularYield    sum3 =    500 + yield1;
			Assert.AreEqual( sum3.type     , type1);
			Assert.AreEqual( sum3.value    ,   600);
				
			SingularYield    sum4 = yield1 + yield4;
			Assert.AreEqual( sum4.type     , type1);
			Assert.AreEqual( sum4.value    ,  -300);
				
			SingularYield   diff1 = yield2 - yield1;
			Assert.AreEqual(diff1.type     , type1);
			Assert.AreEqual(diff1.value    ,   100);
			
			SingularYield   diff2 = yield1 - yield4;
			Assert.AreEqual(diff2.type     , type1);
			Assert.AreEqual(diff2.value    ,   500);
			
			SingularYield   diff3 = yield4 -    100;
			Assert.AreEqual(diff3.type     , type1);
			Assert.AreEqual(diff3.value    ,  -500);
			
			SingularYield   diff4 =    500 - yield1;
			Assert.AreEqual(diff4.type     , type1);
			Assert.AreEqual(diff4.value    ,   400);
			
			SingularYield   prod1 = yield1 *      2;
			Assert.AreEqual(prod1.type     , type1);
			Assert.AreEqual(prod1.value    ,   200);
			
			SingularYield   prod2 = yield1 *     -1;
			Assert.AreEqual(prod2.type     , type1);
			Assert.AreEqual(prod2.value    ,  -100);
			
			Assert.Throws<InvalidCastException>(() => yield1 += yield3);
			Assert.Throws<InvalidCastException>(() => yield1 += yield3);
			Assert.Throws<InvalidCastException>(() => yield1 -= yield3);
			Assert.AreEqual(yield1.value, 100);
		}
	}
}