using ExampleProject.Gameplay.Rarity;
using ExampleProject.Manager;
using ExampleProject.UI.BaseUI.BasePopup;
using ExampleProject.UI.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VTLTools.Effect;

namespace ExampleProject.Gameplay.Currency
{
    [CreateAssetMenu(fileName = "CurrencyData", menuName = "ScriptableObjects/Currency/CurrencyData")]
    public class CurrencyData : ScriptableObject
    {
        #region Fields

        public CurrencyType type;
        public Sprite icon;
        public Sprite frame;
        public Color backgroundColor;
        public Color textColor;
        public Color outlineColor;
        public Effect collectEffect;

        #endregion

        #region Properties



        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods



        #endregion
    }

    public enum CurrencyType
    {
        None = 0,
        Coin = 1,
        Gem = 2,
        LuckyWheelTicket =3
    }
}
