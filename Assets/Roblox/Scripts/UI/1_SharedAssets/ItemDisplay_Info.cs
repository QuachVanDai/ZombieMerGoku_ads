
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.UI.Shared
{
    public class ItemDisplay_Info : MonoBehaviour
    {
        #region Fields

        [SerializeField] ItemDisplayImage itemDisplayImage;
        [SerializeField] Text itemName;

        #endregion

        #region Properties



        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public void Spawn(Item _item)
        {
            itemDisplayImage.Spawn(_item);
            itemName.text = _item.GetLocalizedName().ToString().ToUpper();
            var _buffs = _item.GetBuffs();
        }
        public void SetActive(bool _isActive)
        {
            this.gameObject.SetActive(_isActive);
        }

        #endregion
    }
}