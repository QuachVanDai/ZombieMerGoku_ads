using BreakInfinity;
using ExampleProject.Gameplay.Currency;
using ExampleProject.Manager;
using ExampleProject.UI.BaseUI;
using ExampleProject.UI.BaseUI.BasePopup;

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.UI.Shared
{
    public class CurrencyInfo : BaseButton
    {
        #region Fields

        [SerializeField] CurrencyType currencyType;
        [SerializeField] Image icon;
        [SerializeField] Image frame;
        [SerializeField] CurrencyColorText amountText;
        [SerializeField] FloatingTextSpawner_2 floatingTextSpawner;

        #endregion

        #region Properties


        #endregion

        #region LifeCycle   

        protected override void OnEnable()
        {
            base.OnEnable();
            EventDispatcher.Instance.AddListener(EventName.OnUpdateCurrency, OnUpdateCoinListener);
            Init();
            UpdateUI();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            EventDispatcher.Instance.RemoveListener(EventName.OnUpdateCurrency, OnUpdateCoinListener);
        }


        #endregion

        #region Private Methods

        void OnUpdateCoinListener(EventName key, object data)
        {
            UpdateUI();

            // Spawn floating text
            CurrencyValue _value = data as CurrencyValue;
            if (_value.type != currencyType)
                return;

            string _sign = _value.amount >= 0 ? "+" : "";
            floatingTextSpawner.SpawnFloatingText(_value.type, $"{_sign}{BigDouble.FormatShorthand(_value.amount)}");

        }
        void Init()
        {
            amountText.Init(currencyType);
            icon.sprite = Currencies.GetResourceData(currencyType).icon;
            frame.sprite = Currencies.GetResourceData(currencyType).frame;
        }
        void UpdateUI()
        {
            var _text = BigDouble.FormatShorthand(CurrencyProgress.GetCurrencyAmount(currencyType));
            amountText.SetText(_text);
        }

        #endregion

        #region Public Methods

        protected override void OnClickListenerMethod()
        {
            base.OnClickListenerMethod();
        }


        #endregion
    }
}
