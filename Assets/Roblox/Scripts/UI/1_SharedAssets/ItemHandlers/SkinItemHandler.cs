using UnityEngine;
using System.Collections.Generic;
using ExampleProject.Gameplay.Buffs;
using ExampleProject.Gameplay.Rarity;
using ExampleProject.Gameplay.Characters.Skin;

namespace ExampleProject.UI.Shared.ItemHandlers
{
    public class SkinItemHandler : IItemHandler
    {
        public Sprite GetIcon(Item item)
        {
            var skin = Skins.GetResourceData(item.skinId);
            if (skin != null && skin.avatar != null)
                return skin.avatar;

            return null;
        }

        public GameObject GetModel(Item item)
        {
            return Skins.GetSkin3DModel(item.skinId);
        }

        public string GetLocalizedDescription(Item item)
        {
            var skin = Skins.GetResourceData(item.skinId);
            if (skin != null)
                return skin.id.ToString();

            return null;
        }

        public List<Buff> GetBuffs(Item item)
        {
            var skin = Skins.GetResourceData(item.skinId);
            if (skin != null)
                return skin.buffs;

            return null;
        }

        public string GetLocalizedName(Item item)
        {
            var skin = Skins.GetResourceData(item.skinId);
            if (skin != null)
                return skin.id.ToString();

            return null;
        }

        public RarityId GetRarity(Item item)
        {
            var skin = Skins.GetResourceData(item.skinId);
            if (skin != null)
                return skin.rarity;

            return RarityId.Common;
        }

        public bool Equals(Item item1, Item item2)
        {
            return item1.skinId == item2.skinId;
        }
    }
}
