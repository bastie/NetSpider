using biz.ritter.javapi.util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace biz.ritter.javapi.test.util
{
    
    
    /// <summary>
    ///This is a test class for ArraysTest and is intended
    ///to contain all ArraysTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ArraysTest
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
        ///A test for getIndexArray
        ///</summary>
        public void getIndexArrayTestHelper<T>()
        {
            int[,] source = new int [2,3]; 
            source[0, 0] = 0;
            source[0, 1] = 1;
            source[0, 2] = 2;
            source[1, 0] = 10;
            source[1, 1] = 11;
            source[1, 2] = 12;
            int index = 0; 
            int[] expected = new int[]{0,1,2}; 
            int[] actual = Arrays<int>.getIndexArray(source, index);
            Assert.IsTrue(this.equalArray(expected,actual));
            index = 1; 
            expected = new int[] { 10, 11, 12 };
            actual = Arrays<int>.getIndexArray(source, index);
            Assert.IsTrue(this.equalArray(expected, actual));
        }

        private bool equalArray<T,S>(T[] arrayOne, S[] arrayTwo)
        {
            if (null == arrayOne && null == arrayTwo) return true;
            if (null == arrayOne || null == arrayTwo) return false;
            if (arrayOne.Length != arrayTwo.Length) return false;
            bool ok = true;
            for (int i = 0; i < arrayOne.Length && ok; i++)
            {
                if (arrayOne[i] == null && arrayTwo != null) ok = false;
                else if (arrayOne[i] != null && arrayTwo == null) ok = false;
                else if (arrayOne[i] != null && arrayTwo != null) ok = arrayOne[i].Equals(arrayTwo[i]);
            }
            return ok;
        }

        [TestMethod()]
        public void getIndexArrayTest()
        {
            getIndexArrayTestHelper<GenericParameterHelper>();
        }
    }
}
