using NUnit.Framework;
using WebApp_NativeTests.StaticTypes.GameResource;
using WebApp_slib.StaticTypes;
using static WebApp_slib.StaticTypes.PlanetType;

namespace WebApp_NativeTests.StaticTypes { 
[TestFixture(TestName = "Abstract Planet Type", TestOf = typeof(PlanetType))]
public class PlanetTypeTest {
	public static PlanetType getTestPlanetType(
		ElementId?      id    = null,
		AbstractImage   image = null,
		string          name  = "TestPlanetType",
		string          desc  = "TestDescription",
		YieldCalculator calc  = null
	) {
		ElementId _id = id.GetValueOrDefault(0);
		image = image ?? AbstractResourceTest.GetTestAbstractImage();
		calc = calc ?? NULL_YIELD;
		return TestConstructor.testBuildObject(
			() => new PlanetType(
				id: _id,
				icon: image,
				name: name,
				description: desc,
				yieldCalculator: calc
			)
		);
	}

	[Test]
	public static void init(

	) {
		var icon = AbstractResourceTest.GetTestAbstractImage();
		PlanetType t1 = null;
		var yieldResource = 
			SingularGameResourceYieldTest.getTestSingularGameResourceYield(
				type : GameResourceTest.getTestGameResourceType(),
				value: 500
			);
		var fullYield =
			GameResourceYieldTest.GetTestFinishedGameResourceYield(yieldResource);
		Assert.DoesNotThrow(() => t1 = getTestPlanetType(
			id    : 0,
			image : icon,
			name  : "Test1",
			desc  : "TestD1",
			calc  : count => fullYield   
		));

		var calcYield = t1.getYield(123);

		Assert.Multiple( () =>{
			Assert.AreEqual( 0         , t1.id          );
			Assert.AreEqual( "Test1"   , t1.name        );
			Assert.AreEqual( "TestD1"  , t1.description );
			Assert.AreEqual( icon      , t1.icon        );
			Assert.AreEqual( fullYield , calcYield      );
		});
	}


	[Test]
	public static void nothingCalculator() {
		Assert.AreEqual(
			MutableResourceYield.NOTHING_CONST, 
			NULL_YIELD.Invoke(123)
		);
	}
}
}