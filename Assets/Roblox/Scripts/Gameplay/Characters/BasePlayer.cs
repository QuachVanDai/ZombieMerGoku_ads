using ExampleProject.Gameplay.Characters.Skin;
using ExampleProject.Gameplay.Unit;
using ExampleProject.Gameplay.Weapon;
using ExampleProject.Gameplay.Wing;
using ExampleProject.Manager;
using ExampleProject.Tools;
using System;
using UnityEngine;

namespace ExampleProject.Gameplay.Characters
{
    public class BasePlayer : MonoBehaviour
    {
        #region Fields

        [SerializeField] protected CharacterAnimator characterAnimator;
        [SerializeField] protected SkinnedMeshRenderer skinnedMeshRenderer;

        [SerializeField] Transform wingPlacement;
        [SerializeField] Transform rightHandWeaponPlacement;
        [SerializeField] Transform leftHandWeaponPlacement;

        [SerializeField] protected Wing.Wing wing;
        [SerializeField] protected Weapon.Weapon weapon;
        [SerializeField] protected SkinId skin;

        #endregion

        #region Properties

        DominantHand DominantHand
        {
            get
            {
                SkinData _data = Skins.GetResourceData(skin);
                return _data != null ? _data.dominantHand : DominantHand.RightHand;
            }
        }


        #endregion

        #region LifeCycle   

        protected virtual void OnEnable()
        {
            EventDispatcher.Instance.AddListener(EventName.OnEquipSkin, OnEquipSkinListener);
            EventDispatcher.Instance.AddListener(EventName.OnEquipWing, OnEquipWingListener);
            EventDispatcher.Instance.AddListener(EventName.OnEquipWeapon, OnEquipWeaponListener);
        }
        protected virtual void OnDisable()
        {
            EventDispatcher.Instance.RemoveListener(EventName.OnEquipSkin, OnEquipSkinListener);
            EventDispatcher.Instance.RemoveListener(EventName.OnEquipWing, OnEquipWingListener);
            EventDispatcher.Instance.RemoveListener(EventName.OnEquipWeapon, OnEquipWeaponListener);
        }


        #endregion

        #region Private Methods

        void OnEquipSkinListener(EventName _key, object _data)
        {
            SetSkin((SkinId)_data);
            if (weapon != null)
                weapon.SetParent(rightHandWeaponPlacement, leftHandWeaponPlacement, DominantHand);
        }
        void OnEquipWingListener(EventName _key, object _data)
        {
            SetWing((WingId)_data);
        }
        void OnEquipWeaponListener(EventName _key, object _data)
        {
            SetWeapon((WeaponId)_data);
        }

        #endregion

        #region Public Methods

        public virtual void Init()
        {
            SetSkin(PlayerEquipment.SkinId);
            SetWing(PlayerEquipment.WingId);
            SetWeapon(PlayerEquipment.WeaponId);
        }
        public virtual void SetParent(Transform _parent)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.SetParent(_parent);
        }
        public virtual void SetSkin(SkinId _id)
        {
            skin = _id;
            var _data = Skins.GetResourceData(skin);
            if (_data == null || skinnedMeshRenderer == null)
                return;

            if (_data.mesh != null)
                skinnedMeshRenderer.sharedMesh = _data.mesh;

            if (_data.material != null)
                skinnedMeshRenderer.sharedMaterial = _data.material;
        }
        public virtual void SetWing(WingId _id)
        {
            if (wing != null)
            {
                wing.SelfDestroy();
                wing = null;
            }
            Helpers.DestroyAllChilds(wingPlacement);

            if (_id == WingId.None)
                return;

            var _wingData = Wings.GetResourceData(_id);
            if (_wingData == null || _wingData.prefab == null || wingPlacement == null)
                return;

            wing = Instantiate(_wingData.prefab, wingPlacement);
            wing.Init(_id);
        }
        public virtual void SetWeapon(WeaponId _id)
        {
            if (weapon != null)
            {
                weapon.SelfDestroy();
                weapon = null;
            }
            Helpers.DestroyAllChilds(rightHandWeaponPlacement);
            Helpers.DestroyAllChilds(leftHandWeaponPlacement);

            if (_id == WeaponId.None)
                return;

            var _weaponData = Weapons.GetResourceData(_id);
            if (_weaponData == null || _weaponData.prefab == null)
                return;

            weapon = Instantiate(_weaponData.prefab);
            weapon.SetParent(rightHandWeaponPlacement, leftHandWeaponPlacement, DominantHand);
            weapon.Init(_id);
        }
        public void SetActive(bool _value)
        {
            gameObject.SetActive(_value);
        }

        #endregion
    }
}
