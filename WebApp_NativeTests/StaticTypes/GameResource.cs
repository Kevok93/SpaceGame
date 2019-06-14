using NUnit.Framework;
using NUnit.Framework.Internal;
using WebApp_slib.StaticTypes;

namespace WebApp_NativeTests.StaticTypes {
[TestFixture(TestName = "Game Resource Tests", TestOf = typeof(GameResource))]
public class GameResourceTest {
    
    [TestCase(arg1: "Cash", arg2: "Holla holla", arg3: "USD", TestName = "Cash")]
    [TestCase(arg1: "Cash", arg2: "Holla holla", arg3: "USD", TestName = "Cash")]
    public void init(
        string name,
        string desc,
        string unit
    ) {
        var t1 = new GameResource(
            name: name,
            description: desc,
            units: unit
        );

        Assert.Multiple( () =>{
            Assert.That(t1.name        == name);
            Assert.That(t1.description == desc);
            Assert.That(t1.units       == unit);
        });
    }
    
    
    
}
}
