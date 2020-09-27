using System;
using java = biz.ritter.javapi;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace org.apache.test.commons.compress.bzip2
{
    /// <summary>
    /// Summary description for BZip
    /// </summary>
    [TestClass]
    public class BZip2Test
    {
        public BZip2Test()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void extractBZip2Test()
        {
            int buffersize = 4096;
            java.io.File bz2 = new java.io.File("C:\\Develop\\Projekte\\J.net\\JavApi Test\\org\\apache\\commons\\compress\\bzip2\\ASL2.bz2");
            java.lang.SystemJ.outJ.println(bz2.toString());
            java.io.FileInputStream fin = new java.io.FileInputStream(bz2);
            java.io.BufferedInputStream inJ = new java.io.BufferedInputStream(fin);
            java.io.FileOutputStream outJ = new java.io.FileOutputStream("C:\\Develop\\Projekte\\J.net\\JavApi Commons Compress (Apache Port)\\archive.tar");
            org.apache.commons.compress.compressors.bzip2.BZip2CompressorInputStream bzIn = new org.apache.commons.compress.compressors.bzip2.BZip2CompressorInputStream(inJ);
            byte[] buffer = new byte[buffersize];
            int n = 0;
            while (-1 != (n = bzIn.read(buffer)))
            {
                outJ.write(buffer, 0, n);
            }
            outJ.close();
            bzIn.close();
        }
    }
}
