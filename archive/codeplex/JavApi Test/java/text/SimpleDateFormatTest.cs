using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using java = biz.ritter.javapi;

namespace biz.ritter.javapi.test.text
{
    [TestClass]
    public class SimpleDateFormatTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void testParseDateFormat()
        {
            java.text.SimpleDateFormat sdf = new java.text.SimpleDateFormat("dd.MM.yyyy");
            java.text.ParsePosition zeroPP = new java.text.ParsePosition (0);
            java.util.Date result = null;
            result = sdf.parse("05.09.1975", zeroPP);
            Assert.IsNotNull(result);
            Assert.AreEqual<int>(75, result.getYear(), "Year 75 expected");
            Assert.AreEqual<int>(9, result.getMonth(), "Month 9 expected");
            Assert.AreEqual<int>(5, result.getDate(), "Day of month 5 expected");
        }
    }
}
