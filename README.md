# Sets Library

![C#](https://img.shields.io/badge/language-C%23-blue.svg)
![.NET 8](https://img.shields.io/badge/.NET-8.0-blueviolet.svg)
[![License](https://img.shields.io/github/license/Shisui-Pho/Sets-Library)](https://github.com/Shisui-Pho/Sets-Library/blob/main/LICENSE)

**Sets Library** is a comprehensive and efficient framework for managing mathematical set operations. It supports advanced features such as nested subsets, dynamic collections, and custom data types, providing developers with the tools to work with structured data efficiently.

---

## Features Overview

| Feature                           | Description                                                                                         |
|-----------------------------------|-----------------------------------------------------------------------------------------------------|
| **Structured Sets**               | Create and manipulate sets with support for nesting and complex operations.                        |
| **Custom Data Types**             | Use custom types that implement `IComparable` for greater flexibility.                            |
| **Dynamic Collections**           | Manage sets dynamically with unique, human-readable naming conventions.                           |
| **Advanced Set Operations**       | Perform intersection, union, complement, symmetric difference, and more.                          |
| **Error Handling**                | Includes robust custom exceptions with detailed error messages.                                   |
| **Extensible Design**             | Easily extend functionality using interfaces and abstract classes.                                |

---
## Why Use Sets Library?

The Sets Library is an indispensable tool for developers and researchers working with structured data, mathematical modeling, or data analysis. It provides:

- **Ease of Use**: Streamlined APIs for handling sets and their operations without worrying about low-level implementation details.
- **Powerful Abstractions**: Handles complex relationships like nested subsets, making it ideal for advanced data structures.
- **Flexibility**: Supports custom data types and operations, enabling diverse use cases in various domains like machine learning, graph theory, and database systems.
- **Scalability**: Efficient algorithms and structures for managing large collections of sets and operations.

Whether you're building educational tools, solving complex data problems, or modeling mathematical systems, this library empowers you to focus on the logic without worrying about implementation nuances.

---
## Namespace Overview

The library is modularized for clarity and scalability. The namespaces group functionalities logically to ensure maintainable and understandable code.

### **SetsLibrary**
The core namespace for interfaces and classes handling structured sets and operations.

| Component               | Description                                                                          |
|-------------------------|--------------------------------------------------------------------------------------|
| `ICustomObjectConverter`| Interface for converting custom objects into structured set elements.                |
| `ISetTree`              | Interface representing nested sets in a tree-like structure.                         |
| `IStructuredSet`        | Interface for structured sets, supporting advanced set operations.                   |
| `SetResultType`         | Enum defining relationships like subset and proper set.                              |
| `BaseSet`               | Abstract class with foundational methods for set manipulation.                       |
| `TypedSet`              | Generic implementation for strongly-typed sets.                                      |
| `StringLiteralSet`      | Specialized implementation for sets of string literals.                              |
| `CustomObjectSet`       | Designed for sets of user-defined objects.                                           |
| `SetTree`               | Core class for managing tree-like set structures.                                    |
| `SetTreeBaseWrapper`    | Base wrapper for delegating `SetTree` operations.                                    |
| `SetTreeWrapper`        | Advanced wrapper adding indexed access to `SetTree`.                                 |

---

### **SetsLibrary.Collections**
Handles collections of structured sets with scalability and flexibility in mind.

| Component               | Description                                                                          |
|-------------------------|--------------------------------------------------------------------------------------|
| `ISetCollection`        | Interface for managing collections of sets.                                          |
| `SetCollection`         | Implementation for dynamic collections with Excel-like naming conventions.           |
| `BaseSortedCollection`  | Base class for sorted collections of set elements.                                   |
| `SortedElements`        | Handles sorted elements within collections.                                          |
| `SortedSubSets`         | Manages sorted subsets within collections.                                           |

---

### **SetsLibrary.Utilities**
Contains utility classes for set parsing, validation, and extraction.

| Component               | Description                                                                          |
|-------------------------|--------------------------------------------------------------------------------------|
| `BraceEvaluator`        | Validates braces in set expressions.                                                 |
| `SetTreeExtractor`      | Parses and constructs sets from string representations.                              |
| `SetTreeUtility`        | Provides helper methods for working with `SetTree` structures.                       |

---

### **SetsLibrary.SetOperations**
A new namespace providing advanced set operations for `IStructuredSet<T>` objects.

| Operation                 | Description                                                                          |
|---------------------------|--------------------------------------------------------------------------------------|
| `IntersectWith`           | Computes the intersection of two sets.                                               |
| `UnionWith`               | Combines two sets into one, retaining unique elements.                               |
| `Complement`              | Computes the complement of a set with respect to a universal set.                    |
| `Difference`              | Returns elements in one set that are not in another.                                 |
| `SymmetricDifference`     | Returns elements in either set but not both.                                         |
| `CartesianProduct`        | Generates all ordered pairs of elements from two sets (not yet implemented).         |
| `IsDisjoint`              | Checks if two sets have no common elements.                                          |
| `SetStructuresEqual`      | Compares the structures of two sets for equality.                                    |

---
## Example Usage

This section demonstrates how to leverage the Sets Library to perform common set operations and manage collections of structured sets.

### Basic Set Creation
The `BaseSet<T>` class and its derived classes (`TypedSet`, `StringLiteralSet`, etc.) allow you to define and manipulate sets.

```
using SetsLibrary;

var config = new SetExtractionConfiguration<int>(',');

// Creating a set from a string expression
var setA = new TypedSet<int>("{1,2,3}", config);

// Accessing set properties
Console.WriteLine($"Set Cardinality: {setA.Cardinality}");
Console.WriteLine($"Set Elements: {setA.BuildStringRepresentation()}");
```

### Performing Set Operations
Use `SetsOperations` to perform union, intersection, and other operations between sets.

```
using SetsLibrary.SetOperations;

var setB = new TypedSet<int>("{3,4,5}", config);

// Union
var unionSet = setA.UnionWith(setB);
Console.WriteLine($"Union: {unionSet.BuildStringRepresentation()}");

// Intersection
var intersectionSet = setA.IntersectWith(setB);
Console.WriteLine($"Intersection: {intersectionSet.BuildStringRepresentation()}");
```

### Working with Set Collections
The `SetCollection<T>` class allows you to manage multiple sets efficiently.

```
using SetsLibrary;
using SetsLibrary.Collections;


var collection = new SetCollection<int>();

// Adding sets to the collection
collection.Add(setA);
collection.Add(setB);

// Retrieving sets by name
//The names follow the EXCEL column naming, ALL CAPS!!
var retrievedSet = collection["A"];
Console.WriteLine($"Retrieved Set: {retrievedSet.BuildStringRepresentation()}");
```

### Debugging with Custom Exceptions
The library provides custom exceptions for better debugging.

```
try
{
    // Attempt an invalid operation
    var invalidSet = setA.IntersectWith(null);
}
catch (SetsException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine($"Details: {ex.Details}");
}
```
