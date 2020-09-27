using biz.ritter.javapi.io;
using NUnit.Framework;
using System;
using biz.ritter.javapi.net;
using java = biz.ritter.javapi;

namespace biz.ritter.test.javapi.io
{
    
    
    /// <summary>
    ///This is a test class for FileTest and is intended
    ///to contain all FileTest Unit Tests
    ///</summary>
    [TestFixture()]
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
        #endregion


        /// <summary>
        ///A test for File Constructor
        ///</summary>
        [Test()]
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
        [Test()]
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
        [Test()]
        public void FileConstructorTest2()
        {
            string pathname = this.tempPath+"Hello"; // TODO: Initialize to an appropriate value
            File target = new File(pathname);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for File Constructor
        ///</summary>
        [Test()]
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
        [Test()]
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
        [Test()]
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
        [Test()]
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
        [Test()]
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
        [Test()]
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
        [Test()]
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
        [Test()]
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
        [Test()]
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
        [Test()]
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
        [Test()]
        public void isDirectoryTest()
        {
            File target = new File("/"); // TODO: Initialize to an appropriate value
            Assert.IsTrue(target.isDirectory());
        }

        /// <summary>
        ///A test for isFile
        ///</summary>
        [Test()]
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
        [Test()]
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
        [Test()]
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
        [Test()]
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
        [Test()]
        public void listFilesTest()
        {
            File target = new File(this.tempPath); // TODO: Initialize to an appropriate value
            File[] actual;
            actual = target.listFiles();
            Assert.IsNotNull(actual);
        }

    }
}
