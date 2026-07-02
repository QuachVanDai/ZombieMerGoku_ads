# Item Handler Architecture - Visual Guide

## Component Diagram

```
┌────────────────────────────────────────────────────────────────────────┐
│                         ITEM SYSTEM ARCHITECTURE                       │
└────────────────────────────────────────────────────────────────────────┘

                              USER CODE
                                 │
                     ┌───────────┴───────────┐
                     │                       │
                     ▼                       ▼
            ┌──────────────────┐    ┌──────────────────┐
            │  Game/Scene Code │    │   UI Components  │
            └────────┬─────────┘    └────────┬─────────┘
                     │                       │
                     └───────────┬───────────┘
                                 │
                                 ▼
                    ┌────────────────────────┐
                    │   Item Class           │
                    │ (Facade / Entry Point) │
                    │                        │
                    │ GetIcon()              │
                    │ GetModel()             │
                    │ GetName()              │
                    │ GetRarity()            │
                    │ GetBuffs()             │
                    │ Equals()               │
                    └───────────┬────────────┘
                                │
                                │ Looks up handler
                                ▼
                    ┌────────────────────────────┐
                    │  ItemHandlerRegistry       │
                    │  (Static Registry)         │
                    │                            │
                    │  Dictionary<ItemType,      │
                    │    IItemHandler>           │
                    └───────┬───────────────────┘
                            │
        ┌───────────────────┼───────────────────┐
        │                   │                   │
        ▼                   ▼                   ▼
    ┌─────────┐        ┌─────────┐        ┌─────────┐
    │Currency │        │  Skin   │        │ Weapon  │  ...
    │Handler  │        │ Handler │        │ Handler │
    └────┬────┘        └────┬────┘        └────┬────┘
         │                  │                  │
         └──────────────────┼──────────────────┘
                            │
         ┌──────────────────┴──────────────────┐
         │                                     │
         ▼                                     ▼
    ┌───────────────────┐            ┌──────────────────┐
    │  Game Data        │            │  Asset Loading   │
    │  (Currencies,     │            │  (3D Models,     │
    │   Skins, etc)     │            │   Sprites, etc)  │
    └───────────────────┘            └──────────────────┘
```

## Sequence Diagram: Getting Item Icon

```
User Code                 Item                Registry                Handler
    │                      │                      │                      │
    ├─ item.GetIcon() ────►│                      │                      │
    │                      │                      │                      │
    │                      ├─ GetHandler(type) ──►│                      │
    │                      │                      │                      │
    │                      │◄─ handler instance ──│                      │
    │                      │                      │                      │
    │                      ├─────────────────────────── GetIcon(item) ───►│
    │                      │                      │                      │
    │                      │                      │         [Get icon    │
    │                      │                      │          from data]  │
    │                      │                      │                      │
    │                      │                      │◄───── return icon ────│
    │                      │◄────────────────────────────────────────────│
    │                      │                      │                      │
    │◄─── return icon ─────│                      │                      │
    │                      │                      │                      │
```

## Class Relationship Diagram

```
                        ┌──────────────────────┐
                        │ IItemHandler         │
                        │ ◄◄interface►►       │
                        ├──────────────────────┤
                        │ + GetIcon()          │
                        │ + GetModel()         │
                        │ + GetRarity()        │
                        │ + GetBuffs()         │
                        │ + GetName()          │
                        │ + Equals()           │
                        └──────────┬───────────┘
                                   △
                    ┌──────────────┼──────────────┐
                    │              │              │
        ┌──────────────────┐ ┌──────────────┐ ┌────────────────┐
        │ CurrencyHandler  │ │ SkinHandler  │ │ WeaponHandler  │ ...
        ├──────────────────┤ ├──────────────┤ ├────────────────┤
        │ + GetIcon()      │ │ + GetIcon()  │ │ + GetIcon()    │
        │ + GetRarity()    │ │ + GetModel() │ │ + GetModel()   │
        │ + ...            │ │ + ...        │ │ + ...          │
        └──────────────────┘ └──────────────┘ └────────────────┘


        ┌────────────────────────────────────────────────────────┐
        │ ItemHandlerRegistry (static)                           │
        ├────────────────────────────────────────────────────────┤
        │ - handlers: Dictionary<ItemType, IItemHandler>        │
        │ - initialized: bool                                    │
        ├────────────────────────────────────────────────────────┤
        │ + Initialize()                                         │
        │ + RegisterHandler(type, handler)                       │
        │ + GetHandler(type): IItemHandler                       │
        │ + Clear()                                              │
        └────────────────────────────────────────────────────────┘


        ┌────────────────────────────────────────────────────────┐
        │ Item (Serializable)                                    │
        ├────────────────────────────────────────────────────────┤
        │ - type: ItemType                                       │
        │ - currencyValue: CurrencyValue                         │
        │ - skinId: SkinId                                       │
        │ - weaponId: WeaponId                                   │
        │ - ...other payloads                                    │
        ├────────────────────────────────────────────────────────┤
        │ + GetIcon(): Sprite                                    │
        │ + GetModel(): GameObject                               │
        │ + GetRarity(): RarityId                                │
        │ + GetBuffs(): List<Buff>                               │
        │ + GetName(): LocalizedString                           │
        │ + Equals(Item): bool                                   │
        └────────────────────────────────────────────────────────┘
```

## Adding a New Item Type - Flow Diagram

```
START: Need new item type
  │
  ├─► Step 1: Add to ItemType enum
  │     └─► ItemType.MyCustom = 99
  │
  ├─► Step 2: Add payload field to Item
  │     └─► public MyCustomId myCustomId
  │
  ├─► Step 3: Create Handler class
  │     ├─► Implement IItemHandler
  │     ├─► Implement all 7 methods
  │     └─► File: ItemHandlers/MyCustomHandler.cs
  │
  ├─► Step 4: Register Handler
  │     └─► ItemHandlerRegistry.RegisterHandler(
  │           ItemType.MyCustom, 
  │           new MyCustomHandler()
  │         )
  │
  └─► END: Item system supports MyCustom type
```

## Data Flow: Multiple Games

```
┌─────────────────────────────────────────────────────────────────┐
│                    Shared Package (Package)                     │
│                                                                  │
│  Item.cs                                                         │
│  ItemHandlerRegistry.cs                                          │
│  IItemHandler.cs                                                 │
│  CurrencyHandler.cs                                              │
│  SkinHandler.cs                                                  │
│  WeaponHandler.cs                                                │
└─────────────────────────────────────────────────────────────────┘
                              △
                ┌─────────────┴──────────────┐
                │                            │
    ┌───────────────────────────┐  ┌──────────────────────────┐
    │        Game A             │  │        Game B            │
    ├───────────────────────────┤  ├──────────────────────────┤
    │                           │  │                          │
    │ Handlers:                 │  │ Handlers:                │
    │ - CurrencyHandler         │  │ - CurrencyHandler        │
    │ - SkinHandler             │  │ - SkinHandler            │
    │ - WeaponHandler           │  │ - WeaponHandler          │
    │ - PetHandler              │  │ - SpellHandler (custom)  │
    │                           │  │ - ArtifactHandler        │
    │ Game A Setup:             │  │   (custom)               │
    │ ├─ Use built-ins as-is    │  │                          │
    │ └─ Register Pet handler   │  │ Game B Setup:            │
    │                           │  │ ├─ Override skin handler │
    │                           │  │ ├─ Add spell handler     │
    │                           │  │ └─ Add artifact handler  │
    └───────────────────────────┘  └──────────────────────────┘
```

## Extension Points

```
┌─────────────────────────────────────────────────────────────┐
│              Item System Extension Points                   │
└─────────────────────────────────────────────────────────────┘

1. NEW ITEM TYPE
   └─► Create new handler implementing IItemHandler

2. OVERRIDE EXISTING HANDLER
   └─► Register new handler for existing ItemType

3. CUSTOM INITIALIZATION
   └─► Call ItemHandlerRegistry.RegisterHandler() at startup

4. TESTING
   └─► Create mock handlers and register them
   └─► Call ItemHandlerRegistry.Clear() to reset

5. GAME-SPECIFIC LOGIC
   └─► Each handler can contain game-specific code
   └─► No changes needed to Item.cs
```

## Memory Layout

```
BEFORE (with switches):
┌─────────────────────────────────────────────────────┐
│ Item.cs                                             │
│ ├─ GetIcon()            [Big switch block]         │
│ ├─ GetModel()           [Big switch block]         │
│ ├─ GetBuffs()           [Big switch block]         │
│ ├─ GetRarity()          [Big switch block]         │
│ ├─ GetName()            [Big switch block]         │
│ └─ Equals()             [Big switch block]         │
│                                                     │
│ Size: ~500+ lines of repetitive switch logic      │
└─────────────────────────────────────────────────────┘

AFTER (with handlers):
┌──────────────────────────┐    ┌──────────────────────────┐
│ Item.cs                  │    │ ItemHandlers/            │
│ ├─ GetIcon()             │    │ ├─ IItemHandler.cs       │
│ │  └─ delegate           │    │ ├─ Registry.cs           │
│ ├─ GetModel()            │    │ ├─ Currency.cs           │
│ │  └─ delegate           │    │ ├─ Skin.cs               │
│ ├─ GetBuffs()            │    │ ├─ Weapon.cs             │
│ │  └─ delegate           │    │ └─ ...                   │
│ ├─ GetRarity()           │    │                          │
│ │  └─ delegate           │    │ Size: Modular,           │
│ ├─ GetName()             │    │       organized,          │
│ │  └─ delegate           │    │       maintainable       │
│ └─ Equals()              │    │                          │
│  └─ delegate             │    │                          │
│                          │    │                          │
│ Size: ~80 lines          │    │ Size: ~150 lines total   │
└──────────────────────────┘    └──────────────────────────┘

Total code is similar, but MUCH more organized and maintainable!
```

---

## Key Takeaways

1. **Item** is now a thin facade that delegates to handlers
2. **Handlers** contain the actual logic for each item type
3. **Registry** manages the mapping between ItemTypes and Handlers
4. **Extensibility** is now easy - add handlers, don't modify Item
5. **Reusability** across games is now straightforward
