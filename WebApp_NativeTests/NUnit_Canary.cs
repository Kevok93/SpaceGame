using NUnit.Framework;

namespace WebApp_NativeTests {
    public class NUnit_Canary {
        [Test]
        public void LinkTest() {
            var canaryCage = new WebApp_NativeLib.LinkCanaryCage();
            Assert.True(canaryCage.canary.isAlive);
        }
    }
}
