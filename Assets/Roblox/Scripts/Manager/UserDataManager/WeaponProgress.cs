using BreakInfinity;

using ExampleProject.Gameplay.Characters.Skin;
using ExampleProject.Gameplay.Weapon;
using ExampleProject.Gameplay.Wing;
using ExampleProject.GameSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExampleProject.Manager
{
    [Serializable]
    public class WeaponProgress
    {
        #region Fields

        [SerializeField] List<WeaponId> unlockedWeapons = new List<WeaponId>();        [SerializeField] List<OnUnlockingProgressWeapon> onUnlockingProgressWeapons = new List<OnUnlockingProgressWeapon>();
        #endregion

        #region Properties

        public static WeaponProgress Progress
        {
            get => SharedRobloxUserData.WeaponProgress;
            set => SharedRobloxUserData.WeaponProgress = value;
        }
        public static List<WeaponId> UnlockedWeapons
        {
            get
            {
                var _result = new List<WeaponId>(Progress.unlockedWeapons);
                _result.AddRange(Weapons.GetFreeWeapons());
                return _result;
            }
        }

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public static void UnlockWeapon(WeaponId _weaponId, bool _isPlaySound = true)
        {
            if (Progress.unlockedWeapons.Contains(_weaponId))
                return;

            if (_isPlaySound)
                SoundSystem.Instance.PlayUnlock();
            Progress.unlockedWeapons.Add(_weaponId);
            if (Progress.onUnlockingProgressWeapons.Any(_x => _x.weaponId == _weaponId))
            {
                var _progressWeapon = Progress.onUnlockingProgressWeapons.First(_x => _x.weaponId == _weaponId);
                Progress.onUnlockingProgressWeapons.Remove(_progressWeapon);
            }
            EventDispatcher.Instance.Dispatch(EventName.OnUnlockWeapon, _weaponId);
     
            UserDataManager.SaveData();

            // Process achievement
            //AchievementProgress.ProcessAchievementAllProgress(AchievementId.UnlockWeapon, 1);

            // Equip
            PlayerEquipment.EquipWeapon(_weaponId);
        }
        public static bool IsUnlocked(WeaponId _weaponId)
        {
            // None weapon is always unlocked
            if (_weaponId == WeaponId.None)
                return true;
            return Progress.unlockedWeapons.Contains(_weaponId);
        }
        public static void ProcessWeaponProgress(WeaponId _weaponId, int _progress)
        {
            if (Progress.onUnlockingProgressWeapons.Any(_x => _x.weaponId == _weaponId))
            {
                // If the weapon is already in progress, update its progress
                var _progressWeapon = Progress.onUnlockingProgressWeapons.First(_x => _x.weaponId == _weaponId);
                _progressWeapon.currentProgress += _progress;

                // Check if the progress weapon is completed
                if (_progressWeapon.IsCompleted)
                {
                    UnlockWeapon(_weaponId);
                    Progress.onUnlockingProgressWeapons.Remove(_progressWeapon);
                }
            }
            else
            {
                // If the weapon is not in progress, create a new progress weapon
                var _newProgressWeapon = new OnUnlockingProgressWeapon
                {
                    weaponId = _weaponId,
                    currentProgress = _progress
                };
                Progress.onUnlockingProgressWeapons.Add(_newProgressWeapon);

                // Check if the new progress weapon is completed
                if (_newProgressWeapon.IsCompleted)
                {
                    UnlockWeapon(_weaponId);
                    Progress.onUnlockingProgressWeapons.Remove(_newProgressWeapon);
                }
            }
            EventDispatcher.Instance.Dispatch(EventName.OnProcessWeaponProgress, null);
            UserDataManager.SaveData();
        }
        public static int GetWeaponProgress(WeaponId _weaponId)
        {
            var _progressWeapon = Progress.onUnlockingProgressWeapons.FirstOrDefault(_x => _x.weaponId == _weaponId);
            return _progressWeapon != null ? _progressWeapon.currentProgress : 0;
        }

        #endregion
    }

    [Serializable]
    public class OnUnlockingProgressWeapon
    {
        public WeaponId weaponId;

        public int currentProgress;

        public bool IsCompleted => currentProgress >= MaxProgress;

        public int MaxProgress => Weapons.GetResourceData(weaponId).unlockCondition.value;
    }

}
