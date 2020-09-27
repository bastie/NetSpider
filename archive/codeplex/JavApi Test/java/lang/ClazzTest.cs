using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using java = biz.ritter.javapi;

namespace biz.ritter.javapi.test.lang
{
    [TestClass]
    public class ClazzTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            java.lang.Class clazz = this.getClass();
            String name = clazz.getName();
            Assert.IsTrue("ClazzTest".equalsIgnoreCase(name));
        }
    }
}
