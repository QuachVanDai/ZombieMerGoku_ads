using UnityEngine;
using System.Collections.Generic;
using ExampleProject.Gameplay.Buffs;
using ExampleProject.Gameplay.Rarity;

namespace ExampleProject.UI.Shared.ItemHandlers
{
    public class RemoveAdsItemHandler : IItemHandler
    {
        public Sprite GetIcon(Item item)
        {
            return null;
        }

        public GameObject GetModel(Item item)
        {
            return null;
        }

        public string GetLocalizedDescription(Item item)
        {
            return null;
        }

        public List<Buff> GetBuffs(Item item)
        {
            return null;
        }

        public string GetLocalizedName(Item item)
        {
            return null;
        }

        public RarityId GetRarity(Item item)
        {
            return RarityId.Common;
        }

        public bool Equals(Item item1, Item item2)
        {
            return item1.removeAdsType == item2.removeAdsType;
        }
    }
}
