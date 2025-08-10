using AleksandarNaumovic.EntryPoint.Commands;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace AleksandarNaumovic.EntryPoint.Test.Commands;

[TestFixture]
[TestOf(typeof(ArrayComparator))]
public class ArrayComparatorTest
{
	private ArrayComparator comparator;

	[SetUp]
	public void SetUp()
	{
		comparator = new ArrayComparator();
	}

	[Test]
	public void TestBeginsWithTheSameLength()
	{
		Assert.IsTrue(comparator.Begins(["1", "2", "3", "4", "5"], ["1", "2", "3", "4", "5"]));
	}

	[Test]
	public void TestBeginsWithShorterBeginning()
	{
		Assert.IsTrue(comparator.Begins(["1", "2", "3", "4", "5"], ["1", "2", "3"]));
	}

	[Test]
	public void TestBeginsFalseWithLongerBeginning()
	{
		Assert.IsFalse(comparator.Begins(["1", "2", "3"], ["1", "2", "3", "4", "5"]));
	}

	[Test]
	public void TestBeginsFalseDoesNotMatch()
	{
		Assert.IsFalse(comparator.Begins(["1", "2", "3", "4", "5"], ["1", "2", "2"]));
	}
}