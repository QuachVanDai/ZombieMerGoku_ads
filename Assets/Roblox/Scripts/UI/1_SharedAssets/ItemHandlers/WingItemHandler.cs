using UnityEngine;
using System.Collections.Generic;
using ExampleProject.Gameplay.Buffs;
using ExampleProject.Gameplay.Rarity;
using ExampleProject.Gameplay.Wing;

namespace ExampleProject.UI.Shared.ItemHandlers
{
    public class WingItemHandler : IItemHandler
    {
        public Sprite GetIcon(Item item)
        {
            var wing = Wings.GetResourceData(item.wingId);
            if (wing != null && wing.icon != null)
                return wing.icon;

            return null;
        }

        public GameObject GetModel(Item item)
        {
            return Wings.GetWing3DModel(item.wingId);
        }

        public string GetLocalizedDescription(Item item)
        {
            var wing = Wings.GetResourceData(item.wingId);
            if (wing != null)
                return wing.localizedDescription;

            return null;
        }

        public List<Buff> GetBuffs(Item item)
        {
            var wing = Wings.GetResourceData(item.wingId);
            if (wing != null)
                return wing.buffs;

            return null;
        }

        public string GetLocalizedName(Item item)
        {
            var wing = Wings.GetResourceData(item.wingId);
            if (wing != null)
                return wing.localizedName;

            return null;
        }

        public RarityId GetRarity(Item item)
        {
            var wing = Wings.GetResourceData(item.wingId);
            if (wing != null)
                return wing.rarity;

            return RarityId.Common;
        }

        public bool Equals(Item item1, Item item2)
        {
            return item1.wingId == item2.wingId;
        }
    }
}
