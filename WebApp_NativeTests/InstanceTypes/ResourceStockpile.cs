using NUnit.Framework;
using WebApp_NativeTests.StaticTypes.GameResource;
using WebApp_slib.InstanceTypes;
using WebApp_slib.StaticTypes;

namespace WebApp_NativeTests.InstanceTypes {
public class ResourceStockpileTest {
	
	public static ResourceStockpile GetTestResourceYield(
		GameResourceType type = null,
		int value = 100
	) {
		type = type ?? GameResourceTest.getTestGameResourceType();
		return TestConstructor.testBuildObject(
			() => new ResourceStockpile(type, value)
		);
	}

	[Test]
	public static void init() {
		GameResourceType testType = GameResourceTest.getTestGameResourceType();
		ResourceStockpile? testObj = null;
		Assert.DoesNotThrow(() => {
			testObj = GetTestResourceYield(
				type: testType,
				value: 123
			);
		});

		Assert.IsNotNull(testObj                      );
		Assert.AreSame  (testObj.Value.type , testType);
		Assert.AreEqual (testObj.Value.value, 123     );

		Assert.Catch(() => new ResourceStockpile(null, 123));
		Assert.DoesNotThrow(() => testObj.ToString());
	}

	[Test]
	public static void math() {
		
		GameResourceType type1 = GameResourceTest.getTestGameResourceType(id: 1);
		GameResourceType type2 = GameResourceTest.getTestGameResourceType(id: 2);

		ResourceStockpile stock1  = GetTestResourceYield(type: type1, value: 100);
		ResourceStockpile stock2  = GetTestResourceYield(type: type1, value: 200);
		ResourceStockpile stock3  = GetTestResourceYield(type: type2, value: 300);

		//Addition
		Assert.That(stock1.canCombine(stock2));
		ResourceStockpile  sum1   = stock1 + stock2;
		Assert.AreEqual(stock1.type , type1);
		Assert.AreEqual(stock1.value,   100);
		Assert.AreEqual(stock2.type , type1);
		Assert.AreEqual(stock2.value,   200);
		Assert.AreEqual(  sum1.type , type1);
		Assert.AreEqual(  sum1.value,   300);

		ResourceStockpile? sum2   = null;
		Assert.That(!stock1.canCombine(stock3));
		Assert.Catch(() => sum2 = stock1 + stock3);
		Assert.AreEqual(stock1.type , type1);
		Assert.AreEqual(stock1.value,   100);
		Assert.AreEqual(stock3.type , type2);
		Assert.AreEqual(stock3.value,   300);
		Assert.IsNull(sum2);

		ResourceStockpile  sum3   = stock1 + 300;
		Assert.AreEqual(stock1.type , type1);
		Assert.AreEqual(stock1.value, 100);
		Assert.AreEqual(  sum3.type , type1);
		Assert.AreEqual(  sum3.value, 400);

		//Subtraction
		ResourceStockpile  diff1  = stock1 - stock2;
		Assert.AreEqual(stock1.type , type1);
		Assert.AreEqual(stock1.value,   100);
		Assert.AreEqual(stock2.type , type1);
		Assert.AreEqual(stock2.value,   200);
		Assert.AreEqual( diff1.type , type1);
		Assert.AreEqual( diff1.value,  -100);

		ResourceStockpile? diff2  = null;
		Assert.Catch(() => diff2 = stock1 - stock3);
		Assert.AreEqual(stock1.type , type1);
		Assert.AreEqual(stock1.value,   100);
		Assert.AreEqual(stock3.type , type2);
		Assert.AreEqual(stock3.value,   300);
		Assert.IsNull(diff2);

		ResourceStockpile  diff3  = stock1 - 300;
		Assert.AreEqual(stock1.type , type1);
		Assert.AreEqual(stock1.value,   100);
		Assert.AreEqual( diff3.type , type1);
		Assert.AreEqual( diff3.value,  -200);
		
		//Scaling
		ResourceStockpile  prod1  = stock1 * 7;
		Assert.AreEqual(stock1.type , type1);
		Assert.AreEqual(stock1.value,   100);
		Assert.AreEqual( prod1.type , type1);
		Assert.AreEqual( prod1.value,   700);
	}

	[Test]
	public static void equality() {

		GameResourceType type1 = GameResourceTest.getTestGameResourceType(id: 1);
		GameResourceType type2 = GameResourceTest.getTestGameResourceType(id: 2);

		ResourceStockpile stock1 = GetTestResourceYield(type: type1, value: 100);
		ResourceStockpile stock2 = GetTestResourceYield(type: type1, value: 200);
		ResourceStockpile stock3 = GetTestResourceYield(type: type2, value: 300);
		ResourceStockpile stock4 = GetTestResourceYield(type: type1, value: 100);
		
		Assert.AreNotEqual(stock1, stock2);
		Assert.AreNotEqual(stock1, stock3);
		Assert.AreEqual   (stock1, stock4);
		
	}
	
	[Test]
	public static void comparison() {

		GameResourceType type1 = GameResourceTest.getTestGameResourceType(id: 1);
		GameResourceType type2 = GameResourceTest.getTestGameResourceType(id: 2);

		ResourceStockpile stock1 = GetTestResourceYield(type: type1, value: 100);
		ResourceStockpile stock2 = GetTestResourceYield(type: type1, value: 200);
		ResourceStockpile stock3 = GetTestResourceYield(type: type2, value: 300);

		Assert.That(stock1.CompareTo(stock2), Is.EqualTo (0));
		Assert.That(stock1.CompareTo(stock3), Is.LessThan(0));

	}
}
}