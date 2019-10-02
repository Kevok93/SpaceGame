using NUnit.Framework;
using WebApp_NativeTests.StaticTypes.GameResource;
using WebApp_slib.StaticTypes;
using static WebApp_NativeTests.StaticTypes.GameResource.SingularGameResourceYieldTest;
using static WebApp_slib.StaticTypes.ClusterType;

namespace WebApp_NativeTests.StaticTypes {
[TestFixture(TestName = "Abstract Planetary Cluster Type",TestOf = typeof(ClusterType))]
public class ClusterTypeTest {
	public static ClusterType getTestClusterType(
		ElementId?      id    = null,
		AbstractImage   image = null,
		string          name  = "TestClusterType",
		string          desc  = "TestDescription",
		YieldModifier   mod   = null
	) {
		ElementId _id = id.GetValueOrDefault(0);
		image = image ?? AbstractResourceTest.GetTestAbstractImage();
		mod  = mod  ?? NOOP_MODIFIER;
		return TestConstructor.testBuildObject(
			() => new ClusterType(
				id: _id,
				icon: image,
				name: name,
				description: desc,
				yieldModifier: mod
			)
		);
	}
	
	[Test]
	public static void init() {
		var         icon = AbstractResourceTest.GetTestAbstractImage();
		ClusterType t1   = null;
		var yieldResource = 
			getTestSingularGameResourceYield(
				type : GameResourceTest.getTestGameResourceType(),
				value: 500
			);
		var fullYield =
			GameResourceYieldTest.GetTestFinishedGameResourceYield(yields: yieldResource);
		YieldModifier yieldMod = (yield) => yield.scalePure(2).readOnly();
		Assert.DoesNotThrow(() => t1 = getTestClusterType(
			id    : 0,
			image : icon,
			name  : "Test1",
			desc  : "TestD1",
			mod   : yieldMod
		));
		

		Assert.Multiple( () =>{
			Assert.AreEqual   ( 0         , t1.id                    );
			Assert.AreEqual   ( "Test1"   , t1.name                  );
			Assert.AreEqual   ( "TestD1"  , t1.description           );
			Assert.AreEqual   ( icon      , t1.icon                  );
			Assert.AreNotSame ( fullYield , t1.modifyYield(fullYield));
			Assert.AreNotEqual( fullYield , t1.modifyYield(fullYield));
		});
	}

}
}