# Quick Reference - Item Handler Architecture

## For Game Developers: Quick Start

### I just want to use Item - How?
Use it exactly as before! No changes needed:
```csharp
Item myItem = new Item { type = ItemType.Skin, skinId = SkinId.Player };
Sprite icon = myItem.GetIcon();
LocalizedString name = myItem.GetLocalizedName();
```

### I want to add a custom item type - How?
1. Add to enum: `ItemType.MyCustom = 99,` in `Item.cs`
2. Add field: `public MyCustomId myCustomId = MyCustomId.None;` in `Item.cs`
3. Create handler: Copy `TEMPLATE_CustomItemHandler.cs` and implement methods
4. Register: Add one line in `ItemHandlerRegistry.Initialize()` or at game start

### Where are the handlers?
All in: `Assets/Roblox/Scripts/UI/1_SharedAssets/ItemHandlers/`

### Can I override existing handlers?
Yes! Call at startup:
```csharp
ItemHandlerRegistry.RegisterHandler(ItemType.Currency, new MyCustomCurrencyHandler());
```

---

## Architecture Overview

```
┌─────────────────────────────────────────┐
│         Item Class                      │
│  (Minimal, delegates to handlers)       │
└────────────────┬────────────────────────┘
                 │
                 │ Gets handler
                 ▼
┌─────────────────────────────────────────┐
│    ItemHandlerRegistry                  │
│  (Lookup: ItemType → IItemHandler)      │
└────────────────┬────────────────────────┘
                 │
    ┌────────────┼────────────┐
    │            │            │
    ▼            ▼            ▼
┌──────────┐ ┌──────────┐ ┌──────────┐
│ Currency │ │  Skin    │ │ Weapon   │ ...
│ Handler  │ │ Handler  │ │ Handler  │
└──────────┘ └──────────┘ └──────────┘
```

---

## Key Classes

### `IItemHandler` Interface
```csharp
// Define: ItemHandlers/IItemHandler.cs
Sprite GetIcon(Item item)
GameObject GetModel(Item item)
LocalizedString GetLocalizedDescription(Item item)
List<Buff> GetBuffs(Item item)
LocalizedString GetLocalizedName(Item item)
RarityId GetRarity(Item item)
bool Equals(Item item1, Item item2)
```

### `ItemHandlerRegistry` Static Class
```csharp
// Lookup the handler for an item type
IItemHandler handler = ItemHandlerRegistry.GetHandler(ItemType.Skin);

// Register a handler
ItemHandlerRegistry.RegisterHandler(ItemType.MyType, new MyHandler());

// Clear all handlers (testing)
ItemHandlerRegistry.Clear();
```

### `Item` Class
```csharp
// Delegate to appropriate handler
public Sprite GetIcon()
public GameObject GetModel()
public LocalizedString GetLocalizedDescription()
public List<Buff> GetBuffs()
public LocalizedString GetLocalizedName()
public RarityId GetRarity()
public bool Equals(Item other)
```

---

## Common Tasks

### Task: Add Support for a New Item Type

**File: `Item.cs`**
```csharp
public enum ItemType {
    None = 0,
    Currency = 1,
    Skin = 2,
    MyNewItem = 99,  // <- ADD
}

public class Item {
    // ... existing fields ...

    [ShowIf(nameof(type), ItemType.MyNewItem)]
    public MyNewItemId myNewItemId = MyNewItemId.None;  // <- ADD
}
```

**File: `ItemHandlers/MyNewItemHandler.cs`** (new file)
```csharp
public class MyNewItemHandler : IItemHandler {
    public Sprite GetIcon(Item item) { 
        // Implement
    }
    public GameObject GetModel(Item item) { 
        // Implement
    }
    // ... etc ...
}
```

**File: `ItemHandlers/ItemHandlerRegistry.cs`**
```csharp
public static void Initialize() {
    if (_initialized) return;

    // ... existing registrations ...
    RegisterHandler(ItemType.MyNewItem, new MyNewItemHandler());  // <- ADD

    _initialized = true;
}
```

### Task: Use in a UI System
```csharp
public void DisplayItem(Item item) {
    iconImage.sprite = item.GetIcon();
    nameText.text = item.GetLocalizedName();
    descText.text = item.GetLocalizedDescription();

    var buffs = item.GetBuffs();
    if (buffs != null) {
        buffDisplay.ShowBuffs(buffs);
    }
}
```

### Task: Compare Items
```csharp
if (inventoryItem.Equals(equippedItem)) {
    Debug.Log("This item is already equipped!");
}
```

### Task: Use in Multiple Games
```csharp
// GameA - Uses Currency, Skin, Weapon
public class GameASetup : MonoBehaviour {
    void Start() {
        // Built-in handlers auto-register
        // Use as-is!
    }
}

// GameB - Uses Currency, Skin, Spell, Artifact
public class GameBSetup : MonoBehaviour {
    void Start() {
        // Override with custom handlers
        ItemHandlerRegistry.RegisterHandler(
            ItemType.Skin, 
            new GameBSpecificSkinHandler()
        );
    }
}
```

---

## File Locations

```
Assets/Roblox/Scripts/UI/1_SharedAssets/
├── Item.cs                                (MODIFIED - now uses handlers)
├── REFACTORING_SUMMARY.md                 (Detailed changes)
├── ItemHandlers/
│   ├── IItemHandler.cs                    (INTERFACE)
│   ├── ItemHandlerRegistry.cs             (REGISTRY)
│   ├── README.md                          (FULL DOCS)
│   ├── TEMPLATE_CustomItemHandler.cs      (TEMPLATE FOR NEW HANDLERS)
│   ├── CurrencyItemHandler.cs             (Built-in)
│   ├── SkinItemHandler.cs                 (Built-in)
│   ├── WeaponItemHandler.cs               (Built-in)
│   ├── WingItemHandler.cs                 (Built-in)
│   ├── PetItemHandler.cs                  (Built-in)
│   ├── RemoveAdsItemHandler.cs            (Built-in)
│   ├── ProfileAvatarItemHandler.cs        (Built-in)
│   ├── ProfileFrameItemHandler.cs         (Built-in)
│   └── Examples/
│       └── ItemHandlerUsageExample.cs     (EXAMPLES)
```

---

## Troubleshooting

### "Handler not found for ItemType.X"
- Did you register it? Add to `ItemHandlerRegistry.Initialize()` or call `RegisterHandler()`
- Did you add it to the enum? Check `ItemType.cs`

### "Item methods return null"
- Handler might not be registered
- Handler's methods are returning null (check implementation)

### "I want to test my handler"
- Use `ItemHandlerRegistry.Clear()` to reset
- Create a mock handler: `class MockHandler : IItemHandler { ... }`
- Register: `ItemHandlerRegistry.RegisterHandler(type, mockHandler)`

---

## Benefits at a Glance

✅ Add new item types **without modifying Item.cs**
✅ Reuse across **multiple games**
✅ Easier **testing** with mock handlers
✅ **No breaking changes** to existing code
✅ Clean, **maintainable** architecture
✅ **Scales** to unlimited item types

---

## See Also

- `README.md` - Full documentation and extension guide
- `REFACTORING_SUMMARY.md` - What changed and why
- `TEMPLATE_CustomItemHandler.cs` - Template for your custom handlers
- `Examples/ItemHandlerUsageExample.cs` - Usage examples
