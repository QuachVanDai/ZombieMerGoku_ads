using System.Collections;
using System.Collections.Generic;
using ExampleProject.Gameplay.Scenes;
using ExampleProject.Manager;
using ExampleProject.UI.BaseUI;
using ExampleProject.UI.BaseUI.BasePopup;
using UnityEngine;

namespace ExampleProject
{
    public class HidePopupButton : BaseButton
    {
        #region Fields
        [SerializeField] BasePopup popupToHide;


        #endregion

        #region Properties



        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods

        protected override void OnClickListenerMethod()
        {
            base.OnClickListenerMethod();
            popupToHide.Hide();
        }

        #endregion

        #region Public Methods



        #endregion
    }
}
