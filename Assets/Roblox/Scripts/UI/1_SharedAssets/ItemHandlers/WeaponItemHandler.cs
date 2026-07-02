using UnityEngine;
using System.Collections.Generic;
using ExampleProject.Gameplay.Buffs;
using ExampleProject.Gameplay.Rarity;
using ExampleProject.Gameplay.Weapon;

namespace ExampleProject.UI.Shared.ItemHandlers
{
    public class WeaponItemHandler : IItemHandler
    {
        public Sprite GetIcon(Item item)
        {
            var weapon = Weapons.GetResourceData(item.weaponId);
            if (weapon != null && weapon.icon != null)
                return weapon.icon;

            return null;
        }

        public GameObject GetModel(Item item)
        {
            return Weapons.GetWeapon3DModel(item.weaponId);
        }

        public string GetLocalizedDescription(Item item)
        {
            var weapon = Weapons.GetResourceData(item.weaponId);
            if (weapon != null)
                return weapon.localizedDescription;

            return null;
        }

        public List<Buff> GetBuffs(Item item)
        {
            var weapon = Weapons.GetResourceData(item.weaponId);
            if (weapon != null)
                return weapon.buffs;

            return null;
        }

        public string GetLocalizedName(Item item)
        {
            var weapon = Weapons.GetResourceData(item.weaponId);
            if (weapon != null)
                return weapon.localizedName;

            return null;
        }

        public RarityId GetRarity(Item item)
        {
            var weapon = Weapons.GetResourceData(item.weaponId);
            if (weapon != null)
                return weapon.rarity;

            return RarityId.Common;
        }

        public bool Equals(Item item1, Item item2)
        {
            return item1.weaponId == item2.weaponId;
        }
    }
}
