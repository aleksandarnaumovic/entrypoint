using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace AleksandarNaumovic.EntryPoint.Test
{
	[TestFixture]
	public class EntryPointTest
	{
		[Test]
		public void TestGetInstance()
		{
			EntryPointConfiguration config = new EntryPointConfiguration();

			Assert.NotNull(EntryPoint.GetInstance(config));
			Assert.AreSame(EntryPoint.GetInstance(config), EntryPoint.GetInstance(config));
		}

		[Test]
		public void TestExecuteDefaultMessage()
		{
			EntryPointConfiguration config = new EntryPointConfiguration();
			Assert.AreEqual("\r\nEntryPoint v1.0.0 (C) Aleksandar Naumovic 2024.", new EntryPoint(config).Execute(new string[0]));
		}
	}
}
