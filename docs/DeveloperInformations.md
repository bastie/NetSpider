# Developer informations

## Other implementations as Java
Some things need to replace
     
    java.lang.System => java.lang.SystemJ, in result of System is a namespace in C#
    java.lang.System.out => java.lang.SystemJ.outJ, because out is a keyword in C#

C# provides `const` and `readonly` keyword. `readonly`is more like Java `final` keyword
for constants. `readonly` constants can set in constructors.
     
## How to release

### Types
Inheritance is nearly same with different syntax.

Java:

```java
public final class Knight extends Farmer implements Nameable, Payable {}
public interface Payable extends Motivable {}
```

C#:

```csharp
public sealed class Knight : Famer, Nameable, Payable {}
public interface Payable: Motivable {}
```

### Constructors
Not like Java the super class constructors in C# are called over declaration not implementation.

Java:

```java
class Wrench extends Tool {
  public Wrench (String material) {
    super (material);
  }
}
```

C#:

```csharp
class Wrench : Tool {
  public Wrench (String material) : base (material) {}
}
```

### Constants
C# provides `const` and `readonly` keyword. `readonly` is more like Java `final` keyword
for constants. `readonly` constants can set in constructors.



### Enums
Take a look at solutions:
 * reimplementation like Java [EnumCollections](https://github.com/matteckert/EnumCollections) 
 * use [extension methods](https://weyprecht.de/2019/10/16/enums-in-csharp-and-java/)


### VampireApi
 * in ==> inJ
 * out ==> outJ
 * package java.lang.ref => java.lang.refj
 * System class ==> SystemJ
 * add at beginning using System; and using java = biz.ritter.javapi;
 * extends java.io.Serializable ==> : java.io.Serializable and (!!!) using System and [Serializable] for type and all(!) subtypes
 * Variable name operator ==> operatorJ
 * Variable name string ==> stringJ
 * Variable name params ==> paramsJ
 * Variable name object ==> objectJ
 * java.util.Map<?,?>.Entry<?,?> ==> java.util.MapNS.Entry<Object,Object>
 
 
### Unsorted informations
Keyword replacing:
 * import package.package.*; => using package.package;
 * usings are on "package"-stage, never import a type with using
 * package => namespace
 * namespace is a block not a statement
 * namespace after using
 * extends ==> :
 * implements ==> : or ,
 * static block ==> static constructor
 * boolean ==> bool
 * final class ==> sealed class 
 * final method ==> sealed method 
 * final var ==> readonly var    or sometime const
 * method throws signature ==> comment out or remove
 * type name [] ==> type [] name
 * important: you need to using System for basic types like String
 * java.io.Serializable as marker Interface ist replacing from [Serializable] for all (sub)types
 * transient ==> using System and [NonSerializable]
 * instanceof ==> is
 * synchronized method ==> lock(this) - maybe [MethodImpl(MethodImplOptions.Synchronized)]
 * synchronized block with type ==> replace with lock
 * synchronized block without type ==> create readonly object and replace synchronized with lock (object)
 * array.length ==> array.Length
 * base class constructor call super (xyz) is declared with : base (xyz)
 * non final methods in non final class needed to be virtual
 * methods are "final" by default, need to be virtual or abstract if not
 * visibility are littlebit different and you need more internal and public 
 * if you override a method same visiblity are important
 * if you override a method with return value, the visible of return type need to be same or more
 * @Override ==> do not use override keyword for interface methods
 * do not call variable name same as method name
 * switch default need a break
 * java.lang.Boolean.FALSE (false) != System.Boolean.FalseString - add a .ToLower()
 * generic classes needed types, like Object

### Streams

Java Streams to C# LINQ see https://blog.lahteenmaki.net/java-streams-vs-c-linq-vs-java6.html 

### Class
the class problems...
return Class<?> ==> return Type
MyClass.class ==> typeof(MyClass)
from 

   ```java
   Class<?> refClass = refChildNode.getClass();
   Class<?> testClass = testChildNode.getClass();
   if (!refClass.equals(testClass)) {
      
   }
   ```

to

   ```c#
    Type refClass = refChildNode.GetType();
    Type testClass = testChildNode.GetType();
    if (!refClass.Equals(testClass)) 
    {
    }
   ```
   
   
#### Inner anonymous classes
Create a non-anonymous inner class and instantiate this instead. This example are from Apache Derby database:

from

   ```java
   private static ContextService getContextService() {
     return AccessController.doPrivileged(
       new PrivilegedAction<ContextService>(){ // => extends new Inner class extends this type 
         public ContextService run(){ // => copy method in new class
           return ContextService.getFactory();
         }
       }
     );
   }
   ```
   
to
   
   ```c#
   private static ContextService getContextService() {
     return AccessController.doPrivileged(
       new IAC_PrivilegedAction_getContextService() // <= call new instance of new class instead inner anonymous class
     );
   }
   class IAC_PrivilegedAction_getContextService : PrivilegedAction<ContextService> { // <= extends new Inner class extends this type
     public ContextService run(){ // <= copy method in new class
       return ContextService.getFactory(); 
     }
   }
   ```

If from inner anonymous class outer type references, types or methods are called then
add needed types to constructor in new classes and add save this in the new class as references.

   
### catch exception

#### unused exception var
Unused parameter name in Java like ``ignored`` should be removed. And so on from

   ```java
   try { omg(); } catch (Exception ignored){}
   ```
   
to

   ```c#
   try { omg(); } catch (Exception){}
   ```

#### multiple exceptions

from

   ```java
   try {
     doSomethingStupidWithCoffee();
   }
   catch (UnsupportedHumanException | NotReallyException ex) {
     deinstallMySelf();
   }
   ```
   
to

   ```c#
     try{
       doSomethindStupidWithSharp(); 
     }
     catch (Exception ex) when (
       ex is UnsupportedHumanException ||
       ex is NotRellyException
     ){
       deinstallWorld();
     }
   ```

### the unsigned right shift operator
from

   ```java
     result = var1 >>> var2
   ```
   
to
 
   ```c#
     result = (int)((uint)var1 >> var2)
   ```

### regular expressions
   * take a look at https://www.generacodice.com/en/articolo/484294/What-is-the-C%23-equivalent-of-java.util.regex 
    
## Development hacks

    # check version in VampireApi using projects
    tail -c 1740 bin/Debug/netcoreapp3.1/NetVampiro.dll 
    
    # replace version in VampireApi using project (build,copy,run)
    dotnet build
    cp NetVampiro/bin/Debug/netcoreapp3.1/NetVampiro.* ./bin/Debug/netcoreapp3.1
    dotnet run --no-build
    
    
