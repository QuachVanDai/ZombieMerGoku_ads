using ExampleProject.Gameplay.Rarity;
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.UI.Shared
{
    public class RarityBackground : MonoBehaviour
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

        public void Test()
        {
            Invoke(nameof(MethodNeedToCall), 0f);
        }
        void MethodNeedToCall()
        {

        }

        #endregion

        #region Public Methods

        public void SetRarity(RarityId _rarity)
        {
            ThisImage.color = Rarities.GetResourceData(_rarity).colorPack.color;
        }
        public void SetActive(bool _isActive)
        {
            gameObject.SetActive(_isActive);
        }

        #endregion
    }
}
