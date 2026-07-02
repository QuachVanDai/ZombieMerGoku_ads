using ExampleProject.Gameplay.Currency;
using ExampleProject.Gameplay.Rarity;
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.UI.Shared
{
    public class CurrencyFrame : MonoBehaviour
    {
        #region Fields

        readonly Image img;

        #endregion

        #region Properties

        Image ThisImage => img != null ? img : GetComponent<Image>();

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public void SetCurrency(CurrencyType _currency)
        {
            ThisImage.color = Currencies.GetResourceData(_currency).backgroundColor;
        }
        public void SetActive(bool _isActive)
        {
            gameObject.SetActive(_isActive);
        }

        #endregion
    }
}
