using ExampleProject.Gameplay.Rarity;
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.UI.Shared
{
    public class RarityFrame : MonoBehaviour
    {
        #region Fields

        [SerializeField] Image frame;
        [SerializeField] RarityTitle rarityTitle;

        #endregion

        #region Properties



        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public void SetRarity(RarityId _rarity)
        {
            var _data = Rarities.GetResourceData(_rarity);
            frame.color = _data.colorPack.color;
            rarityTitle.SetRarity(_rarity);
        }

        #endregion
    }
}
