using AleksandarNaumovic.EntryPoint.Commands;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace AleksandarNaumovic.EntryPoint.Test.Commands
{
	[TestFixture]
	internal class ParameterInfoTest
	{
		[Test]
		public void TestConstructorNamed()
		{
			ParameterInfo info = new ParameterInfo("param1", true);

			Assert.AreEqual("param1", info.Name);
			Assert.IsTrue(info.IsRequired);
			Assert.AreEqual(ParameterInfo.ParameterType.NamedOption, info.Type);
			Assert.IsNull(info.EndPosition);
		}

		[Test]
		public void TestConstructorPositionedFromTheEnd()
		{
			ParameterInfo info = new ParameterInfo("param1", false, 4);

			Assert.AreEqual("param1", info.Name);
			Assert.IsFalse(info.IsRequired);
			Assert.AreEqual(ParameterInfo.ParameterType.PositionedFromTheEnd, info.Type);
			Assert.AreEqual(4, info.EndPosition);
		}
	}
}
