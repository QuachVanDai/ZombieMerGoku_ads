using ExampleProject.Gameplay.Characters.Skin;
using UnityEngine;

namespace ExampleProject.Gameplay.Weapon
{
    public class DualWeapon : Weapon
    {
        #region Fields

        [SerializeField] Transform weapon1;
        [SerializeField] Transform weapon2;

        #endregion

        #region Properties



        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public override void SetParent(Transform _rightHand, Transform _leftHand, DominantHand _dominantHand)
        {
            Transform _weapon1Parent = _dominantHand == DominantHand.RightHand ? _rightHand : _leftHand;
            Transform _weapon2Parent = _dominantHand != DominantHand.RightHand ? _rightHand : _leftHand;

            weapon1.SetParent(_weapon1Parent);
            weapon2.SetParent(_weapon2Parent);

            weapon1.localPosition = Vector3.zero;
            weapon1.localRotation = Quaternion.identity;
            weapon2.localPosition = Vector3.zero;
            weapon2.localRotation = Quaternion.identity;

            weapon1.localScale = Vector3.one;
            weapon2.localScale = new Vector3(-1, 1, -1);
        }

        #endregion
    }
}
