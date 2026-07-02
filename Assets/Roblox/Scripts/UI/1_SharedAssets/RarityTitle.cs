using ExampleProject.Gameplay.Rarity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ExampleProject.UI.Shared
{
    public class RarityTitle : MonoBehaviour
    {
        #region Fields

        Text txt;
        List<Outline> outlines;
        Gradient gradient;

        #endregion

        #region Properties

        Text ThisText => txt != null ? txt : GetComponent<Text>();
        List<Outline> ThisOutlines => outlines != null ? outlines : new List<Outline>(GetComponentsInChildren<Outline>());
        Gradient ThisGradient => gradient != null ? gradient : GetComponent<Gradient>();

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public void SetRarity(RarityId _id)
        {
            var _data = Rarities.GetResourceData(_id);
            

            for (int i = 0; i < ThisOutlines.Count; i++)
            {
                ThisOutlines[i].effectColor = _data.colorPack.DarkColor;
            }
        }

        #endregion
    }
}