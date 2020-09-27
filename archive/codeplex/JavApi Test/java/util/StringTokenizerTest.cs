using biz.ritter.javapi.util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace biz.ritter.javapi.test.util
{
    
    
    /// <summary>
    ///This is a test class for StringTokenizerTest and is intended
    ///to contain all StringTokenizerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StringTokenizerTest
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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for StringTokenizer Constructor
        ///</summary>
        [TestMethod()]
        public void StringTokenizerConstructorTest()
        {
            string str = string.Empty; // TODO: Initialize to an appropriate value
            string delimiters = string.Empty; // TODO: Initialize to an appropriate value
            bool returnDelimiters = false; // TODO: Initialize to an appropriate value
            StringTokenizer target = new StringTokenizer(str, delimiters, returnDelimiters);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for StringTokenizer Constructor
        ///</summary>
        [TestMethod()]
        public void StringTokenizerConstructorTest1()
        {
            string str = string.Empty; // TODO: Initialize to an appropriate value
            string delimiters = string.Empty; // TODO: Initialize to an appropriate value
            StringTokenizer target = new StringTokenizer(str, delimiters);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for StringTokenizer Constructor
        ///</summary>
        [TestMethod()]
        public void StringTokenizerConstructorTest2()
        {
            string str = string.Empty; // TODO: Initialize to an appropriate value
            StringTokenizer target = new StringTokenizer(str);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for countTokens
        ///</summary>
        [TestMethod()]
        public void countTokensTest()
        {
            string str = "two tokens"; // TODO: Initialize to an appropriate value
            StringTokenizer target = new StringTokenizer(str); // TODO: Initialize to an appropriate value
            int expected = 2; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.countTokens();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for hasMoreElements
        ///</summary>
        [TestMethod()]
        public void hasMoreElementsTest()
        {
            string str = "1 2"; // TODO: Initialize to an appropriate value
            StringTokenizer target = new StringTokenizer(str); // TODO: Initialize to an appropriate value
            int expected = 2; // TODO: Initialize to an appropriate value
            int actual = 0;
            while (target.hasMoreElements())
            {
                actual++;
                target.nextElement();
            }
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for hasMoreTokens
        ///</summary>
        [TestMethod()]
        public void hasMoreTokensTest()
        {
            string str = "1 2 3"; // TODO: Initialize to an appropriate value
            StringTokenizer target = new StringTokenizer(str); // TODO: Initialize to an appropriate value
            int expected = 3; // TODO: Initialize to an appropriate value
            int actual = 0;
            while (target.hasMoreTokens())
            {
                actual++;
                target.nextToken();
            }
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for nextElement
        ///</summary>
        [TestMethod()]
        public void nextElementTest()
        {
            string str = "next"; // TODO: Initialize to an appropriate value
            StringTokenizer target = new StringTokenizer(str); // TODO: Initialize to an appropriate value
            object expected = str; // TODO: Initialize to an appropriate value
            object actual;
            actual = target.nextElement();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for nextToken
        ///</summary>
        [TestMethod()]
        public void nextTokenTest()
        {
            string str = "next"; // TODO: Initialize to an appropriate value
            StringTokenizer target = new StringTokenizer(str); // TODO: Initialize to an appropriate value
            string delims = "x"; // TODO: Initialize to an appropriate value
            string expected = "ne"; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.nextToken(delims);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for nextToken
        ///</summary>
        [TestMethod()]
        public void nextTokenTest1()
        {
            string str = "next"; // TODO: Initialize to an appropriate value
            StringTokenizer target = new StringTokenizer(str); // TODO: Initialize to an appropriate value
            string expected = str; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.nextToken();
            Assert.AreEqual(expected, actual);
        }
    }
}
