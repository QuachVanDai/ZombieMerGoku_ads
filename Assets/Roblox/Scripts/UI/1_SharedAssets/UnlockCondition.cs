using System;
using ExampleProject.Manager;

namespace ExampleProject.UI.Shared
{
    [Serializable]
    public class UnlockCondition
    {
        public UnlockMethod method = UnlockMethod.Free;
        public int value;
        public CurrencyValue currencyValue;

        public bool IsCurrencyUnlock => method == UnlockMethod.Currency;
    }

    public enum UnlockMethod
    {
        Free = 0,
        Currency = 1,
        Ads = 2,
        IAP = 3,
    }
}
