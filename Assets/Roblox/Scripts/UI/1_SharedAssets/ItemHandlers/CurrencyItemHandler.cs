using UnityEngine;
using System.Collections.Generic;
using ExampleProject.Gameplay.Buffs;
using ExampleProject.Gameplay.Rarity;
using ExampleProject.Gameplay.Currency;

namespace ExampleProject.UI.Shared.ItemHandlers
{
    public class CurrencyItemHandler : IItemHandler
    {
        public Sprite GetIcon(Item item)
        {
            if (item.currencyValue == null)
                return null;

            var currency = Currencies.GetResourceData(item.currencyValue.type);
            if (currency != null && currency.icon != null)
                return currency.icon;

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
            if (item1.currencyValue == null && item2.currencyValue == null)
                return true;

            if (item1.currencyValue == null || item2.currencyValue == null)
                return false;

            return item1.currencyValue.Equals(item2.currencyValue);
        }
    }
}
