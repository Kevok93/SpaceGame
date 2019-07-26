using System;
using System.Collections.ObjectModel;
using NUnit.Framework;
using WebApp_slib.StaticTypes;
using static WebApp_NativeTests.StaticTypes.GameResource.SingularGameResourceYieldTest;
using static WebApp_slib.StaticTypes.GameResourceType;
using static WebApp_slib.StaticTypes.ResourceYield;

namespace WebApp_NativeTests.StaticTypes.GameResource {
[TestFixture(TestName = "Game Resource Tests", TestOf = typeof(GameResourceType))]
public class GameResourceTest {

    public static GameResourceType getTestGameResourceType(
        int    id = 0,
        string name = "TestResourceType",
        string desc = "TestDescription",
        string unit = "TestUnit"
    ) {
        return TestConstructor.testBuildObject(
            () => new GameResourceType(
                id          : id,
                name        : name,
                description : desc,
                units       : unit
            )
        );
    }
    
    [Test]
    [TestCase(0, "Cash", "Holla holla", "USD", TestName = "Cash")]
    public void init(
        int    id,
        string name,
        string desc,
        string unit
    ) {
        GameResourceType t1 = null;
        Assert.DoesNotThrow(() => t1 = getTestGameResourceType(
            id  : id,
            name: name,
            desc: desc,
            unit: unit
        ));

        Assert.Multiple( () =>{
            Assert.AreEqual( id  , t1.id          );
            Assert.AreEqual( name, t1.name        );
            Assert.AreEqual( desc, t1.description );
            Assert.AreEqual( unit, t1.units       );
        });
    }
    
    
    
}


}
