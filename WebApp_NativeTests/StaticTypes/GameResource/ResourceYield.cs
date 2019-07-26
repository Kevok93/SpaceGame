using System;
using NUnit.Framework;
using WebApp_slib.StaticTypes;
using static WebApp_NativeTests.StaticTypes.GameResource.SingularGameResourceYieldTest;
using static WebApp_slib.StaticTypes.GameResourceType;
using static WebApp_slib.StaticTypes.ResourceYield;

namespace WebApp_NativeTests.StaticTypes.GameResource {

[TestFixture(TestName = "Game Resource Yield Tests", TestOf = typeof(ResourceYield))]
public class GameResourceYieldTest {
    
    
    
    public static UnfinishedResourceYield GetTestUnfinishedResourceYield(
        params SingularYield[] yields
    ) {
        yields = yields ?? new[] {getTestSingularGameResourceYield()};
        return TestConstructor.testBuildObject(
            () => {
                var unfinishedYield = new UnfinishedResourceYield();
                foreach (var yield in yields) {
                    unfinishedYield.Add(yield);
                }

                return unfinishedYield;
            }
        );
    }
    
    public static ResourceYield GetTestGameResourceYield(
        params SingularYield[] yields
    ) {
        var unfinishedYield = GetTestUnfinishedResourceYield(yields);
        return TestConstructor.testBuildObject(
            () => unfinishedYield.readOnly()
        );
    }
        
    [Test]
    public void initUnfinished() {
        var returnType1 = GameResourceTest.getTestGameResourceType();
        var yield1      = new SingularYield(returnType1, 500);

        UnfinishedResourceYield unfinishedYield = null;
        Assert.DoesNotThrow(
            () => unfinishedYield = GetTestUnfinishedResourceYield(yield1)
        );

        Assert.That(unfinishedYield.Count,        Is.EqualTo(1));
        Assert.That(unfinishedYield,              Contains.Key(returnType1));
        Assert.That(unfinishedYield[returnType1], Is.EqualTo(500));
        Assert.That(unfinishedYield.Get(returnType1).value, Is.EqualTo(500));
        Assert.That(unfinishedYield.Get(returnType1).type , Is.EqualTo(returnType1));
        unfinishedYield[returnType1] = 1000;
        Assert.That(unfinishedYield[returnType1], Is.EqualTo(1000));
        unfinishedYield.Set(yield1);
        Assert.That(unfinishedYield[returnType1], Is.EqualTo(500));
        

    }
    
    [Test]
    public void initFinished() {
        var returnType1 = GameResourceTest.getTestGameResourceType();
        var yield1 = new SingularYield(returnType1, 500);

        ResourceYield finishedYield = null;
        Assert.DoesNotThrow(
            () => finishedYield = GetTestGameResourceYield(yield1)
        );

        Assert.That(finishedYield.Count, Is.EqualTo(1));
        Assert.That(finishedYield, Contains.Key(returnType1));
        Assert.That(finishedYield[returnType1], Is.EqualTo(500));
        Assert.That(finishedYield.Get(returnType1).value, Is.EqualTo(500));
        Assert.That(finishedYield.Get(returnType1).type , Is.EqualTo(returnType1));
    }
    
    [Test]
    public void nullConstant() {
        var nullYield = NOTHING;

        Assert.That(nullYield.Count, Is.EqualTo(0));
    }
    
    [Test]
    public void addition() {
        GameResourceType type1 = GameResourceTest.getTestGameResourceType(id: 1);
        GameResourceType type2 = GameResourceTest.getTestGameResourceType(id: 2);
        GameResourceType typeN = null;
        
        SingularYield singleYield1 = getTestSingularGameResourceYield(type1,  100);
        SingularYield singleYield2 = getTestSingularGameResourceYield(type1,  200);
        SingularYield singleYield3 = getTestSingularGameResourceYield(type1, -300);
        SingularYield singleYield4 = getTestSingularGameResourceYield(type2,  400);

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

        ResourceYield sumN1 = yield1 + yieldN;
        Assert.That(sumN1.Count , Is.EqualTo(1));
        Assert.That(sumN1       , Contains.Key(type1));
        Assert.That(sumN1[type1], Is.EqualTo(100));
        
        ResourceYield sumN2 = yieldN + yield1;
        Assert.That(sumN2.Count , Is.EqualTo(1));
        Assert.That(sumN2       , Contains.Key(type1));
        Assert.That(sumN2[type1], Is.EqualTo(100));

        ResourceYield sumN3 = yieldN + yieldN;
        Assert.That(sumN3.Count , Is.EqualTo(0));
        Assert.That(sumN3       , Does.Not.ContainKey(type1));

    }
    
    
    
}
}