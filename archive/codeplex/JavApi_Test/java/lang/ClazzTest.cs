using System;
using NUnit.Framework;

using java = biz.ritter.javapi;

namespace biz.ritter.test.javapi.lang
{
    [TestFixture()]
    public class ClazzTest
    {
        [Test()]
        public void TestMethod1()
        {
            java.lang.Class clazz = this.getClass();
            String name = clazz.getName();
            Assert.IsTrue("ClazzTest".equalsIgnoreCase(name));
        }
    }
}
