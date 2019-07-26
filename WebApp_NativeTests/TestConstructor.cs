using System;
using NUnit.Framework;

namespace WebApp_NativeTests {
	public static class TestConstructor {
		public static T testBuildObject<T>(Func<T> ctor) {
				try { return ctor.Invoke(); } 
				catch (Exception e) {
					string tName = typeof(T).Name;
					throw new InconclusiveException(
						message: $"Could not initialize a {tName}",
						inner: e
					);
				}
			
		}
	}
}