using NUnit.Framework;

using System;
using java = biz.ritter.javapi;

namespace vampire.test.io
{
    public class UnitTest_IoFile
    {
        
        [SetUp]
        public void SetUp () {	
        }

        [Test]
        public void Test_WriteSimpleFile () {
            String name = java.lang.SystemJ.getProperty("java.io.tmpdir")+"VampireApi4U.html";
            String content = "<html><body><h1>Just Another Vampire Api 4 .net, aka Java4.net</h1></body></html>";
            byte[] value = System.Text.Encoding.GetEncoding("utf-8").GetBytes(content);

            java.io.FileOutputStream fos = new java.io.FileOutputStream(name);
            fos.write (value,0,value.Length);
            fos.flush();
            fos.close();
            
            Assert.False (new java.io.File (java.lang.SystemJ.getProperty("java.io.tmpdir")+"not.exist").exists(),"java.io.File.exists() found not existing file");
            Assert.True (new java.io.File (name).exists(),"write simple file error");
            Assert.AreEqual (81, new java.io.File(name).length(),"content size of file unexpected");
        }
    
        [Test]
        public void Test_FileAbsolute()
        {
          java.io.File absolute = new java.io.File ("/VampireApi/extends/JavaApi.nice");
          Assert.True (absolute.isAbsolute(),"File "+absolute.toString()+" need to be absolute File");
        }

        [Test]
        public void Test_FileHidden() 
        {
          switch (System.Environment.OSVersion.Platform)
          {
            case PlatformID.Unix:
            case PlatformID.MacOSX:
                String name = java.lang.SystemJ.getProperty("java.io.tmpdir")+".VampireApi.secret";

                java.io.File hidden = new java.io.File (name); 
                if (!hidden.exists()) {
		            String content = "secret=geheimnis";
		            byte[] value = System.Text.Encoding.GetEncoding("utf-8").GetBytes(content);
		
                    java.io.FileOutputStream fos = new java.io.FileOutputStream(name);
		            fos.write (value,0,value.Length);
		            fos.flush();
		            fos.close();
                }
                Assert.True (new java.io.File (name).exists(),"java.io.File.exists() found not file "+name);
                Assert.True (hidden.isHidden(), "File "+hidden.toString()+" should be hidden");
                break;
            default :
                Assert.Warn ("Test_FileHidden not implemented for this platform");
                break;
          }
            
        }

    }
}
