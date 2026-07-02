using System;
using UnityEngine;
using ExampleProject.Gameplay.Currency;
using ExampleProject.Gameplay.Weapon;
using ExampleProject.Gameplay.Characters.Skin;
using ExampleProject.Gameplay.Wing;

using ExampleProject.Manager;
using System.Collections.Generic;
using ExampleProject.Gameplay.Buffs;
using ExampleProject.Gameplay.Rarity;
using ExampleProject.UI.Shared.ItemHandlers;

namespace ExampleProject.UI.Shared
{
    public enum ItemType
    {
        None = 0,
        Currency = 1,
        Skin = 2,
        Weapon = 3,
        Wing = 4,
        RemoveAds = 6,
        ProfileAvatar = 7,
        ProfileFrame = 8,
    }

    [Serializable]
    public class Item
    {
        // Type of reward
        public ItemType type = ItemType.None;

        // Currency payload (used when type == Currency)
        public CurrencyValue currencyValue;

        // Skin payload (used when type == Skin)
        public SkinId skinId = SkinId.None;

        // Weapon payload (used when type == Weapon)
        public WeaponId weaponId = WeaponId.None;

        // wing payload (used when type == Wing)
        public WingId wingId = WingId.None;

        // Remove ads payload (used when type == RemoveAds)
        public RemoveAdsType removeAdsType = RemoveAdsType.None;



        // Get icon to show on popup (falls back to overrides)
        public Sprite GetIcon()
        {
            var handler = ItemHandlerRegistry.GetHandler(type);
            return handler != null ? handler.GetIcon(this) : null;
        }

        // Get 3D model
        public GameObject GetModel()
        {
            var handler = ItemHandlerRegistry.GetHandler(type);
            return handler != null ? handler.GetModel(this) : null;
        }

        // Get localized name to show on popup (falls back to overrides)
        public string GetLocalizedDescription()
        {
            var handler = ItemHandlerRegistry.GetHandler(type);
            return handler != null ? handler.GetLocalizedDescription(this) : null;
        }

        // Get buffs
        public List<Buff> GetBuffs()
        {
            var handler = ItemHandlerRegistry.GetHandler(type);
            return handler != null ? handler.GetBuffs(this) : null;
        }

        // Get localized name
        public string GetLocalizedName()
        {
            var handler = ItemHandlerRegistry.GetHandler(type);
            return handler != null ? handler.GetLocalizedName(this) : null;
        }

        // Get rarity
        public RarityId GetRarity()
        {
            var handler = ItemHandlerRegistry.GetHandler(type);
            if (handler != null)
                return handler.GetRarity(this);

            return RarityId.Common;
        }

        // Compare 2 items for equality (used for checking if showing item is equipped item)
        public bool Equals(Item _other)
        {
            if (type != _other.type)
                return false;

            var handler = ItemHandlerRegistry.GetHandler(type);
            return handler != null && handler.Equals(this, _other);
        }
    }
}
