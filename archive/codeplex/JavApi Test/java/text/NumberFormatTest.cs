using System;
using java = biz.ritter.javapi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.ritter.javapi.test.text
{
    [TestClass]
    public class NumberFormatTest
    {
        [TestMethod]
        public void TestSimpleIntegerNumberFormat()
        {
            Object[,] expected = new Object[,] {
                {0d, "0"},
                {1d, "1"},
                {-1d, "-1"},
                {.1d, "0"},
                {.4d, "0"},
                {.5d, "0"},
                {.6d, "1"}
            };
            java.text.NumberFormat inf = java.text.NumberFormat.getIntegerInstance();
            for (int i = 0; i < expected.Length / expected.Rank; i++)
            {
                String actual = inf.format((double)expected[i, 0]);
                Assert.AreEqual((String)expected[i, 1], actual, false, "Test " + expected[i, 0] + " failed with result: " + actual);
            }
        }
    }
}
