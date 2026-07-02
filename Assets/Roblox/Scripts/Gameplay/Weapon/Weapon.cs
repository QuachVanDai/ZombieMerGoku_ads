using ExampleProject.Gameplay.Characters.Skin;
using ExampleProject.Tools;
using System;
using UnityEngine;

namespace ExampleProject.Gameplay.Weapon
{
    public class Weapon : MonoBehaviour
    {
        #region Fields

        public WeaponId id;

        #endregion

        #region Properties



        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public void SetActive(bool _isActive)
        {
            gameObject.SetActive(_isActive);
        }
        public void SelfDestroy()
        {
            Destroy(gameObject);
        }
        public void SetUILayerAllChildren()
        {
            Helpers.SetLayerAllChildren(transform, StringsSafeAccess.LAYER_UI_3D);
        }

        public void Init(WeaponId _id)
        {
            this.id = _id;
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
        }
        public virtual void SetParent(Transform  _rightHand, Transform _leftHand, DominantHand _dominantHand)
        {
            Transform _parent = _dominantHand == DominantHand.RightHand ? _rightHand : _leftHand;
            transform.SetParent(_parent);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        #endregion
    }
}
