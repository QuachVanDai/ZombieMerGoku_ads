using BreakInfinity;
using ExampleProject.Gameplay.Currency;
using ExampleProject.GameSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExampleProject.Manager
{
    [Serializable]
    public class CurrencyProgress
    {
        #region Fields

        [SerializeField] List<CurrencyValue> currencyData = new List<CurrencyValue>();        [SerializeField] List<CurrencyValue> currencyEarnedData = new List<CurrencyValue>();
        #endregion

        #region Properties

        public static CurrencyProgress Instance
        {
            get => SharedRobloxUserData.CurrencyProgress;
            set => SharedRobloxUserData.CurrencyProgress = value;
        }

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public static void AddCurrency(CurrencyValue _value, string _where, int _level, bool _isPlaySound = true)
        {
            var _data = Instance.currencyData.Find(_c => _c.type == _value.type);
            if (_data != null)
                _data.amount += _value.amount;
            else
                Instance.currencyData.Add(new CurrencyValue(_value.type, _value.amount));

            if (_isPlaySound)
                SoundSystem.Instance.PlayCoinClaim();
            EventDispatcher.Instance.Dispatch(EventName.OnUpdateCurrency, _value);

            // Track earned currency for analytics
            var _earnedData = Instance.currencyEarnedData.Find(_c => _c.type == _value.type);
            if (_earnedData != null)
                _earnedData.amount += _value.amount;
            else
                Instance.currencyEarnedData.Add(new CurrencyValue(_value.type, _value.amount));

            UserDataManager.SaveData();
        }
        public static void SubtractCurrency(CurrencyValue _value, string _where, int _level)
        {
            var _data = Instance.currencyData.Find(c => c.type == _value.type);
            if (_data != null && _data.amount >= _value.amount)
            {
                _data.amount -= _value.amount;
                EventDispatcher.Instance.Dispatch(EventName.OnUpdateCurrency, new CurrencyValue(_value.type, -_value.amount));
                UserDataManager.SaveData();
            }
            else
                Debug.Log($"Cannot subtract");
        }
        public static BigDouble GetCurrencyAmount(CurrencyType _type)
        {
            var data = Instance.currencyData.Find(_c => _c.type == _type);
            return data != null ? data.amount : 0;
        }
        public static BigDouble GetCurrencyEarnedAmount(CurrencyType _type)
        {
            var data = Instance.currencyEarnedData.Find(_c => _c.type == _type);
            return data != null ? data.amount : 0;
        }
        public static bool HasEnoughCurrency(CurrencyType _type, BigDouble _amount)
        {
            return GetCurrencyAmount(_type) >= _amount;
        }
        public static bool HasEnoughCurrency(CurrencyValue _value)
        {
            return GetCurrencyAmount(_value.type) >= _value.amount;
        }
        public CurrencyProgress()
        {
            currencyData = new List<CurrencyValue>();
        }

        #endregion
    }
    [Serializable]
    public class CurrencyValue
    {
        public CurrencyType type;
        public BigDouble amount;

        public double AmountAsDouble => amount.ToDouble();

        public CurrencyValue(CurrencyType _type, BigDouble _amount)
        {
            type = _type;
            amount = _amount;
        }
    }
}
