using UnityEngine;
using System.Collections.Generic;
using ExampleProject.Gameplay.Buffs;
using ExampleProject.Gameplay.Rarity;
// using ExampleProject.Gameplay.YourItemType;  // <- Add your namespace here

namespace ExampleProject.UI.Shared.ItemHandlers
{
    /// <summary>
    /// Template for creating a custom item handler.
    /// 
    /// Instructions:
    /// 1. Replace "TemplateItemHandler" with your handler name
    /// 2. Add your item type namespace above
    /// 3. Replace "YourItem" references with your actual data types
    /// 4. Implement each method to retrieve data from your item system
    /// 5. Register in ItemHandlerRegistry.Initialize() or at startup
    /// 
    /// Example:
    /// ItemHandlerRegistry.RegisterHandler(ItemType.YourCustomType, new YourCustomItemHandler());
    /// </summary>
    public class TemplateItemHandler : IItemHandler
    {
        /// <summary>
        /// Returns the icon sprite for your item.
        /// Implement based on how your item system stores icons.
        /// </summary>
        public Sprite GetIcon(Item item)
        {
            // TODO: Replace with your implementation
            // Example pattern:
            // var data = YourItems.GetResourceData(item.yourItemId);
            // return data != null ? data.icon : null;

            return null;
        }

        /// <summary>
        /// Returns the 3D model for your item, if applicable.
        /// Return null if your item type doesn't have 3D models.
        /// </summary>
        public GameObject GetModel(Item item)
        {
            // TODO: Replace with your implementation or leave as null
            // Example pattern:
            // return YourItems.GetItemModel(item.yourItemId);

            return null;
        }

        /// <summary>
        /// Returns the localized description for your item.
        /// Used for displaying item details in UI.
        /// </summary>
        public string GetLocalizedDescription(Item item)
        {
            // TODO: Replace with your implementation
            // Example pattern:
            // var data = YourItems.GetResourceData(item.yourItemId);
            // return data != null ? data.localizedDescription : null;

            return null;
        }

        /// <summary>
        /// Returns the list of buffs/stats for your item.
        /// Return null if your item type doesn't have buffs.
        /// </summary>
        public List<Buff> GetBuffs(Item item)
        {
            // TODO: Replace with your implementation or leave as null
            // Example pattern:
            // var data = YourItems.GetResourceData(item.yourItemId);
            // return data != null ? data.buffs : null;

            return null;
        }

        /// <summary>
        /// Returns the localized name for your item.
        /// Used for displaying item name in UI.
        /// </summary>
        public string GetLocalizedName(Item item)
        {
            // TODO: Replace with your implementation
            // Example pattern:
            // var data = YourItems.GetResourceData(item.yourItemId);
            // return data != null ? data.localizedName : null;

            return null;
        }

        /// <summary>
        /// Returns the rarity level for your item.
        /// Common is the default fallback.
        /// </summary>
        public RarityId GetRarity(Item item)
        {
            // TODO: Replace with your implementation
            // Example pattern:
            // var data = YourItems.GetResourceData(item.yourItemId);
            // return data != null ? data.rarity : RarityId.Common;

            return RarityId.Common;
        }

        /// <summary>
        /// Compares two items of your type for equality.
        /// Should return true only if both items represent the same item.
        /// </summary>
        public bool Equals(Item item1, Item item2)
        {
            // TODO: Replace with your implementation
            // Example pattern (for ID-based items):
            // return item1.yourItemId == item2.yourItemId;

            // Example pattern (for value-based items):
            // return item1.yourItemValue.Equals(item2.yourItemValue);

            return false;
        }
    }
}
