using System;
using NUnit.Framework;
using WebApp_slib.StaticTypes;
using static WebApp_NativeTests.StaticTypes.GameResource.SingularGameResourceYieldTest;
using static WebApp_slib.StaticTypes.GameResourceType;
using static WebApp_slib.StaticTypes.MutableResourceYield;

namespace WebApp_NativeTests.StaticTypes.GameResource {

[TestFixture(TestName = "Game Resource Yield Tests", TestOf = typeof(MutableResourceYield))]
public class GameResourceYieldTest {
    
    
    
    public static MutableResourceYield GetTestResourceYield(
        params SingularYield[] yields
    ) {
        yields = yields ?? new[] {getTestSingularGameResourceYield()};
        return TestConstructor.testBuildObject(
            () => {
                var unfinishedYield = new MutableResourceYield();
                foreach (var yield in yields) {
                    unfinishedYield.Add(yield);
                }

                return unfinishedYield;
            }
        );
    }
    
    public static ResourceYield GetTestFinishedGameResourceYield(
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

        MutableResourceYield? _unfinishedYield = null;
        Assert.DoesNotThrow(
            () => _unfinishedYield = GetTestResourceYield(yield1)
        );
        MutableResourceYield unfinishedYield = _unfinishedYield.Value;

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

        ResourceYield? finishedYield = null;
        Assert.DoesNotThrow(
            () => finishedYield = GetTestFinishedGameResourceYield(yield1)
        );

        var yieldT = finishedYield?.getYield(returnType1);
        Assert.That(finishedYield?.Count        , Is.EqualTo(1)             );
        Assert.That(finishedYield               , Contains.Key(returnType1) );
        Assert.That(finishedYield?[returnType1] , Is.EqualTo(500)           );
        Assert.That(yieldT?.value               , Is.EqualTo(500)           );
        Assert.That(yieldT?.type                , Is.EqualTo(returnType1)   );
    }
    
    [Test]
    public void nullConstant() {
        var nullYield = NOTHING_CONST;

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

        ResourceYield  yield1 = GetTestFinishedGameResourceYield(
            singleYield1
        );
        ResourceYield yield2 = GetTestFinishedGameResourceYield(
            singleYield2,
            singleYield4
        );
        ResourceYield yield3 = GetTestFinishedGameResourceYield(
            singleYield3
        );

        MutableResourceYield yieldU1 = new MutableResourceYield();
        MutableResourceYield yieldU2 = yield1.cloneUnlocked();
        
        ResourceYield sumI = yield1.combinePure(NOTHING_CONST);
        Assert.That(sumI.Count   , Is.EqualTo(1));
        Assert.That(sumI         , Contains.Key(type1));
        Assert.That(sumI[type1]  , Is.EqualTo(100));
        
        ResourceYield sum1 = yield1.combinePure(yield2);
        Assert.That(sum1.Count   , Is.EqualTo(2));
        Assert.That(sum1         , Contains.Key(type1));
        Assert.That(sum1         , Contains.Key(type2));
        Assert.That(sum1[type1]  , Is.EqualTo(300));
        Assert.That(sum1[type2]  , Is.EqualTo(400));
        //Purity (A)
        Assert.That(yield1.Count , Is.EqualTo(1));
        Assert.That(yield1       , Contains.Key(type1));
        Assert.That(yield1[type1], Is.EqualTo(100));

        ResourceYield sum2 = yield1.combinePure(yield3);
        Assert.That(sum2.Count   , Is.EqualTo(1));
        Assert.That(sum2         , Contains.Key(type1));
        Assert.That(sum2[type1]  , Is.EqualTo(-200));
        //Purity (B)
        Assert.That(yield3.Count , Is.EqualTo(1));
        Assert.That(yield3       , Contains.Key(type1));
        Assert.That(yield3[type1], Is.EqualTo(-300));


        Assert.That(yieldU1.Count , Is.EqualTo(0));
        Assert.That(yieldU1       , Does.Not.ContainKey(type1));
        ResourceYield sumU1 = yieldU1.combineDirty(yield1);
        Assert.That(yieldU1.Count , Is.EqualTo(1));
        Assert.That(yieldU1       , Contains.Key(type1));
        Assert.That(yieldU1[type1], Is.EqualTo(100));
        Assert.That(yieldU1       , Is.EqualTo(sumU1));

        
        Assert.That(yieldU2.Count , Is.EqualTo(1));
        Assert.That(yieldU2       , Contains.Key(type1));
        Assert.That(yieldU2[type1], Is.EqualTo(100));
        ResourceYield sumU2 = yieldU2.combineDirty(yield2);
        Assert.That(yieldU2.Count , Is.EqualTo(2));
        Assert.That(yieldU2       , Contains.Key(type1));
        Assert.That(yieldU2       , Contains.Key(type2));
        Assert.That(yieldU2[type1], Is.EqualTo(300));
        Assert.That(yieldU2[type2], Is.EqualTo(400));
        Assert.That(yieldU2       , Is.EqualTo(sumU2));
        
    }

    [Test]
    public void scaling() {
        GameResourceType type1 = GameResourceTest.getTestGameResourceType(id: 1);
        GameResourceType type2 = GameResourceTest.getTestGameResourceType(id: 2);
        GameResourceType typeN = null;
        
        SingularYield singleYield1 = getTestSingularGameResourceYield(type: type1, value:  100);
        SingularYield singleYield2 = getTestSingularGameResourceYield(type: type1, value:  200);
        SingularYield singleYield4 = getTestSingularGameResourceYield(type: type2, value: -400);

        ResourceYield yield1 = GetTestFinishedGameResourceYield(
            singleYield1
        );
        ResourceYield yield2 = GetTestFinishedGameResourceYield(
            singleYield2,
            singleYield4
        );
        ResourceYield? yieldN1 = null;
        
        MutableResourceYield yieldU1 = new MutableResourceYield();
        MutableResourceYield yieldU2 = yield1.cloneUnlocked();

        MutableResourceYield prodI = yield1.scalePure(1);
        Assert.That(prodI.Count   , Is.EqualTo(1));
        Assert.That(prodI         , Contains.Key(type1));
        Assert.That(prodI[type1]  , Is.EqualTo(100));
        
        MutableResourceYield prod2 = yield2.scalePure(2);
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
        
        ResourceYield prodN1 = yieldN1.scalePure(2);
        Assert.That(prodN1.Count  , Is.EqualTo(0));
        Assert.That(prodN1        , Does.Not.ContainKey(type1));
        
        ResourceYield prodN2 = yieldN1 * 2;
        Assert.That(prodN2.Count  , Is.EqualTo(0));
        Assert.That(prodN2        , Does.Not.ContainKey(type1));
        
        ResourceYield prodU1 = yieldU1.scaleDirty(2);
        Assert.That(prodU1.Count  , Is.EqualTo(0));
        Assert.That(prodU1        , Does.Not.ContainKey(type1));
        
        Assert.That(yieldU2.Count , Is.EqualTo(1));
        Assert.That(yieldU2       , Contains.Key(type1));
        Assert.That(yieldU2[type1], Is.EqualTo(100));
        ResourceYield prodU2 = yieldU2.scaleDirty(2);
        Assert.That(yieldU2.Count , Is.EqualTo(1));
        Assert.That(yieldU2       , Contains.Key(type1));
        Assert.That(yieldU2[type1], Is.EqualTo(200));
        Assert.That(yieldU2       , Is.EqualTo(prodU2));
        
    }
    
    [Test]
    public void finishedYieldWrappers() {
        var returnType1 = GameResourceTest.getTestGameResourceType();
        var yield1      = new SingularYield(returnType1, 500);

        var unfinishedYield = GetTestResourceYield(yield1);
        var   finishedYield = unfinishedYield.readOnly();
        
        Assert.AreEqual(
            unfinishedYield.Values,
              finishedYield.Values
        );
    }


}
}