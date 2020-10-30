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
        public void Test_FileAbsolute()
        {
          java.io.File absolute = new java.io.File ("/VampireApi/extends/JavaApi.nice");
          Assert.True (absolute.isAbsolute(),"File "+absolute.toString()+" need to be absolute File");
        }

    }
}