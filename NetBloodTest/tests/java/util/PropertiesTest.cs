using NUnit.Framework;

using java = biz.ritter.javapi;

namespace vampire.test.util
{
    public class UnitTest_Properties
    {
        private java.lang.StringJ rootDir;
        
        [SetUp]
        public void SetUp () {
            this.rootDir = "../../../";
        }
    
        [Test]
        public void Test_LoadProperties()
        {
            java.util.Properties p = java.lang.SystemJ.getProperties();
            p.list(java.lang.SystemJ.outJ);
            p.load(new java.io.FileInputStream (new java.io.File(this.rootDir+"tests/data/java.util.Properties.properties")));
            p.list(java.lang.SystemJ.outJ);
			Assert.AreEqual("world", p.getProperty("hello"), "Propertyfile load test");
//			Assert.AreEqual("Ägypten", p.getProperty("state"), "Propertyfile load test");
        }
    }
}
