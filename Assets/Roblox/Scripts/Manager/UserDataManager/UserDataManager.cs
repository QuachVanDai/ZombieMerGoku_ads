using ExampleProject.GameSystem;

using ExampleProject.UI.Shared;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExampleProject.Manager
{
    public class UserDataManager
    {
        public static void AddItem(Item _item, string _where = "", int _level = 0)
        {
            if (_item == null || _item.type == ItemType.None)
                return;

            if (_item.type == ItemType.Currency && _item.currencyValue == null)
                return;

            switch (_item.type)
            {
                case ItemType.Skin:
                    SkinProgress.UnlockSkin(_item.skinId);
                    break;
                case ItemType.Weapon:
                    WeaponProgress.UnlockWeapon(_item.weaponId);
                    break;
                case ItemType.Wing:
                    WingProgress.UnlockWing(_item.wingId);
                    break;
                case ItemType.Currency:
                    CurrencyProgress.AddCurrency(_item.currencyValue, _where, _level);
                    break;
                case ItemType.RemoveAds:
                    ShopProgress.RemoveAds(_item.removeAdsType);
                    break;
                default:
                    Debug.LogWarning("Unknown item type: " + _item.type);
                    break;
            }
        }
        public static void AddItems(List<Item> _items, string _where = "", int _level = 0)
        {
            if (_items == null || _items.Count == 0)
                return;

            foreach (var _item in _items)
            {
                AddItem(_item, _where, _level);
            }
        }
        public static bool HasItem(Item _item)
        {
            if (_item == null)
                return false;

            switch (_item.type)
            {
                case ItemType.Skin:
                    return SkinProgress.IsUnlocked(_item.skinId);
                case ItemType.Weapon:
                    return WeaponProgress.IsUnlocked(_item.weaponId);
                case ItemType.Wing:
                    return WingProgress.IsUnlocked(_item.wingId);
                default:
                    return false;
            }
        }
        static UserData userData;

        public static SharedRobloxUserData SharedRobloxUserData
        {
            get
            {
                UserData _userData = UserData;
                if (_userData.sharedRobloxUserData == null)
                    _userData.sharedRobloxUserData = new SharedRobloxUserData();

                return _userData.sharedRobloxUserData;
            }
        }
        
        public static FormationProgress FormationProgress
        {
            get
            {
                UserData _userData = UserData;
                if (_userData.formationProgress == null)
                    _userData.formationProgress = new FormationProgress();

                if (_userData.formationProgress.fieldUnits == null || _userData.formationProgress.fieldUnits.Count == 0)
                    _userData.formationProgress.SetupDefaultFormation();

                return _userData.formationProgress;
            }
            set
            {
                UserData _userData = UserData;
                _userData.formationProgress = value ?? new FormationProgress();

                if (_userData.formationProgress.fieldUnits == null || _userData.formationProgress.fieldUnits.Count == 0)
                    _userData.formationProgress.SetupDefaultFormation();

                SaveData();
            }
        }




        #region User Data

        public static UserData UserData
        {
            get
            {
                if (userData == null)
                    userData = CreateDefaultUserData();
                else
                    userData = NormalizeUserData(userData);

                return userData;
            }
            private set => userData = NormalizeUserData(value);
        }
        public static void SetUserData(UserData _data)
        {
            UserData = _data;
            SaveData();
        }
        static UserDataManager()
        {
            UserData = CreateDefaultUserData();

           
        }
        static UserData CreateDefaultUserData()
        {
            return NormalizeUserData(new UserData());
        }
        static UserData NormalizeUserData(UserData userData)
        {
            if (userData == null)
                userData = new UserData();

            if (userData.sharedRobloxUserData == null)
                userData.sharedRobloxUserData = new SharedRobloxUserData();

            if (userData.formationProgress == null)
                userData.formationProgress = new FormationProgress();

      

            if (userData.formationProgress.fieldUnits == null || userData.formationProgress.fieldUnits.Count == 0)
                userData.formationProgress.SetupDefaultFormation();

            return userData;
        }
        public static void SaveData()
        {
            // Playable flow: keep progress in memory only, no persistent JSON save.
        }

        #endregion
    }

    [Serializable]
    public class UserData
    {
        public SharedRobloxUserData sharedRobloxUserData;
        
      
        public FormationProgress formationProgress;
     
        public UserData()
        {
            sharedRobloxUserData = new SharedRobloxUserData();
            
           
            formationProgress = new FormationProgress();
          
        }
    }
}
