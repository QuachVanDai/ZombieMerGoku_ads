using ExampleProject.Gameplay.Currency;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.UI.Shared
{
    public class CurrencyColorText_2 : MonoBehaviour
    {
        #region Fields

        [SerializeField] Text text;
        [SerializeField] List<Outline> outlines;

        #endregion

        #region Properties


        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public void Init(CurrencyType _type)
        {
            var _currencyData = Currencies.GetResourceData(_type);
            foreach (var _outline in outlines)
            {
                _outline.effectColor = _currencyData.outlineColor;
            }
            text.color = _currencyData.textColor;
        }
        public void SetText(string _text)
        {
            text.text = _text;
        }

        #endregion
    }
}