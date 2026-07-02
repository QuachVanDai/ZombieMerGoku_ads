using System;
using ExampleProject.Tools;
using UnityEngine;

namespace ExampleProject.UI.Shared
{
    [Serializable]
    public class ColorPack
    {
        #region Fields

        public float darkMultiplier = 0.5f;
        public float lightMultiplier = 1.5f;
        public Color color;

        #endregion

        #region Properties

        public Color DarkColor => new Color(color.r * darkMultiplier, color.g * darkMultiplier, color.b * darkMultiplier, color.a);
        public Color LightColor => new Color(color.r * lightMultiplier, color.g * lightMultiplier, color.b * lightMultiplier, color.a);
      

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods



        #endregion
    }
}
