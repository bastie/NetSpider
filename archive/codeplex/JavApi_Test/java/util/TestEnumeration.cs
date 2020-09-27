using System;
using NUnit.Framework;
using java = biz.ritter.javapi;

namespace biz.ritter.test.javapi.util
{
	[TestFixture()]
	public class TestEnumeration
	{
		public void TestCase ()
		{
		}

		[Test()]
		public void testEmptyHashtableKeyEnumerator() {
			java.util.Hashtable<String, String> nameWert = new java.util.Hashtable<String, String>();
			NUnit.Framework.Assert.False(nameWert.keys().hasMoreElements());
		}

		[Test()]
		public void testOneElementHashtableKeyEnumerator() {
			java.util.Hashtable<String, String> nameWert = new java.util.Hashtable<String, String>();
			nameWert.put("1","one");

			java.util.Enumeration<String> names = nameWert.keys();

			Assert.True (names.hasMoreElements());
			String one = names.nextElement();
			Assert.AreEqual("1",one);
			one = nameWert.get(one);
			Assert.AreEqual("one",one);
			Assert.False(names.hasMoreElements());

		}

		[Test()]
		public void testMoreElementHashtableKeyEnumerator() {
			java.util.Hashtable<String, String> nameWert = new java.util.Hashtable<String, String>();
			nameWert.put("1","one");
			nameWert.put("2","one");

			java.util.Enumeration<String> names = nameWert.keys();

			Assert.True (names.hasMoreElements());
			names.nextElement();
			Assert.True (names.hasMoreElements());
			names.nextElement();
			Assert.False(names.hasMoreElements());

		}
	}
}

