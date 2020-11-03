using NUnit.Framework;

using System;
using java = biz.ritter.javapi;

namespace vampire.test.dotnet
{

    /// simple subclass test object
    public class XArrayList<E> : java.util.ArrayList<E> {
    }

    /// <summary>
    /// This test throws compiler error if not supported.
    /// So last call is Assert.True(true);
    /// </summary>
    public class UnitTest_ForEach
    {
        
        [SetUp]
        public void SetUp () {	
        }

        [Test]
        public void Test_ForEach_subclass () {
          java.lang.SystemJ.outJ.println("subclass foreach test");
        
          java.util.ArrayList<String> testArray = new XArrayList<String>();
          testArray.add("VampireAPI");
          testArray.add("supports");
          testArray.add("foreach statements");
          testArray.add("! Yeah!!!");
          
          // for count loop
          java.lang.SystemJ.outJ.println("for count loop: ");
          for (int i = 0; i < testArray.size(); i++) {
            java.lang.SystemJ.outJ.print(testArray.get(i));
            java.lang.SystemJ.outJ.print(" ");
          }
          java.lang.SystemJ.outJ.println ();
          
          // for iterator loop
          java.lang.SystemJ.outJ.println("for iterator loop: ");
          for (java.util.Iterator<String> it = testArray.iterator() ; it.hasNext();) {
            java.lang.SystemJ.outJ.print(it.next());
            java.lang.SystemJ.outJ.print(" ");
          }
          java.lang.SystemJ.outJ.println ();
          
          // foreach loop
          java.lang.SystemJ.outJ.println("foreach loop: ");
          foreach (String text in testArray) {
            java.lang.SystemJ.outJ.print(text);
            java.lang.SystemJ.outJ.print(" ");
          }
          java.lang.SystemJ.outJ.println ();
          
          
          Assert.True (true,"compiler OK");
        }
    
        [Test]
        public void Test_ForEach_LinkedList () {
          java.lang.SystemJ.outJ.println("LinkedList foreach test");
        
        
          java.util.LinkedList<String> testArray = new java.util.LinkedList<String>();
          testArray.add("VampireAPI");
          testArray.add("supports");
          testArray.add("foreach statements");
          testArray.add("! Yeah!!!");
          
          // for count loop
          java.lang.SystemJ.outJ.println("for count loop: ");
          for (int i = 0; i < testArray.size(); i++) {
            java.lang.SystemJ.outJ.print(testArray.get(i));
            java.lang.SystemJ.outJ.print(" ");
          }
          java.lang.SystemJ.outJ.println ();
          
          // for iterator loop
          java.lang.SystemJ.outJ.println("for iterator loop: ");
          for (java.util.Iterator<String> it = testArray.iterator() ; it.hasNext();) {
            java.lang.SystemJ.outJ.print(it.next());
            java.lang.SystemJ.outJ.print(" ");
          }
          java.lang.SystemJ.outJ.println ();
          
          // foreach loop
          java.lang.SystemJ.outJ.println("foreach loop: ");
          foreach (String text in testArray) {
            java.lang.SystemJ.outJ.print(text);
            java.lang.SystemJ.outJ.print(" ");
          }
          java.lang.SystemJ.outJ.println ();
          
          
          Assert.True (true,"compiler OK");
        }
        
        
        [Test]
        public void Test_ForEach_ArrayList () {
          java.lang.SystemJ.outJ.println("ArrayList foreach test");
        
        
          java.util.ArrayList<String> testArray = new java.util.ArrayList<String>();
          testArray.add("VampireAPI");
          testArray.add("supports");
          testArray.add("foreach statements");
          testArray.add("! Yeah!!!");
          
          // for count loop
          java.lang.SystemJ.outJ.println("for count loop: ");
          for (int i = 0; i < testArray.size(); i++) {
            java.lang.SystemJ.outJ.print(testArray.get(i));
            java.lang.SystemJ.outJ.print(" ");
          }
          java.lang.SystemJ.outJ.println ();
          
          // for iterator loop
          java.lang.SystemJ.outJ.println("for iterator loop: ");
          for (java.util.Iterator<String> it = testArray.iterator() ; it.hasNext();) {
            java.lang.SystemJ.outJ.print(it.next());
            java.lang.SystemJ.outJ.print(" ");
          }
          java.lang.SystemJ.outJ.println ();
          
          // foreach loop
          java.lang.SystemJ.outJ.println("foreach loop: ");
          foreach (String text in testArray) {
            java.lang.SystemJ.outJ.print(text);
            java.lang.SystemJ.outJ.print(" ");
          }
          java.lang.SystemJ.outJ.println ();
          
          
          Assert.True (true,"compiler OK");
        }

        [Test]
        public void Test_ForEach_Vector () {
          java.lang.SystemJ.outJ.println("Vector foreach test");
        
        
          java.util.Vector<String> testArray = new java.util.Vector<String>();
          testArray.add("VampireAPI");
          testArray.add("supports");
          testArray.add("foreach statements");
          testArray.add("! Yeah!!!");
          
          // for count loop
          java.lang.SystemJ.outJ.println("for count loop: ");
          for (int i = 0; i < testArray.size(); i++) {
            java.lang.SystemJ.outJ.print(testArray.get(i));
            java.lang.SystemJ.outJ.print(" ");
          }
          java.lang.SystemJ.outJ.println ();
          
          // for iterator loop
          java.lang.SystemJ.outJ.println("for iterator loop: ");
          for (java.util.Iterator<String> it = testArray.iterator() ; it.hasNext();) {
            java.lang.SystemJ.outJ.print(it.next());
            java.lang.SystemJ.outJ.print(" ");
          }
          java.lang.SystemJ.outJ.println ();
          
          // foreach loop
          java.lang.SystemJ.outJ.println("foreach loop: ");
          foreach (String text in testArray) {
            java.lang.SystemJ.outJ.print(text);
            java.lang.SystemJ.outJ.print(" ");
          }
          java.lang.SystemJ.outJ.println ();
          
          
          Assert.True (true,"compiler OK");
        }    

/*
        [Test]
        public void Test_ForEach_HashSet () {
          java.lang.SystemJ.outJ.println("Vector foreach test");
        
        
          java.util.HashSet<String> testArray = new java.util.HashSet<String>();
          testArray.add("VampireAPI");
          testArray.add("supports");
          testArray.add("foreach statements");
          testArray.add("! Yeah!!!");
                    
          // for iterator loop
          java.lang.SystemJ.outJ.println("for iterator loop: ");
          for (java.util.Iterator<String> it = testArray.iterator() ; it.hasNext();) {
            java.lang.SystemJ.outJ.print(it.next());
            java.lang.SystemJ.outJ.print(" ");
          }
          java.lang.SystemJ.outJ.println ();
          
          // foreach loop
          java.lang.SystemJ.outJ.println("foreach loop: ");
          foreach (String text in testArray) {
            java.lang.SystemJ.outJ.print(text);
            java.lang.SystemJ.outJ.print(" ");
          }
          java.lang.SystemJ.outJ.println ();
          
          
          Assert.True (false,"compiler error");
        }    
*/

    }
}
