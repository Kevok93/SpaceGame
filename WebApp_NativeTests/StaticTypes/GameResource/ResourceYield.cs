using System;
using NUnit.Framework;
using WebApp_slib.StaticTypes;
using static WebApp_NativeTests.StaticTypes.GameResource.SingularGameResourceYieldTest;
using static WebApp_slib.StaticTypes.GameResourceType;
using static WebApp_slib.StaticTypes.ResourceYield;

namespace WebApp_NativeTests.StaticTypes.GameResource {

[TestFixture(TestName = "Game Resource Yield Tests", TestOf = typeof(ResourceYield))]
public class GameResourceYieldTest {
    
    
    
    public static ResourceYield GetTestResourceYield(
        params SingularYield[] yields
    ) {
        yields = yields ?? new[] {getTestSingularGameResourceYield()};
        return TestConstructor.testBuildObject(
            () => {
                var unfinishedYield = new ResourceYield();
                foreach (var yield in yields) {
                    unfinishedYield.Add(yield);
                }

                return unfinishedYield;
            }
        );
    }
    
    public static FinishedResourceYield GetTestFinishedGameResourceYield(
        params SingularYield[] yields
    ) {
        var unfinishedYield = GetTestResourceYield(yields);
        return TestConstructor.testBuildObject(
            () => unfinishedYield.readOnly()
        );
    }
        
    [Test]
    public void initUnfinished() {
        var returnType1 = GameResourceTest.getTestGameResourceType();
        var yield1      = new SingularYield(returnType1, 500);

        ResourceYield unfinishedYield = null;
        Assert.DoesNotThrow(
            () => unfinishedYield = GetTestResourceYield(yield1)
        );

        var yieldT = unfinishedYield.getYield(returnType1);
        Assert.That(unfinishedYield.Count       , Is.EqualTo(1));
        Assert.That(unfinishedYield             , Contains.Key(returnType1));
        Assert.That(unfinishedYield[returnType1], Is.EqualTo(500));
        Assert.That(yieldT.value                , Is.EqualTo(500));
        Assert.That(yieldT.type                 , Is.EqualTo(returnType1));
        unfinishedYield[returnType1] = 1000;
        Assert.That(unfinishedYield[returnType1], Is.EqualTo(1000));
        unfinishedYield.Set(yield1);
        Assert.That(unfinishedYield[returnType1], Is.EqualTo(500));
    }
    
    [Test]
    public void initFinished() {
        var returnType1 = GameResourceTest.getTestGameResourceType();
        var yield1 = new SingularYield(returnType1, 500);

        FinishedResourceYield finishedYield = null;
        Assert.DoesNotThrow(
            () => finishedYield = GetTestFinishedGameResourceYield(yield1)
        );

        var yieldT = finishedYield.getYield(returnType1);
        Assert.That(finishedYield.Count        , Is.EqualTo(1)             );
        Assert.That(finishedYield              , Contains.Key(returnType1) );
        Assert.That(finishedYield[returnType1] , Is.EqualTo(500)           );
        Assert.That(yieldT.value               , Is.EqualTo(500)           );
        Assert.That(yieldT.type                , Is.EqualTo(returnType1)   );
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

        FinishedResourceYield yield1 = GetTestFinishedGameResourceYield(
            singleYield1
        );
        FinishedResourceYield yield2 = GetTestFinishedGameResourceYield(
            singleYield2,
            singleYield4
        );
        FinishedResourceYield yield3 = GetTestFinishedGameResourceYield(
            singleYield3
        );
        FinishedResourceYield yieldN = null;

        ResourceYield yieldU1 = new ResourceYield();
        ResourceYield yieldU2 = yield1.cloneUnlocked();
        ResourceYield yieldUN = null;
        
        FinishedResourceYield sumI = yield1.combinePure(NOTHING);
        Assert.That(sumI.Count   , Is.EqualTo(1));
        Assert.That(sumI         , Contains.Key(type1));
        Assert.That(sumI[type1]  , Is.EqualTo(100));
        
        FinishedResourceYield sum1 = yield1.combinePure(yield2);
        Assert.That(sum1.Count   , Is.EqualTo(2));
        Assert.That(sum1         , Contains.Key(type1));
        Assert.That(sum1         , Contains.Key(type2));
        Assert.That(sum1[type1]  , Is.EqualTo(300));
        Assert.That(sum1[type2]  , Is.EqualTo(400));
        //Purity (A)
        Assert.That(yield1.Count , Is.EqualTo(1));
        Assert.That(yield1       , Contains.Key(type1));
        Assert.That(yield1[type1], Is.EqualTo(100));

        FinishedResourceYield sum2 = yield1.combinePure(yield3);
        Assert.That(sum2.Count   , Is.EqualTo(1));
        Assert.That(sum2         , Contains.Key(type1));
        Assert.That(sum2[type1]  , Is.EqualTo(-200));
        //Purity (B)
        Assert.That(yield3.Count , Is.EqualTo(1));
        Assert.That(yield3       , Contains.Key(type1));
        Assert.That(yield3[type1], Is.EqualTo(-300));

        FinishedResourceYield sumN1 = yield1.combinePure(yieldN);
        Assert.That(sumN1.Count  , Is.EqualTo(1));
        Assert.That(sumN1        , Contains.Key(type1));
        Assert.That(sumN1[type1] , Is.EqualTo(100));
        
        FinishedResourceYield sumN2 = yieldN.combinePure(yield1);
        Assert.That(sumN2.Count  , Is.EqualTo(1));
        Assert.That(sumN2        , Contains.Key(type1));
        Assert.That(sumN2[type1] , Is.EqualTo(100));

        FinishedResourceYield sumN3 = yieldN.combinePure(yieldN);
        Assert.That(sumN3.Count  , Is.EqualTo(0));
        Assert.That(sumN3        , Does.Not.ContainKey(type1));
        
        
        Assert.That(yieldU1.Count , Is.EqualTo(0));
        Assert.That(yieldU1       , Does.Not.ContainKey(type1));
        FinishedResourceYield sumU1 = yieldU1.combineDirty(yield1);
        Assert.That(yieldU1.Count , Is.EqualTo(1));
        Assert.That(yieldU1       , Contains.Key(type1));
        Assert.That(yieldU1[type1], Is.EqualTo(100));
        Assert.That(yieldU1       , Is.SameAs(sumU1));

        
        Assert.That(yieldU2.Count , Is.EqualTo(1));
        Assert.That(yieldU2       , Contains.Key(type1));
        Assert.That(yieldU2[type1], Is.EqualTo(100));
        FinishedResourceYield sumU2 = yieldU2.combineDirty(yield2);
        Assert.That(yieldU2.Count , Is.EqualTo(2));
        Assert.That(yieldU2       , Contains.Key(type1));
        Assert.That(yieldU2       , Contains.Key(type2));
        Assert.That(yieldU2[type1], Is.EqualTo(300));
        Assert.That(yieldU2[type2], Is.EqualTo(400));
        Assert.That(yieldU2       , Is.SameAs(sumU2));
        
        Exception sumUN = Assert.Catch(() => yieldUN.combineDirty(yield1));
        Assert.That(sumUN, Is.AssignableFrom<ArgumentNullException>());


    }

    [Test]
    public void scaling() {
        GameResourceType type1 = GameResourceTest.getTestGameResourceType(id: 1);
        GameResourceType type2 = GameResourceTest.getTestGameResourceType(id: 2);
        GameResourceType typeN = null;
        
        SingularYield singleYield1 = getTestSingularGameResourceYield(type: type1, value:  100);
        SingularYield singleYield2 = getTestSingularGameResourceYield(type: type1, value:  200);
        SingularYield singleYield4 = getTestSingularGameResourceYield(type: type2, value: -400);

        FinishedResourceYield yield1 = GetTestFinishedGameResourceYield(
            singleYield1
        );
        FinishedResourceYield yield2 = GetTestFinishedGameResourceYield(
            singleYield2,
            singleYield4
        );
        FinishedResourceYield yieldN = null;
        
        ResourceYield yieldU1 = new ResourceYield();
        ResourceYield yieldU2 = yield1.cloneUnlocked();
        ResourceYield yieldUN = null;

        FinishedResourceYield prodI = yield1.scalePure(1);
        Assert.That(prodI.Count   , Is.EqualTo(1));
        Assert.That(prodI         , Contains.Key(type1));
        Assert.That(prodI[type1]  , Is.EqualTo(100));
        
        FinishedResourceYield prod2 = yield2.scalePure(2);
        Assert.That(prod2.Count   , Is.EqualTo(2));
        Assert.That(prod2         , Contains.Key(type1));
        Assert.That(prod2[type1]  , Is.EqualTo( 400));
        Assert.That(prod2         , Contains.Key(type2));
        Assert.That(prod2[type2]  , Is.EqualTo(-800));
        //Purity (A)
        Assert.That(yield2.Count , Is.EqualTo(2));
        Assert.That(yield2       , Contains.Key(type1));
        Assert.That(yield2[type1], Is.EqualTo( 200));
        Assert.That(yield2       , Contains.Key(type2));
        Assert.That(yield2[type2], Is.EqualTo(-400));
        
        FinishedResourceYield prodN = yieldN.scalePure(2);
        Assert.That(prodN.Count  , Is.EqualTo(0));
        Assert.That(prodN        , Does.Not.ContainKey(type1));
        
        FinishedResourceYield prodU1 = yieldU1.scaleDirty(2);
        Assert.That(prodU1.Count  , Is.EqualTo(0));
        Assert.That(prodU1        , Does.Not.ContainKey(type1));
        
        Assert.That(yieldU2.Count , Is.EqualTo(1));
        Assert.That(yieldU2       , Contains.Key(type1));
        Assert.That(yieldU2[type1], Is.EqualTo(100));
        FinishedResourceYield prodU2 = yieldU2.scaleDirty(2);
        Assert.That(yieldU2.Count , Is.EqualTo(1));
        Assert.That(yieldU2       , Contains.Key(type1));
        Assert.That(yieldU2[type1], Is.EqualTo(200));
        Assert.That(yieldU2       , Is.SameAs(prodU2));
        
        Exception sumUN = Assert.Catch(() => yieldUN.scaleDirty(2));
        Assert.That(sumUN, Is.AssignableFrom<ArgumentNullException>());
    }


}
}