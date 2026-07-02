using BreakInfinity;

using ExampleProject.Gameplay.Wing;
using ExampleProject.GameSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExampleProject.Manager
{
    [Serializable]
    public class WingProgress
    {
        #region Fields

        [SerializeField] List<WingId> unlockedWings = new List<WingId>();        [SerializeField] List<OnUnlockingProgressWing> onUnlockingProgressWings = new List<OnUnlockingProgressWing>();
        #endregion

        #region Properties

        public static WingProgress Progress
        {
            get => SharedRobloxUserData.WingProgress;
            set => SharedRobloxUserData.WingProgress = value;
        }
        public static List<WingId> UnlockedWings
        {
            get
            {
                var _result = new List<WingId>(Progress.unlockedWings);
                _result.AddRange(Wings.GetFreeWings());
                return _result;
            }
        }


        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public static void UnlockWing(WingId _wingId, bool _isPlaySound = true)
        {
            if (Progress.unlockedWings.Contains(_wingId))
                return;

            if (_isPlaySound)
                SoundSystem.Instance.PlayUnlock();
            Progress.unlockedWings.Add(_wingId);
            if (Progress.onUnlockingProgressWings.Any(_x => _x.wingId == _wingId))
            {
                var _progressWing = Progress.onUnlockingProgressWings.First(_x => _x.wingId == _wingId);
                Progress.onUnlockingProgressWings.Remove(_progressWing);
            }
            EventDispatcher.Instance.Dispatch(EventName.OnUnlockWing, _wingId);
           
            UserDataManager.SaveData();

            // Process achievement
            //AchievementProgress.ProcessAchievementAllProgress(AchievementId.UnlockWing, 1);

            // Equip
            PlayerEquipment.EquipWing(_wingId);
        }
        public static bool IsUnlocked(WingId _wingId)
        {
            // None wing is always unlocked
            if (_wingId == WingId.None)
                return true;
            return Progress.unlockedWings.Contains(_wingId);
        }
        public static void ProcessWingProgress(WingId _wingId, int _progress)
        {
            if (Progress.onUnlockingProgressWings.Any(_x => _x.wingId == _wingId))
            {
                // If the wing is already in progress, update its progress
                var _progressWing = Progress.onUnlockingProgressWings.First(_x => _x.wingId == _wingId);
                _progressWing.currentProgress += _progress;

                // Check if the progress wing is completed
                if (_progressWing.IsCompleted)
                {
                    UnlockWing(_wingId);
                    Progress.onUnlockingProgressWings.Remove(_progressWing);
                }
            }
            else
            {
                // If the wing is not in progress, create a new progress wing
                var _newProgressWing = new OnUnlockingProgressWing
                {
                    wingId = _wingId,
                    currentProgress = _progress
                };
                Progress.onUnlockingProgressWings.Add(_newProgressWing);

                // Check if the new progress wing is completed
                if (_newProgressWing.IsCompleted)
                {
                    UnlockWing(_wingId);
                    Progress.onUnlockingProgressWings.Remove(_newProgressWing);
                }
            }
            EventDispatcher.Instance.Dispatch(EventName.OnProcessWingProgress, null);
            UserDataManager.SaveData();
        }
        public static int GetWingProgress(WingId _wingId)
        {
            var _progressWing = Progress.onUnlockingProgressWings.FirstOrDefault(_x => _x.wingId == _wingId);
            return _progressWing != null ? _progressWing.currentProgress : 0;
        }

        #endregion
    }

    [Serializable]
    public class OnUnlockingProgressWing
    {
        public WingId wingId;

        public int currentProgress;

        public bool IsCompleted => currentProgress >= MaxProgress;

        public int MaxProgress => Wings.GetResourceData(wingId).unlockCondition.value;
    }

}
