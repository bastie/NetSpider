using biz.ritter.javapi.io;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using biz.ritter.javapi.net;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.test.io
{
    
    
    /// <summary>
    ///This is a test class for FileTest and is intended
    ///to contain all FileTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FileTest
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
        private String tempPath = java.lang.SystemJ.getProperty("user.dir");
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            String userDir = java.lang.SystemJ.getProperty("user.dir");
            if (!userDir.EndsWith(java.lang.SystemJ.getProperty("path.separator")))
            {
                userDir += java.lang.SystemJ.getProperty("path.separator");
            }
        }
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
        ///A test for File Constructor
        ///</summary>
        [TestMethod()]
        public void FileConstructorTest()
        {
            string parent = this.tempPath; ; // TODO: Initialize to an appropriate value
            string child = "Test"; // TODO: Initialize to an appropriate value
            File target = new File(parent, child);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for File Constructor
        ///</summary>
        [TestMethod()]
        public void FileConstructorTest1()
        {
            URI uri = new URI("file://C:/1.txt"); // TODO: Initialize to an appropriate value
            try
            {
                File target = new File(uri);
                Assert.Fail("Wenn wir den Konstruktor implementieren, dann auch die Testmethode");
            }
            catch (java.lang.UnsupportedOperationException)
            {
            }
        }

        /// <summary>
        ///A test for File Constructor
        ///</summary>
        [TestMethod()]
        public void FileConstructorTest2()
        {
            string pathname = this.tempPath+"Hello"; // TODO: Initialize to an appropriate value
            File target = new File(pathname);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for File Constructor
        ///</summary>
        [TestMethod()]
        public void FileConstructorTest3()
        {
            File parent = null; // TODO: Initialize to an appropriate value
            string child = "Hello"; // TODO: Initialize to an appropriate value
            File target = new File(parent, child);
            File same = new File(child);
            Assert.IsNotNull(target);
            Assert.AreEqual(0, target.compareTo(same));
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            File target = new File(this.tempPath); // TODO: Initialize to an appropriate value
            string expected = this.tempPath; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for createNewFile
        ///</summary>
        [TestMethod()]
        public void createNewFileTest()
        {
            File target = new File(this.tempPath,"HereIAm.file"); // TODO: Initialize to an appropriate value
            if (target.exists()) target.delete();
            Assert.IsFalse(target.exists());
            bool created = target.createNewFile();
            Assert.IsTrue(created);
            target = new File(target.getAbsolutePath());
            Assert.IsTrue(target.exists());
        }

        /// <summary>
        ///A test for canRead
        ///</summary>
        [TestMethod()]
        public void canReadTest()
        {
            File target = new File(this.tempPath,"non"); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.canRead();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for compareTo
        ///</summary>
        [TestMethod()]
        public void compareToTest()
        {
            File target = new File("Hello"); // TODO: Initialize to an appropriate value
            File other = new File((String)null,"Hello"); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.compareTo(other);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for delete
        ///</summary>
        [TestMethod()]
        public void deleteTest()
        {
            File target = new File(tempPath,"Erease.Me"); // TODO: Initialize to an appropriate value
            target.createNewFile();
            Assert.IsTrue(target.exists());
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual = target.delete();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for exists
        ///</summary>
        [TestMethod()]
        public void existsTest()
        {
            String name = "notExists";
            File target = new File(name); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual = target.exists();
            Assert.AreEqual(expected, actual);

            target = new File(this.tempPath);
            expected = true;
            actual = target.exists();
            Assert.AreEqual(expected, actual);

            target = new File(string.Empty);
            expected = false;
            actual = target.exists();
            Assert.AreEqual(expected, actual);

            target = new File(tempPath, string.Empty);
            expected = true;
            actual = target.exists();
            Assert.AreEqual(expected, actual);

            target = new File(tempPath, name);
            expected = false;
            actual = target.exists();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getAbsolutePath
        ///</summary>
        [TestMethod()]
        public void getAbsolutePathTest()
        {
            String name = "Ritter.biz";
            File target = new File(this.tempPath, name); // TODO: Initialize to an appropriate value
            string expected = this.tempPath + java.lang.SystemJ.getProperty("file.separator") + name; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.getAbsolutePath();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getName
        ///</summary>
        [TestMethod()]
        public void getNameTest()
        {
            String name = "Ritter.biz";
            File target = new File(this.tempPath, name); // TODO: Initialize to an appropriate value
            string expected = name; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.getName();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getPath
        ///</summary>
        [TestMethod()]
        public void getPathTest()
        {
            File target = new File(this.tempPath); // TODO: Initialize to an appropriate value
            string expected = this.tempPath; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.getPath();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for isDirectory
        ///</summary>
        [TestMethod()]
        public void isDirectoryTest()
        {
            File target = new File("/"); // TODO: Initialize to an appropriate value
            Assert.IsTrue(target.isDirectory());
        }

        /// <summary>
        ///A test for isFile
        ///</summary>
        [TestMethod()]
        public void isFileTest()
        {
            File target = new File(this.tempPath); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.isFile();
            Assert.AreEqual(expected, actual);

            target = new File(this.tempPath,"Yes.file"); // TODO: Initialize to an appropriate value
            target.createNewFile();
            target = new File(target.getAbsolutePath()); // .net caching file status, so after create make new File instance
            Assert.IsTrue(target.exists());
            expected = true; // TODO: Initialize to an appropriate value
            actual = target.isFile();
            Assert.AreEqual(expected, actual);

            target = new File(string.Empty); // TODO: Initialize to an appropriate value
            expected = false; // TODO: Initialize to an appropriate value
            actual = target.isFile();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for lastModified
        ///</summary>
        [TestMethod()]
        public void lastModifiedTest()
        {
            long time = java.lang.SystemJ.currentTimeMillis();
            File target = new File(this.tempPath); // TODO: Initialize to an appropriate value
            long actual;
            actual = target.lastModified();
            Assert.IsTrue(actual < time);
        }

        /// <summary>
        ///A test for length
        ///</summary>
        [TestMethod()]
        public void lengthTest()
        {
            File target = new File(string.Empty); // TODO: Initialize to an appropriate value
            long expected = 0; // TODO: Initialize to an appropriate value
            long actual;
            actual = target.length();
            Assert.AreEqual(expected, actual);

            target = new File("Length.3");
            java.io.FileOutputStream fos = new java.io.FileOutputStream(target);
            fos.write("123".getBytes());
            fos.flush();

            fos.close();
            expected = 3;
            actual = target.length();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for list
        ///</summary>
        [TestMethod()]
        public void listTest()
        {
            File target = new File(this.tempPath); // TODO: Initialize to an appropriate value
            File toList = new File(target, "ListMe.file");
            toList.createNewFile();
            string[] actual = target.list();
            Assert.IsTrue(actual.Length > 0);
            Assert.IsTrue(actual[0].startsWith(target.getAbsolutePath()));
            bool findToList = false;
            foreach (String file in actual) {
                if (file.equals(toList.getAbsolutePath()))
                {
                    findToList = true;
                    break;
                }
            }
            Assert.IsTrue(findToList);
        }

        /// <summary>
        ///A test for listFiles
        ///</summary>
        [TestMethod()]
        public void listFilesTest()
        {
            File target = new File(this.tempPath); // TODO: Initialize to an appropriate value
            File[] actual;
            actual = target.listFiles();
            Assert.IsNotNull(actual);
        }

    }
}
