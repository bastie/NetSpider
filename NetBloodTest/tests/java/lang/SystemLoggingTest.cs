using NUnit.Framework;

using java = biz.ritter.javapi;

namespace vampire.test.lang
{
    public class UnitTest_SystemLogging
    {
        
        [SetUp]
        public void SetUp () {
        }
    
        [Test]
        public void Test_SetSecurityManager()
        {
        }

        [Test]
        public void Test_Loglevel_Value() {
          /// see Javadoc for Enum System.Logger.Level
          Assert.AreEqual(java.util.logging.Level.ALL.intValue(), (int) java.lang.SystemJ.Logger.Level.ALL);
          Assert.AreEqual(java.util.logging.Level.FINER.intValue(), (int) java.lang.SystemJ.Logger.Level.TRACE);
          Assert.AreEqual(java.util.logging.Level.FINE.intValue(), (int) java.lang.SystemJ.Logger.Level.DEBUG);
          Assert.AreEqual(java.util.logging.Level.INFO.intValue(), (int) java.lang.SystemJ.Logger.Level.INFO);
          Assert.AreEqual(java.util.logging.Level.WARNING.intValue(), (int) java.lang.SystemJ.Logger.Level.WARNING);
          Assert.AreEqual(java.util.logging.Level.SEVERE.intValue(), (int) java.lang.SystemJ.Logger.Level.ERROR);
          Assert.AreEqual(java.util.logging.Level.OFF.intValue(), (int) java.lang.SystemJ.Logger.Level.OFF);
        } 
    }
}