using WebApp_slib.StaticTypes;

namespace WebApp_NativeTests.StaticTypes {
	public class AbstractResourceTest {
		public static AbstractImage GetTestAbstractImage(
			int?   id   = null,
			string path = null
		) {
			int _id = id.GetValueOrDefault(0);
			path = path ?? "/dev/null";
			return TestConstructor.testBuildObject(
				() => new AbstractImage(
					id: _id,
					path: path
				)
			);
		}
	}
}