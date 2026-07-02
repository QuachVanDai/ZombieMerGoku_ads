using BreakInfinity;
using ExampleProject.Gameplay.Characters.Skin;
using ExampleProject.GameSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExampleProject.Manager
{
    [Serializable]
    public class SkinProgress
    {
        #region Fields

        [SerializeField] List<SkinId> unlockedSkins = new List<SkinId>();        [SerializeField] List<OnUnlockingProgressSkin> onUnlockingProgressSkins = new List<OnUnlockingProgressSkin>();
        #endregion

        #region Properties

        public static SkinProgress Progress
        {
            get => SharedRobloxUserData.SkinProgress;
            set => SharedRobloxUserData.SkinProgress = value;
        }
        public static List<SkinId> UnlockedSkins
        {
            get
            {
                var _result = new List<SkinId>(Progress.unlockedSkins);
                _result.AddRange(Skins.GetFreeSkins());
                return _result;
            }
        }


        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public static void UnlockSkin(SkinId _skinId, bool _isPlaySound = true)
        {
            if (Progress.unlockedSkins.Contains(_skinId))
                return;

            if (_isPlaySound)
                SoundSystem.Instance.PlayUnlock();
            Progress.unlockedSkins.Add(_skinId);
            if (Progress.onUnlockingProgressSkins.Any(_x => _x.skinId == _skinId))
            {
                var _progressSkin = Progress.onUnlockingProgressSkins.First(_x => _x.skinId == _skinId);
                Progress.onUnlockingProgressSkins.Remove(_progressSkin);
            }
            EventDispatcher.Instance.Dispatch(EventName.OnUnlockSkin, _skinId);
            UserDataManager.SaveData();

            // Process achievement
            //AchievementProgress.ProcessAchievementAllProgress(AchievementId.UnlockSkin, 1);

            // Equip
            PlayerEquipment.EquipSkin(_skinId);
        }
        public static bool IsUnlocked(SkinId _skinId)
        {
            // Default skin is always unlocked
            if (_skinId == SkinId.Steve)
                return true;
            return Progress.unlockedSkins.Contains(_skinId);
        }
        public static void ProcessSkinProgress(SkinId _skinId, int _progress)
        {
            if (Progress.onUnlockingProgressSkins.Any(_x => _x.skinId == _skinId))
            {
                // If the skin is already in progress, update its progress
                var _progressSkin = Progress.onUnlockingProgressSkins.First(_x => _x.skinId == _skinId);
                _progressSkin.currentProgress += _progress;

                // Check if the progress skin is completed
                if (_progressSkin.IsCompleted)
                {
                    UnlockSkin(_skinId);
                    Progress.onUnlockingProgressSkins.Remove(_progressSkin);
                }
            }
            else
            {
                // If the skin is not in progress, create a new progress skin
                var _newProgressSkin = new OnUnlockingProgressSkin
                {
                    skinId = _skinId,
                    currentProgress = _progress
                };
                Progress.onUnlockingProgressSkins.Add(_newProgressSkin);

                // Check if the new progress skin is completed
                if (_newProgressSkin.IsCompleted)
                {
                    UnlockSkin(_skinId);
                    Progress.onUnlockingProgressSkins.Remove(_newProgressSkin);
                }
            }
            EventDispatcher.Instance.Dispatch(EventName.OnProcessSkinProgress, null);
            UserDataManager.SaveData();
        }
        public static int GetSkinProgress(SkinId _skinId)
        {
            var _progressSkin = Progress.onUnlockingProgressSkins.FirstOrDefault(_x => _x.skinId == _skinId);
            return _progressSkin != null ? _progressSkin.currentProgress : 0;
        }

        #endregion
    }

    [Serializable]
    public class OnUnlockingProgressSkin
    {
        public SkinId skinId;

        public int currentProgress;

        public bool IsCompleted => currentProgress >= MaxProgress;

        public BigDouble MaxProgress => Skins.GetResourceData(skinId).unlockCondition.value;
    }

}