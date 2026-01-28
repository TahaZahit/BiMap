[![.NET](https://github.com/TahaZahit/BiMap/actions/workflows/dotnet.yml/badge.svg)](https://github.com/TahaZahit/BiMap/actions/workflows/dotnet.yml)

# BiMap

A **thread-safe**, high-performance, generic bidirectional map for .NET.

`BiMap<TLeft, TRight>` allows you to map keys to values and values back to keys with O(1) lookups in both directions. It ensures a strict 1-to-1 mapping between the two types.

## Features

- **Bidirectional Mapping**: Fast lookups from Left -> Right and Right -> Left.
- **Thread-Safe**: Fully thread-safe implementation using `ReaderWriterLockSlim` for high-concurrency read scenarios.
- **Atomic Operations**: `Add`, `Remove`, and `Clear` operations are atomic, ensuring both sides of the map remain synchronized.
- **.NET Standard 2.0**: Compatible with .NET Framework 4.6.1+, .NET Core 2.0+, and all modern .NET versions.

## Usage

### Initialization
```csharp
using BiMap;

// Create a new BiMap instance
var countryCodes = new BiMap<int, string>();
```

### Adding Items
```csharp
// Add unique pairs
// Note: Both Left and Right values must be unique in the collection.
bool added = countryCodes.Add(1, "US"); 
countryCodes.Add(44, "UK");
countryCodes.Add(90, "TR");

// Duplicate keys (on either side) will return false and not affect the map
bool result = countryCodes.Add(1, "Canada"); // Returns false
```

### Retrieving Values
```csharp
// Get Right from Left
string country = countryCodes.GetLeftToRight(90); // Returns "TR"
// Or use safe access
if (countryCodes.TryGetLeftToRight(44, out string name))
{
    Console.WriteLine(name); // "UK"
}

// Get Left from Right (Reverse lookup)
int code = countryCodes.GetRightToLeft("US"); // Returns 1
if (countryCodes.TryGetRightToLeft("UK", out int callingCode))
{
    Console.WriteLine(callingCode); // 44
}
```

### Removing Items
Removing an item from one side automatically removes the corresponding mapping from the other side.
```csharp
// Remove by Left key
countryCodes.RemoveByLeft(1); 
// Now "US" is also removed from the Right->Left index

// Remove by Right key
countryCodes.RemoveByRight("UK");
// Now 44 is also removed from the Left->Right index
```

### Thread Safety
This library uses `ReaderWriterLockSlim` to strictly manage access:
- **Multiple readers** can access the map simultaneously (e.g., `Get`, `Contains`, `Enumerate`).
- **Exclusive writers** block all other readers and writers ensures data integrity (e.g., `Add`, `Remove`, `Clear`).

```csharp
// Enumeration is thread-safe (returns a snapshot of the current state)
foreach (var pair in countryCodes.EnumerateLeftToRight())
{
    Console.WriteLine($"{pair.Key} -> {pair.Value}");
}
```

## License

This project is licensed under the MIT License.
