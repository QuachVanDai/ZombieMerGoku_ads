using ExampleProject.Gameplay.Characters.Skin;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ExampleProject.Gameplay.Wing;
using ExampleProject.Gameplay.Weapon;
using ExampleProject.UI.Shared;


namespace ExampleProject.Manager
{
    [Serializable]
    public class PlayerEquipment
    {
        #region Fields

        [SerializeField] SkinId skinId;
        [SerializeField] WingId wingId;
        [SerializeField] WeaponId weaponId;

        [SerializeField] string userName;

        #endregion

        #region Properties

        public static PlayerEquipment Instance => SharedRobloxUserData.PlayerEquipment;



        public static SkinId SkinId
        {
            get => Instance.skinId;
            set
            {
                Instance.skinId = value;
                UserDataManager.SaveData();
            }
        }
        public static WingId WingId
        {
            get => Instance.wingId;
            set
            {
                Instance.wingId = value;
                UserDataManager.SaveData();
            }
        }
        public static WeaponId WeaponId
        {
            get => Instance.weaponId;
            set
            {
                Instance.weaponId = value;
                UserDataManager.SaveData();
            }
        }
        public static string UserName
        {
            get => Instance.userName;
            set
            {
                Instance.userName = value;
                UserDataManager.SaveData();
            }
        }

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods

        public static void EquipSkin(SkinId _skinId)
        {
            SkinId = _skinId;
            EventDispatcher.Instance.Dispatch(EventName.OnEquipSkin, _skinId);
        }
        public static void EquipWing(WingId _wingId)
        {
            WingId = _wingId;
            EventDispatcher.Instance.Dispatch(EventName.OnEquipWing, _wingId);
        }
        public static void EquipWeapon(WeaponId _weaponId)
        {
            WeaponId = _weaponId;
            EventDispatcher.Instance.Dispatch(EventName.OnEquipWeapon, _weaponId);
        }
        public static void Equip(Item _item)
        {
            switch (_item.type)
            {
                case ItemType.Weapon:
                    EquipWeapon(_item.weaponId);
                    break;
                case ItemType.Skin:
                    EquipSkin(_item.skinId);
                    break;
                case ItemType.Wing:
                    EquipWing(_item.wingId);
                    break;
            }
        }
        public static void Unequip(Item _item)
        {
            switch (_item.type)
            {
                case ItemType.Weapon:
                    EquipWeapon(Weapons.DefaultWeapon); // Default weapon
                    break;
                case ItemType.Skin:
                    EquipSkin(Skins.DefaultSkin);
                    break;
                case ItemType.Wing:
                    EquipWing(WingId.None);
                    break;
            }
        }
        public static bool IsEquipped(Item _item)
        {
            if (_item == null)
                return false;

            switch (_item.type)
            {
                case ItemType.Weapon:
                    return _item.weaponId == WeaponId;
                case ItemType.Skin:
                    return _item.skinId == SkinId;
                case ItemType.Wing:
                    return _item.wingId == WingId;
                default:
                    return false;
            }
        }
        public static bool IsDefaultItem(Item _item)
        {
            if (_item == null)
                return false;

            switch (_item.type)
            {
                case ItemType.Weapon:
                    return _item.weaponId == Weapons.DefaultWeapon;
                case ItemType.Skin:
                    return _item.skinId == Skins.DefaultSkin;
                case ItemType.Wing:
                    return _item.wingId == WingId.None;
                default:
                    return false;
            }
        }

        #endregion

        #region Public Methods

        public PlayerEquipment()
        {

            skinId = Skins.DefaultSkin;
            wingId = WingId.None;
            weaponId = Weapons.DefaultWeapon; // Default weapon

            var deviceId = SystemInfo.deviceUniqueIdentifier;
            var nameSuffix = deviceId.Length >= 4 ? deviceId.Substring(0, 4) : deviceId;
            userName = $"Player#{nameSuffix}";
        }

        #endregion
    }
}
