using System;
using NUnit.Framework;
using WebApp_slib.StaticTypes;
using static WebApp_NativeTests.StaticTypes.GameResource.GameResourceTest;

namespace WebApp_NativeTests.StaticTypes.GameResource {

    [TestFixture(TestName = "Game Singular Resource Yield Tests", TestOf = typeof(SingularYield))]
    public class SingularGameResourceYieldTest {

        public static SingularYield GetTestSingularGameResourceYield(
            GameResourceType type  = null,
            int              value = 123
        ) {
            type = type ?? GetTestGameResourceType();
            try {return new SingularYield(type, value);}
            catch (Exception e) {
                throw new InconclusiveException(
                    message: "Could not initialize a Single-Resource Yield",
                    inner: e
                );
            }
        }

        [Test]
        public void init() {
            SingularYield?   yield = null;
            GameResourceType type  = GetTestGameResourceType();
            Assert.DoesNotThrow(() => yield = GetTestSingularGameResourceYield(
                type: type,
                value: 456
            ));

            Assert.NotNull(yield);
            Assert.AreSame(type, yield.Value.type);
            Assert.AreEqual(456, yield.Value.value);
        }
        
        [Test]
        public void equality() {
            GameResourceType type1 = GetTestGameResourceType(id: 1);
            GameResourceType type2 = GetTestGameResourceType(id: 2);
            GameResourceType typeN = null;

            SingularYield yield1 = GetTestSingularGameResourceYield(type1, 100);
            SingularYield yield2 = GetTestSingularGameResourceYield(type1, 200);
            SingularYield yield3 = GetTestSingularGameResourceYield(type2, 300);
            SingularYield yieldN = GetTestSingularGameResourceYield(typeN, 400);

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
        }
    }
}