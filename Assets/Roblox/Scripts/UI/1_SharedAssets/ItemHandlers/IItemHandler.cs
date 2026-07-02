using UnityEngine;
using System.Collections.Generic;
using ExampleProject.Gameplay.Buffs;
using ExampleProject.Gameplay.Rarity;

namespace ExampleProject.UI.Shared.ItemHandlers
{
    /// <summary>
    /// Interface for handling item-specific operations.
    /// Implement this for each item type to extend Item functionality without modifying the core Item class.
    /// </summary>
    public interface IItemHandler
    {
        /// <summary>
        /// Returns the icon sprite for this item type.
        /// </summary>
        Sprite GetIcon(Item item);

        /// <summary>
        /// Returns the 3D model GameObject for this item type.
        /// </summary>
        GameObject GetModel(Item item);

        /// <summary>
        /// Returns the localized description for this item type.
        /// </summary>
        string GetLocalizedDescription(Item item);

        /// <summary>
        /// Returns the list of buffs for this item type.
        /// </summary>
        List<Buff> GetBuffs(Item item);

        /// <summary>
        /// Returns the localized name for this item type.
        /// </summary>
        string GetLocalizedName(Item item);

        /// <summary>
        /// Returns the rarity of this item type.
        /// </summary>
        RarityId GetRarity(Item item);

        /// <summary>
        /// Compares two items for equality.
        /// </summary>
        bool Equals(Item item1, Item item2);
    }
}
