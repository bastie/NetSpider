using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.io
{
    [TestClass]
    public class FileReaderTest
    {
        [TestMethod]
        public void readFile()
        {
            // Step 1 - file greater than FileBuffer
            java.io.File file = new java.io.File("/Develop/Projekte/Kobol/ITIL RPC/src/biz/ritter/dbms/views/AufgabenUebersichtView.java");
            java.io.BufferedReader br = new java.io.BufferedReader(new java.io.FileReader(file));
            while (br.ready())
            {
                char c = (char)br.read();
                java.lang.SystemJ.outJ.print(c);
            }
            // Step 2 - file smaller than FileBuffer
            file = new java.io.File("/Develop/Projekte/Kobol/ITIL RPC/src/biz/ritter/dbms/ItilActivator.java");
            br = new java.io.BufferedReader(new java.io.FileReader(file));
            while (br.ready())
            {
                char c = (char)br.read();
                java.lang.SystemJ.outJ.print(c);
            }
        }

        static void Main()
        {
            FileReaderTest test = new FileReaderTest();
            test.readFile();
        }
    }
}
