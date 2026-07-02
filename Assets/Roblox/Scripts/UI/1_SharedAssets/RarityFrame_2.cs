using ExampleProject.Gameplay.Rarity;
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.UI.Shared
{
    public class RarityFrame_2 : MonoBehaviour
    {
        #region Fields

        [SerializeField] Image frame;
        [SerializeField] Image titleFrame;
        [SerializeField] Text titleText;
        [SerializeField] Text rarityText;

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

            // 
            frame.color = _data.colorPack.color;

            //
            titleFrame.color = _data.colorPack.color;

            //
            titleText.color = _data.colorPack.DarkColor;

            //
            rarityText.color = _data.colorPack.color;
        }
        public void SetTitle(string _title)
        {
            titleText.text = _title;
        }

        #endregion
    }
}
