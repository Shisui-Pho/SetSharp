<h1 align="center">Sets Library</h1>
<div align="center">
  <img src="https://img.shields.io/badge/language-C%23-blue.svg" alt="C#" />
  <img src="https://img.shields.io/badge/.NET-8.0-blueviolet.svg" alt=".NET 8" />
  <a href="https://github.com/Shisui-Pho/Sets-Library/blob/main/LICENSE">
    <img src="https://img.shields.io/github/license/Shisui-Pho/Sets-Library" alt="License" />
  </a>
</div>
<br>
<strong>SetsLibrary</strong> is a framework which is designed to dynamically convert a raw string into a set object using various configurations, like element seperators(row terminators) and field-terminators if you wish to read it as an object set. It supports advanced features such as nested subsets, dynamic collections, and custom data types, providing developers with the tools to work with structured data efficiently.
<br>


## Features Overview
The library provides the following features, of course you can also extend some of these to suite your needs. The library just gives you a solid foundation for you to build your mension.

| Feature                           |                                                                                          |
|-----------------------------------|-----------------------------------------------------------------------------------------------------|
| **Structured Sets**               | Create and manipulate sets with support for nesting and complex operations.                        |
| **Custom Data Types**             | Use custom types that implement IComparable for greater flexibility.                               |
| **Dynamic Collections**           | Manage sets dynamically with unique, human-readable naming conventions.                           |
| **Advanced Set Operations**       | Perform intersection, union, complement, symmetric difference, and more.                          |
| **Error Handling**                | Includes robust custom exceptions with detailed error messages.                                   |
| **Extensible Design**             | Easily extend functionality using interfaces and abstract classes.                                |
---

### **SetsLibrary**
These are the core components of the library. Please note that the the sets library relies on `IComparabile<T>` interface to perform it's advance procedures. 

| Component               | Description                                                                          |
|-------------------------|--------------------------------------------------------------------------------------|
| `BaseSet`               | An abstract class with foundational methods for set manipulation. `CustomObjectSet`, `StringLiteralSet` and `TypedSet` inherit from this class. |
 `TypedSet`              | Generic implementation for strongly-typed sets. This is mearnt for all objects that implement `IConvertible` like premitive data types such as `int`, `float`, etc |
| `StringLiteralSet`      | Specialized implementation for sets of string literals. This just read the string as it is with no convesion, but it is validated against structure |
| `CustomObjectSet`       | Designed for sets of user-defined objects and the type of object passed must be of type `ICustomObjectConverter`|
| `ICustomObjectConverter` | Interface for converting custom objects into structured set elements, can be used when you need to use `CustomObjectSet`                |
| `SetTree`              | The core data structure of the system. It represents nested sets in a tree-like structure and is wrapped by various classes to provide extra functionalities. `BaseSet` also wrapps this class(or the interface that inherit from the same interface as `SetTree`)|
|`SetsConfigurations`| A configuration class that will allow you to configure how a string is extracted into a set and how it's rebuilt again|
| `IStructuredSet`        | Interface for structured sets, supporting advanced set operations.                   |
|`SetCollection`| A collections class that acts as table where each Structured set added is given a unique values that will identify it. This follows the mathematical and excel column naming (since sets are often represented by uppercase letters), e.g, A, B, C,...,XDF,..|
|`SortedCollection`|A collections class which stores the elements in a sorted order. It has a method to only add an element if it is unique.|

---
## Name spaces break down  
The project contains four main namespaces, namely
- `SetsLibrary` - Contains all the objects that are related to sets.
- `SetsLibrary.Collections` - Has all the different types of collections that form part of the sets structures.
- `SetsLibrary.Operations` - Contains method related to sets operations like `UinionWith`, `IntersectWith`, `SymmetricDifference`, etc.  
- `SetsLibrary.Extensions` - Under this namespace you will find all the extension methods related to converting to other data types, from Sets structures to enumerables and vice versa.  

---

## Example Usage

### Basic Set Creation

**TypedSet**
```csharp
using SetsLibrary;

//Set the configurations
var config = new SetsConfigurations(","); //Elements are seperated by commas

//Define the expression
string exp1 = "{1,2,3,1,1,2,3}";
string exp2 = "{{1,2,1,2,1,2}}";
string exp3 = "{1,2,{1,3,1,{2}},2,3,2,1,{8,8}}";
string exp4 = "{1,2,{8,6},3,{6,8}}";

//Create sets
var setA = new TypedSet<int>(exp1, config);
var setB = new TypedSet<int>(exp2, config);
var setC = new TypedSet<int>(exp3, config);
var setD = new TypedSet<int>(exp4, config);

//Display results
Console.WriteLine($"|A| = {setA.Cardinality}, {setA.BuildStringRepresentation()}");// |A| = 3, {1,2,3}
Console.WriteLine($"|B| = {setB.Cardinality}, {setB.BuildStringRepresentation()}");// |B| = 1, {{1,2}}
Console.WriteLine($"|C| = {setC.Cardinality}, {setC.BuildStringRepresentation()}");// |C| = 5, {1,2,3,{8},{1,3,{2}}}
Console.WriteLine($"|D| = {setD.Cardinality}, {setD.BuildStringRepresentation()}");// |D| = 4, {1,2,3,{6,8}}
```
**Working with Set Collections**  
The `SetCollection<T>` class allows you to manage multiple sets efficiently.

```csharp
using SetsLibrary;
using SetsLibrary.Collections;


var collection = new SetCollection<int>();

// Adding sets to the collection
collection.Add(setA);
collection.Add(setB);

// Retrieving sets by name
//The names follow the EXCEL column naming, ALL CAPS!!
var retrievedSet = collection["A"];
Console.WriteLine($"Retrieved Set: {retrievedSet.BuildStringRepresentation()}");//{1,2,3}
``` 
**Custom Data Type Integration**
The library allows defining and working with custom data types by implementing `ICustomObjectConverter<T>`.

```csharp
using SetsLibrary;

class Person : ICustomObjectConverter<Person>, IComparable<Person>
{
    public string? Name { get; set; }
    public int Age { get; set; }

    public static Person ToObject(string?[] fields)
    {
        //Here you can do custom error handling to make sure the fields are in the way you like
       //.....
        Person p = new()
        {
            Name = fields[0] ?? string.Empty,
            Age = int.Parse(fields[1] ?? "0")
        };
        return p;
    }
    public int CompareTo(Person? other)
    {
        return other?.Name?.CompareTo(Name) ?? 0;
    }//CompareTo

    public override string ToString() => Name + " " + " " + Age;
}//

//- Row-terminator : "\n"
//-Field terminator: ","
string expression = "{Phiwo,10\nAbel,13\nAbel,13\nAlice,1\nPhiwo,10}";

//Set configuration
var config = new SetsConfigurations(",", "\n");

var set = new CustomObjectSet<Person>(expression,config);

Console.WriteLine($"Element count: {set.Cardinality}");// 3
Console.WriteLine($"Expression: {set.BuildStringRepresentation()}");//{// Abel 13\nAlice 1\nPhiwo 10}
```

## **License**

This project is licensed under the MIT License.  
You are free to use, modify, and distribute this library in your projects. See the [LICENSE](https://github.com/Shisui-Pho/Sets-Library/blob/main/LICENSE) file for details.

---

Thank you for using the Sets Library!  
For questions or feedback, please open an issue or contact us through the [repository](https://github.com/Shisui-Pho/Sets-Library).


