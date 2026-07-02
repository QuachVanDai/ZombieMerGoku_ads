using ExampleProject.Gameplay.Currency;
using ExampleProject.Gameplay.Rarity;
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.UI.Shared
{
    public class ItemBackground : MonoBehaviour
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

        public void SetColor(Item _item)
        {
            switch (_item.type)
            {
                case ItemType.Currency:
                    ThisImage.color = Currencies.GetResourceData(_item.currencyValue.type).backgroundColor;
                    break;
                case ItemType.Skin:
                case ItemType.Weapon:
                case ItemType.Wing:
                    ThisImage.color = Rarities.GetResourceData(_item.GetRarity()).colorPack.color;
                    break;
            }
        }
        public void SetActive(bool _isActive)
        {
            gameObject.SetActive(_isActive);
        }

        #endregion
    }
}
