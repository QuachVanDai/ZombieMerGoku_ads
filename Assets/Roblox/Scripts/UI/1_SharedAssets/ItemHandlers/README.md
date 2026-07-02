# Item Handler Architecture - Extension Guide

## Overview
This architecture provides a clean, extensible way to add new item types to the Item system without modifying the core `Item` class. Each item type is handled by an implementation of `IItemHandler`.

## How It Works

### 1. **The Interface (IItemHandler)**
Located in `ItemHandlers/IItemHandler.cs`, this interface defines all operations that can be performed on an item:
- `GetIcon()` - Returns the sprite icon
- `GetModel()` - Returns the 3D model
- `GetLocalizedDescription()` - Returns localized description
- `GetBuffs()` - Returns list of buffs
- `GetLocalizedName()` - Returns localized name
- `GetRarity()` - Returns rarity
- `Equals()` - Compares two items

### 2. **The Registry (ItemHandlerRegistry)**
Located in `ItemHandlers/ItemHandlerRegistry.cs`, this is a centralized registry that:
- Manages all item handlers
- Auto-initializes with built-in handlers on first use
- Allows registration of custom handlers
- Returns the appropriate handler for a given ItemType

### 3. **Item Class**
The `Item` class now delegates all operations to handlers instead of using switch statements:
```csharp
public Sprite GetIcon()
{
    var handler = ItemHandlerRegistry.GetHandler(type);
    return handler != null ? handler.GetIcon(this) : null;
}
```

## Adding a New Item Type - Step by Step

### Step 1: Add ItemType Enum Value
In `Item.cs`, add your new type to the `ItemType` enum:
```csharp
public enum ItemType
{
    // ... existing types ...
    MyCustomItem = 9,  // Add your new type here
}
```

### Step 2: Add Payload to Item Class
Add a field for your item type's data:
```csharp
[ShowIf(nameof(type), ItemType.MyCustomItem)]
public MyCustomItemId myCustomItemId = MyCustomItemId.None;
```

### Step 3: Create Your Handler
Create a new handler class implementing `IItemHandler`:

```csharp
// File: Assets/Roblox/Scripts/UI/1_SharedAssets/ItemHandlers/MyCustomItemHandler.cs
using UnityEngine;
using I2.Loc;
using System.Collections.Generic;
using ExampleProject.Gameplay.Buffs;
using ExampleProject.Gameplay.Rarity;

namespace ExampleProject.UI.Shared.ItemHandlers
{
    public class MyCustomItemHandler : IItemHandler
    {
        public Sprite GetIcon(Item item)
        {
            // Implement icon retrieval
            var customItem = MyCustomItems.GetResourceData(item.myCustomItemId);
            return customItem != null ? customItem.icon : null;
        }

        public GameObject GetModel(Item item)
        {
            // Implement 3D model retrieval if applicable
            return null;
        }

        public LocalizedString GetLocalizedDescription(Item item)
        {
            var customItem = MyCustomItems.GetResourceData(item.myCustomItemId);
            return customItem != null ? customItem.localizedDescription : null;
        }

        public List<Buff> GetBuffs(Item item)
        {
            var customItem = MyCustomItems.GetResourceData(item.myCustomItemId);
            return customItem != null ? customItem.buffs : null;
        }

        public LocalizedString GetLocalizedName(Item item)
        {
            var customItem = MyCustomItems.GetResourceData(item.myCustomItemId);
            return customItem != null ? customItem.localizedName : null;
        }

        public RarityId GetRarity(Item item)
        {
            var customItem = MyCustomItems.GetResourceData(item.myCustomItemId);
            return customItem != null ? customItem.rarity : RarityId.Common;
        }

        public bool Equals(Item item1, Item item2)
        {
            return item1.myCustomItemId == item2.myCustomItemId;
        }
    }
}
```

### Step 4: Register Your Handler
Add registration in the `ItemHandlerRegistry.Initialize()` method or at startup:

**Option A: Update ItemHandlerRegistry.cs**
```csharp
public static void Initialize()
{
    if (_initialized)
        return;

    // ... existing registrations ...
    RegisterHandler(ItemType.MyCustomItem, new MyCustomItemHandler());

    _initialized = true;
}
```

**Option B: Register at Game Startup (e.g., in GameManager)**
```csharp
void Start()
{
    ItemHandlerRegistry.RegisterHandler(ItemType.MyCustomItem, new MyCustomItemHandler());
}
```

## Benefits of This Architecture

1. **No Core Modifications** - Add new item types without modifying `Item.cs`
2. **Easy Testing** - Mock handlers for unit tests
3. **Reusability** - Use across multiple games with different item types
4. **Clean Code** - No massive switch statements
5. **Type Safety** - Compile-time checking via interfaces
6. **Scalability** - Supports unlimited custom item types
7. **Overridable** - Can replace built-in handlers with custom ones

## For Multiple Games/Projects

### Game A Setup
```csharp
// GameA/Scripts/ItemSetup.cs
public class GameAItemSetup
{
    public void Setup()
    {
        ItemHandlerRegistry.Clear(); // Optional: start fresh
        ItemHandlerRegistry.RegisterHandler(ItemType.MyCustomItemA, new CustomHandlerA());
        ItemHandlerRegistry.RegisterHandler(ItemType.MyOtherItem, new CustomHandlerB());
    }
}
```

### Game B Setup
```csharp
// GameB/Scripts/ItemSetup.cs
public class GameBItemSetup
{
    public void Setup()
    {
        ItemHandlerRegistry.RegisterHandler(ItemType.DifferentCustomItem, new CustomHandlerC());
        // Game B uses different items than Game A
    }
}
```

## File Structure

```
Assets/Roblox/Scripts/UI/1_SharedAssets/
├── Item.cs                                  (Core - minimal changes)
├── ItemHandlers/
│   ├── IItemHandler.cs                      (Interface definition)
│   ├── ItemHandlerRegistry.cs               (Central registry)
│   ├── CurrencyItemHandler.cs               (Built-in)
│   ├── SkinItemHandler.cs                   (Built-in)
│   ├── WeaponItemHandler.cs                 (Built-in)
│   ├── WingItemHandler.cs                   (Built-in)
│   ├── PetItemHandler.cs                    (Built-in)
│   ├── RemoveAdsItemHandler.cs              (Built-in)
│   ├── ProfileAvatarItemHandler.cs          (Built-in)
│   ├── ProfileFrameItemHandler.cs           (Built-in)
│   ├── MyCustomItemHandler.cs               (Example custom)
│   └── ...other custom handlers
```

## Notes

- The registry uses lazy initialization - handlers are registered on first call to `GetHandler()`
- All methods gracefully return `null` or default values if a handler is not found
- The `Equals()` method properly handles type comparison before delegating to the handler
