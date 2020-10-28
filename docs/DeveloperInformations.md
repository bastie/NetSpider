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
    
### Unsorted informations
Keyword replacing:
in ==> inJ
out ==> outJ
package => namespace
package java.lang.ref => java.lang.refj
import package.package.*; => package.package;
extends ==> :
implements ==> : or ,
static block ==> static constructor
boolean ==> bool
final class ==> sealed class 
final method ==> sealed method 
final var ==> readonly var    or sometime const
method throws signature ==> comment out or remove
System class ==> SystemJ
type name [] ==> type [] name
namespace is a block not a statement
add at beginning using System; and using java = biz.ritter.javapi;
important: you need to using System for basic types like String
namespace after using
extends java.io.Serializable ==> : java.io.Serializable and (!!!) using System and [Serializable] for type and all(!) subtypes
transient ==> using System and [NonSerializable]
instanceof ==> is
synchronized method ==> lock(this) - maybe [MethodImpl(MethodImplOptions.Synchronized)]
synchronized block with type ==> replace with lock
synchronized block ==> create readonly object and replace synchronized with lock (object)
Variable name operator ==> operatorJ
Variable name string ==> stringJ
Variable name params ==> paramsJ
Variable name object ==> objectJ
array.length ==> array.Length
java.util.Map<?,?>.Entry<?,?> ==> java.util.MapNS.Entry<Object,Object>
base class constructor call super (xyz) is declared with : base (xyz)
non final methods in non final class needed to be virtual
methods are "final" by default, need to be virtual or abstract if not
usings are on "package"-stage, never import a type with using
if you override a method same visiblity are important
if you override a method with return value, the visible of return type need to be same or more
do not call variable name same as method name
switch default need a break
java.lang.Boolean.FALSE (false) != System.Boolean.FalseString - add a .ToLower()

genric classes needed types 


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
   



    
## Development hacks

    # check version in VampireApi using projects
    tail -c 1740 bin/Debug/netcoreapp3.1/NetVampiro.dll 
    
    # replace version in VampireApi using project (build,copy,run)
    dotnet build
    cp NetVampiro/bin/Debug/netcoreapp3.1/NetVampiro.* ./bin/Debug/netcoreapp3.1
    dotnet run --no-build
    
    
