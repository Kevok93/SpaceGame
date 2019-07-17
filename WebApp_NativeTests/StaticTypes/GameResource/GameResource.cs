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

    public static GameResourceType GetTestGameResourceType(
        int    id = 0,
        string name = "TestResourceType",
        string desc = "TestDescription",
        string unit = "TestUnit"
    ) {
        try {
            return new GameResourceType(
                id: id,
                name: name,
                description: desc,
                units: unit
            );
        }
        catch (Exception e) {
            throw new InconclusiveException(
                message: "Could not initialize a GameResource",
                inner: e
            );
        }
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
        Assert.DoesNotThrow(() => t1 = GetTestGameResourceType(
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


[TestFixture(TestName = "Game Resource Yield Tests", TestOf = typeof(ResourceYield))]
public class GameResourceYieldTest {
    
    
    
    public static UnfinishedResourceYield GetTestUnfinishedResourceYield(
        params SingularYield[] yields
    ) {
        yields = yields ?? new[] {GetTestSingularGameResourceYield()};
        try {
            var unfinishedYield = new UnfinishedResourceYield();
            foreach (var yield in yields) {
                unfinishedYield.Add(yield);
            }

            return unfinishedYield;

        }
        catch (Exception e) {
            throw new InconclusiveException(
                message: "Could not initialize a UnfinishedResourceYield",
                inner: e
            );
        }
    }
    
    public static ResourceYield GetTestGameResourceYield(
        params SingularYield[] yields
    ) {
        var unfinishedYield = GetTestUnfinishedResourceYield(yields);
        try { return unfinishedYield.readOnly(); }
        catch (Exception e) {
            throw new InconclusiveException(
                message: "Could not initialize a ResourceYield",
                inner: e
            );
        }
    }
    
    [Test]
    public void init() {
        var returnType1 = GameResourceTest.GetTestGameResourceType();
        var yield1 = new SingularYield(returnType1, 500);

        ResourceYield finishedYield = null;
        Assert.DoesNotThrow(() => finishedYield = GetTestGameResourceYield(
            yield1
        ));

        Assert.That(finishedYield.Count, Is.EqualTo(1));
        Assert.That(finishedYield, Contains.Key(returnType1));
        Assert.That(finishedYield[returnType1], Is.EqualTo(500));
    }
    
    [Test]
    public void nullConstant() {
        var nullYield = NOTHING;

        Assert.That(nullYield.Count, Is.EqualTo(0));
    }
    
    [Test]
    public void addition() {
        GameResourceType type1 = GameResourceTest.GetTestGameResourceType(id: 1);
        GameResourceType type2 = GameResourceTest.GetTestGameResourceType(id: 2);
        GameResourceType typeN = null;
        
        SingularYield singleYield1 = GetTestSingularGameResourceYield(type1,  100);
        SingularYield singleYield2 = GetTestSingularGameResourceYield(type1,  200);
        SingularYield singleYield3 = GetTestSingularGameResourceYield(type1, -300);
        SingularYield singleYield4 = GetTestSingularGameResourceYield(type2,  400);

        ResourceYield yield1 = GetTestGameResourceYield(
            singleYield1
        );
        ResourceYield yield2 = GetTestGameResourceYield(
            singleYield2,
            singleYield4
        );
        ResourceYield yield3 = GetTestGameResourceYield(
            singleYield3
        );
        ResourceYield yieldN = null;
        
        
        ResourceYield sumI = yield1 + NOTHING;
        Assert.That(sumI.Count , Is.EqualTo(1));
        Assert.That(sumI       , Contains.Key(type1));
        Assert.That(sumI[type1], Is.EqualTo(100));
        
        ResourceYield sum1 = yield1 + yield2;
        Assert.That(sum1.Count   , Is.EqualTo(2));
        Assert.That(sum1         , Contains.Key(type1));
        Assert.That(sum1         , Contains.Key(type2));
        Assert.That(sum1[type1]  , Is.EqualTo(300));
        Assert.That(sum1[type2]  , Is.EqualTo(400));
        //Purity (A)
        Assert.That(yield1.Count , Is.EqualTo(1));
        Assert.That(yield1       , Contains.Key(type1));
        Assert.That(yield1[type1], Is.EqualTo(100));

        ResourceYield sum2 = yield1 + yield3;
        Assert.That(sum2.Count , Is.EqualTo(1));
        Assert.That(sum2       , Contains.Key(type1));
        Assert.That(sum2[type1], Is.EqualTo(-200));
        //Purity (B)
        Assert.That(yield3.Count , Is.EqualTo(1));
        Assert.That(yield3       , Contains.Key(type1));
        Assert.That(yield3[type1], Is.EqualTo(-300));

        ResourceYield sumN = yield1 + yieldN;
        Assert.That(sumI.Count , Is.EqualTo(1));
        Assert.That(sumI       , Contains.Key(type1));
        Assert.That(sumI[type1], Is.EqualTo(100));


    }
    
    
    
}
}
