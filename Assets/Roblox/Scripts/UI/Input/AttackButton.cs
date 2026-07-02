using DG.Tweening;
using ExampleProject.Gameplay.Weapon;
using ExampleProject.Manager;
using ExampleProject.UI.BaseUI;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ExampleProject.UI.Input
{
    public class AttackButton : BaseButton
    {
        #region Fields

        [SerializeField] Image weaponIcon;

        Tween tween;

        #endregion

        #region Properties

        protected override void OnEnable()
        {
            base.OnEnable();
            EventDispatcher.Instance.AddListener(EventName.OnEquipWeapon, OnEquipWeaponListener);
            UpdateIcon();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            EventDispatcher.Instance.RemoveListener(EventName.OnEquipWeapon, OnEquipWeaponListener);
        }



        #endregion

        #region LifeCycle   

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            tween?.Kill();
            this.transform.localScale = Vector3.one;
            tween = this.transform.DOScale(1.1f, 0.1f).SetLoops(2, LoopType.Yoyo);
        }
        protected override void OnClickListenerMethod()
        {
        }
        void OnEquipWeaponListener(EventName key, object data)
        {
            UpdateIcon();
        }
        void UpdateIcon()
        {
            // Playable keeps the attack button art fixed on the prefab.
            // if (PlayerEquipment.WeaponId == WeaponId.None)
            //     return;
            //
            // weaponIcon.sprite = Weapons.GetResourceData(PlayerEquipment.WeaponId).icon;
        }

        #endregion

        #region Private Methods



        #endregion

        #region Public Methods



        #endregion
    }
}
