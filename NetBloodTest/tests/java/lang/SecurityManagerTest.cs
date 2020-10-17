using NUnit.Framework;

using java = biz.ritter.javapi;

namespace vampire.test.lang
{
    public class UnitTest_SecurityManager
    {
        
        [SetUp]
        public void SetUp () {
        }
    
        [Test]
        public void Test_SetSecurityManager()
        {
            java.lang.SecurityManager newSecurityManager = new java.lang.SecurityManager();
            
            this.help_SetSecurityManager (newSecurityManager);
            Assert.AreSame (newSecurityManager, java.lang.SystemJ.getSecurityManager(), "Object is not the same");
        }
        
        
        [Test]
        public void Test_SetSecurityManagerNotAllowdByProperty()
        {
            // can not set, in result of property
            java.lang.SystemJ.setProperty ("java.security.manager", "disallow");
            Assert.Throws<java.lang.UnsupportedOperationException>( 
              delegate {
                help_SetSecurityManager(new java.lang.SecurityManager());
              }
            );
            java.lang.SystemJ.setProperty ("java.security.manager", "allow");
        }
        
        internal void help_SetSecurityManager (java.lang.SecurityManager newSecurityManager) {
            java.lang.SystemJ.setSecurityManager (newSecurityManager);
        }
    }
}
