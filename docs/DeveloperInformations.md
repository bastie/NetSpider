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
    

    
## Development hacks

    # check version in VampireApi using projects
    tail -c 1740 bin/Debug/netcoreapp3.1/NetVampiro.dll 
    
    # replace version in VampireApi using project (build,copy,run)
    dotnet build
    cp NetVampiro/bin/Debug/netcoreapp3.1/NetVampiro.* ./bin/Debug/netcoreapp3.1
    dotnet run --no-build
    
    