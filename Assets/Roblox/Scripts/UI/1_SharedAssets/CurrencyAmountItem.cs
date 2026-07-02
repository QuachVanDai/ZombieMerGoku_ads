using BreakInfinity;
using ExampleProject.Gameplay.Currency;
using ExampleProject.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.UI.Shared
{
    public class CurrencyAmountItem : MonoBehaviour
    {
        #region Fields

        [SerializeField] Image icon;
        [SerializeField] Text amountText;

        #endregion

        #region Properties



        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public void SetData(CurrencyValue _value)
        {
            icon.sprite = Currencies.GetResourceData(_value.type).icon;
            amountText.text = BigDouble.FormatShorthand(_value.amount);
        }

        #endregion
    }
}