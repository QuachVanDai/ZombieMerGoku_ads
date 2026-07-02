using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExampleProject.UI.Shared.ItemHandlers
{
    /// <summary>
    /// Registry that manages item handlers for different item types.
    /// Use this to register custom handlers for item types without modifying the core Item class.
    /// </summary>
    public static class ItemHandlerRegistry
    {
        private static Dictionary<ItemType, IItemHandler> _handlers = new Dictionary<ItemType, IItemHandler>();
        private static bool _initialized = false;

        /// <summary>
        /// Initializes the registry with default handlers for built-in item types.
        /// </summary>
        public static void Initialize()
        {
            if (_initialized)
                return;

            RegisterHandler(ItemType.Currency, new CurrencyItemHandler());
            RegisterHandler(ItemType.Skin, new SkinItemHandler());
            RegisterHandler(ItemType.Weapon, new WeaponItemHandler());
            RegisterHandler(ItemType.Wing, new WingItemHandler());
            RegisterHandler(ItemType.RemoveAds, new RemoveAdsItemHandler());
          
            _initialized = true;
        }

        /// <summary>
        /// Registers a handler for a specific item type.
        /// Call this during game initialization to add custom item types or override existing ones.
        /// </summary>
        public static void RegisterHandler(ItemType itemType, IItemHandler handler)
        {
            if (handler == null)
            {
                Debug.LogError($"Cannot register null handler for item type {itemType}");
                return;
            }

            _handlers[itemType] = handler;
        }

        /// <summary>
        /// Gets the handler for a specific item type.
        /// </summary>
        public static IItemHandler GetHandler(ItemType itemType)
        {
            Initialize();

            if (!_handlers.ContainsKey(itemType))
            {
                Debug.LogWarning($"No handler registered for item type {itemType}");
                return null;
            }

            return _handlers[itemType];
        }

        /// <summary>
        /// Clears all registered handlers. Useful for testing.
        /// </summary>
        public static void Clear()
        {
            _handlers.Clear();
            _initialized = false;
        }
    }
}
