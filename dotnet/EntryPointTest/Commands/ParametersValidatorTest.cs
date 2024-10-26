using AleksandarNaumovic.EntryPoint.Commands;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace AleksandarNaumovic.EntryPoint.Test.Commands
{
	[TestFixture]
	internal class ParametersValidatorTest
	{
		private ParametersValidator validator;

		[SetUp]
		public void SetUp()
		{
			validator = new ParametersValidator();
		}

		[Test]
		public void TestValidateReqired()
		{
			IList<ParameterInfo> info = new List<ParameterInfo>();

			info.Add(new ParameterInfo("firstname", true));
			info.Add(new ParameterInfo("lastname", true));
			info.Add(new ParameterInfo("age", false));

			IDictionary<string, string> parameters = new Dictionary<string, string>();

			parameters.Add("firstname", "name first");
			parameters.Add("lastname", "name last");

			Assert.IsTrue(validator.Validate(info, parameters));
		}

		[Test]
		public void TestValidateReqiredMissingFirst()
		{
			IList<ParameterInfo> info = new List<ParameterInfo>();

			info.Add(new ParameterInfo("firstname", true));
			info.Add(new ParameterInfo("lastname", true));
			info.Add(new ParameterInfo("age", false));

			IDictionary<string, string> parameters = new Dictionary<string, string>();

			//parameters.Add("firstname", "name first");
			parameters.Add("lastname", "name last");

			Assert.IsFalse(validator.Validate(info, parameters));
		}

		[Test]
		public void TestValidateReqiredMissingSecond()
		{
			IList<ParameterInfo> info = new List<ParameterInfo>();

			info.Add(new ParameterInfo("firstname", true));
			info.Add(new ParameterInfo("lastname", true));
			info.Add(new ParameterInfo("age", false));

			IDictionary<string, string> parameters = new Dictionary<string, string>();

			parameters.Add("firstname", "name first");
			//parameters.Add("lastname", "name last");

			Assert.IsFalse(validator.Validate(info, parameters));
		}
	}
}
