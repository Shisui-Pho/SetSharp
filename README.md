

<h1 align="center">SetSharp</h1>
<div align="center">
  <img src="https://img.shields.io/badge/language-C%23-blue.svg" alt="C#" />
  <img src="https://img.shields.io/badge/.NET-8.0-blueviolet.svg" alt=".NET 8" />
  <a href="https://github.com/Shisui-Pho/SetSharp/blob/main/LICENSE">
    <img src="https://img.shields.io/github/license/Shisui-Pho/SetSharp" alt="License" />
  </a>
</div>

## Overview
SetSharp parses and manipulates structured sets expressed as strings. It supports nested subsets, dynamic collections, and custom-object conversion. The API is intentionally small and extensible so you can parse expressions into usable set objects and perform set operations programmatically.

Highlights:
- Compact, extensible API for structured sets
- Typed sets (primitives), string-literal sets, and custom-object sets
- Common set operations: union, intersection, symmetric difference, complement

## Installation
Add the project to your solution as a project reference.

## Quick start
Create a configuration and parse an expression:

```csharp
using SetSharp;

var cfg = new SetsConfigurations(",");
var set = new TypedSet<int>("{1,2,3,2,1}", cfg);
Console.WriteLine(set.BuildStringRepresentation()); // {1,2,3}
```

For custom types implement `ICustomObjectConverter<T>` and use `CustomObjectSet<T>` to parse rows/fields into T.

## API highlights
- TypedSet<T> — strongly-typed sets for primitives (IConvertible).
- StringLiteralSet — preserves raw string elements.
- CustomObjectSet<T> — parse rows/fields into T via `ICustomObjectConverter<T>`.
- SetsConfigurations — configure element/field/row separators and parsing options.
- SetCollection — manage multiple named sets (Excel-style column names).
- SortedCollection<T> — keeps elements in sorted order and supports uniqueness semantics.

## Namespaces
- `SetSharp` — core set types and interfaces.
- `SetSharp.Collections` — collection helpers (SetCollection, SortedCollection, etc.).
- `SetSharp.Operations` — set operation implementations (UnionWith, IntersectWith, SymmetricDifference, ...).
- `SetSharp.Extensions` — conversion and helper extension methods.

## Examples
Typed set parsing and cardinality:

```csharp
var cfg = new SetsConfigurations(",");
var a = new TypedSet<int>("{1,2,3,1}", cfg);
Console.WriteLine(a.Cardinality); // 3
```

Custom object parsing (summary): implement `ICustomObjectConverter<T>.ToObject(string?[] fields)` to convert parsed fields into your type, then use `CustomObjectSet<T>` with a `SetsConfigurations` that specifies field and row terminators.

## Contributing
- Open issues for bugs or feature requests.
- Submit focused PRs with tests and documentation for non-trivial changes.

## License
This project is licensed under the MIT License — see the `LICENSE` file for details.

For questions or to report issues, please open an issue in this repository.