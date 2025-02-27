<h1 align="center">Sets Library</h1>
<div align="center">
  <img src="https://img.shields.io/badge/language-C%23-blue.svg" alt="C#" />
  <img src="https://img.shields.io/badge/.NET-8.0-blueviolet.svg" alt=".NET 8" />
  <a href="https://github.com/Shisui-Pho/Sets-Library/blob/main/LICENSE">
    <img src="https://img.shields.io/github/license/Shisui-Pho/Sets-Library" alt="License" />
  </a>
</div>
<br>
<p align="center">
  <strong>Sets Library</strong> is a comprehensive and efficient framework for managing mathematical set operations. It supports advanced features such as nested subsets, dynamic collections, and custom data types, providing developers with the tools to work with structured data efficiently.
</p>
<br>

---

## Features Overview

| Feature                           | Description                                                                                         |
|-----------------------------------|-----------------------------------------------------------------------------------------------------|
| **Structured Sets**               | Create and manipulate sets with support for nesting and complex operations.                        |
| **Custom Data Types**             | Use custom types that implement IComparable for greater flexibility.                               |
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
| `ICustomObjectConverter` | Interface for converting custom objects into structured set elements.                |
| `ISetTree`              | Interface representing nested sets in a tree-like structure.                         |
| `IStructuredSet`        | Interface for structured sets, supporting advanced set operations.                   |
| `SetResultType`         | Enum defining relationships like subset and proper set.                              |
| `BaseSet`               | Abstract class with foundational methods for set manipulation.                       |
| `TypedSet`              | Generic implementation for strongly-typed sets.                                      |
| `StringLiteralSet`      | Specialized implementation for sets of string literals.                              |
| `CustomObjectSet`       | Designed for sets of user-defined objects.                                           |
| `SetTree`               | Core class for managing tree-like set structures.                                    |
| `SetTreeBaseWrapper`    | Base wrapper for delegating SetTree operations.                                      |
| `SetTreeWrapper`        | Advanced wrapper adding indexed access to SetTree.                                  |

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
| `SetTreeUtility`        | Provides helper methods for working with SetTree structures.                       |

---

### **SetsLibrary.SetOperations**
A new namespace providing advanced set operations for `IStructuredSet<T>` objects.

| Operation               | Description                                                                          |
|-------------------------|--------------------------------------------------------------------------------------|
| `IntersectWith`         | Computes the intersection of two sets.                                               |
| `UnionWith`             | Combines two sets into one, retaining unique elements.                               |
| `Complement`            | Computes the complement of a set with respect to a universal set.                    |
| `Difference`            | Returns elements in one set that are not in another.                                 |
| `SymmetricDifference`   | Returns elements in either set but not both.                                         |
| `CartesianProduct`      | Generates all ordered pairs of elements from two sets (not yet implemented).         |
| `IsDisjoint`            | Checks if two sets have no common elements.                                          |
| `SetStructuresEqual`    | Compares the structures of two sets for equality.                                    |

---

## Example Usage

This section demonstrates how to leverage the Sets Library to perform common set operations and manage collections of structured sets.

### Basic Set Creation
The `BaseSet<T>` class and its derived classes (e.g., `TypedSet`, `StringLiteralSet`) allow you to define and manipulate sets.

```csharp
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

```csharp
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
Console.WriteLine($"Retrieved Set: {retrievedSet.BuildStringRepresentation()}");
```

### Debugging with Custom Exceptions
The library provides custom exceptions for better debugging.

```csharp
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

## Advanced Usage

This section explores advanced functionalities of the Sets Library, such as nested subsets, custom object integration, and efficient set collection management.

### Handling Nested Subsets
The Sets Library supports hierarchical structures where sets can contain other sets as subsets.

```csharp
using SetsLibrary;

var config = new SetExtractionConfiguration<int>(",", "\n");
var nestedSet = new TypedSet<int>("{1, 2, {3, 4, {5}}}", config);

Console.WriteLine($"Nested Set Representation: {nestedSet.BuildStringRepresentation()}");

// Enumerate subsets
foreach (var subset in nestedSet.EnumerateSubsets())
{
    Console.WriteLine($"Subset: {subset.BuildStringRepresentation()}");
}
```

### Custom Data Type Integration
The library allows defining and working with custom data types by implementing `ICustomObjectConverter<T>`.

```csharp
using SetsLibrary;

class CustomType : IComparable<CustomType>, ICustomObjectConverter<CustomType>
{
    public string Name { get; set; }
    public int Id { get; set; }

    public int CompareTo(CustomType? other) => Id.CompareTo(other?.Id ?? 0);

    public static CustomType ToObject(string field, SetExtractionConfiguration<CustomType> settings)
    {
        var parts = field.Split(settings.FieldTerminator);
        return new CustomType
        {
            Name = parts[0],
            Id = int.Parse(parts[1])
        };
    }

    public override string ToString() => $"{Name} ({Id})";
}

var customConfig = new SetExtractionConfiguration<CustomType>(";", "\n");
var customSet = new CustomObjectSet<CustomType>("{John;1, Jane;2}", customConfig);

Console.WriteLine($"Custom Set: {customSet.BuildStringRepresentation()}");
```

### Managing Large Collections with `SetCollection`
The `SetCollection<T>` class helps organize and manage a large number of sets efficiently, using Excel-like naming conventions.

```csharp
using SetsLibrary;

var collection = new SetCollection<IStructuredSet<int>>();

// Add multiple sets
for (int i = 0; i < 10; i++)
{
    var tempSet = new TypedSet<int>($"{{{i}, {i + 1}}}", new SetExtractionConfiguration<int>(",", "\n"));
    collection.Add(tempSet);
}

// Access sets by name
var firstSet = collection["A"];
Console.WriteLine($"Set A: {firstSet?.BuildStringRepresentation()}");

// Total number of sets
Console.WriteLine($"Total Sets: {collection.Count}");
```

### **Troubleshooting and Common Issues**

While the Sets Library is designed to be robust, you might encounter some issues. Below is a table of common problems and their resolutions:

| **Issue**                                    | **Cause**                                                                                      | **Resolution**                                                                                       |
|----------------------------------------------|------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------|
| **Invalid terminators in configuration**     | Field or row terminators include reserved characters (e.g., `{`, `}`).                         | Use valid terminators. Refer to the reserved character restrictions in the documentation.             |
| **Null reference in `ToObject` method**      | `ICustomObjectConverter` is not provided, but the configuration requires it.                   | Ensure `SetExtractionConfiguration` is initialized with a valid `ICustomObjectConverter`.             |
| **Incorrect set expression syntax**          | Set expression contains mismatched braces or invalid formatting.                               | Use the `BraceEvaluator` utility to verify and correct the set expression before processing.          |
| **Unexpected behavior with `UnionWith`**     | One or both sets being merged are null or improperly initialized.                              | Validate input sets with `ArgumentNullException.ThrowIfNull` before performing operations.            |
| **Error during set operations**              | Null or improperly configured sets are used in intersection, difference, or complement.        | Verify both sets are properly initialized and configured with valid terminators and converters.       |

### **Debugging Tips**

1. **Use `SetsException` for Detailed Error Context**:  
   `SetsException` includes detailed error messages and a `Details` property for additional context. Here's an example:
   ```csharp
   try
   {
       var result = setA.IntersectWith(setB);
   }
   catch (SetsException ex)
   {
       Console.WriteLine(ex.Message);  // Displays the error message
       Console.WriteLine(ex.Details); // Displays additional error details
   }
   ```

2. **Trace Errors in Custom Operations**:  
   When creating custom operations, use exception chaining to preserve the original error trace:
   ```csharp
   catch (Exception ex)
   {
       throw new SetsOperationException("Custom operation failed", "Additional details about the failure", ex);
   }
   ```

3. **Test Your Configuration**:  
   - Validate terminators and converters in `SetExtractionConfiguration` using unit tests.
   - Ensure mock data aligns with the library's formatting and syntax rules.

By following these tips and using the error-handling mechanisms built into the library, you can identify and resolve issues more effectively.

---
## **Contributing**

We welcome contributions to the Sets Library! Whether it’s fixing a bug, improving the documentation, or adding new features, your input is highly valued. 

### **How to Contribute**
1. **Fork the Repository**:  
   Create your own copy of the repository by clicking the "Fork" button at the top right corner of the GitHub page.

2. **Clone Your Fork**:  
   Clone your fork to your local machine using the following command:
   ```
   git clone https://github.com/<your-username>/Sets-Library.git
   ```

3. **Create a Branch**:  
   Create a new branch for your feature or fix:
   ```
   git checkout -b feature/your-feature-name
   ```

4. **Make Your Changes**:  
   Ensure your code follows the style and structure of the existing codebase. Write tests where applicable to verify your changes.

5. **Run Tests**:  
   Run the test suite to confirm that your changes don’t break existing functionality:
   ```
   dotnet test
   ```

6. **Commit Your Changes**:  
   Use clear and descriptive commit messages:
   ```
   git add .
   git commit -m "Add feature XYZ to Sets Library"
   ```

7. **Push Your Changes**:  
   Push your changes to your fork:
   ```
   git push origin feature/your-feature-name
   ```

8. **Open a Pull Request**:  
   Open a pull request on the main repository and provide a clear description of your changes and their purpose.

---

## **License**

This project is licensed under the MIT License.  
You are free to use, modify, and distribute this library in your projects. See the [LICENSE](https://github.com/Shisui-Pho/Sets-Library/blob/main/LICENSE) file for details.

---

Thank you for using the Sets Library!  
For questions or feedback, please open an issue or contact us through the [repository](https://github.com/Shisui-Pho/Sets-Library).


