using ExampleProject.Achievement;
using ExampleProject.Gameplay.Characters.Skin;
using ExampleProject.GameSystem;
using ExampleProject.UI.BaseUI.BasePopup;
using ExampleProject.UI.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace ExampleProject.Manager
{
    [Serializable]
    public class AchievementProgress
    {
        #region Fields

        [SerializeField] List<OnUnlockingProgressAchievement> onUnlockingProgressAchievements = new List<OnUnlockingProgressAchievement>();
        #endregion

        #region Properties

        public static AchievementProgress Progress
        {
            get => SharedRobloxUserData.AchievementProgress;
            set => SharedRobloxUserData.AchievementProgress = value;
        }
        public static bool HasUnclaimedAchievement
        {
            get
            {
                foreach (var _progressAchievement in Progress.onUnlockingProgressAchievements)
                {
                    var _achievementData = Achievements.GetResourceData(_progressAchievement.achievementId);
                    if (_achievementData == null) continue;
                    if (_progressAchievement.IsClaimLastTarget) continue;
                    var _currentTargetData = _achievementData.targets[_progressAchievement.currentTarget];
                    if (_progressAchievement.CurrentProgressValue() >= _currentTargetData.targetValue)
                        return true;
                }
                return false;
            }
        }

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public static void ProcessAchievementProgress(AchievementId _achievementId, int _index, int _progress)
        {
            if (Progress.onUnlockingProgressAchievements.Any(_x => _x.achievementId == _achievementId))
            {
                // If the achievements is already in progress, update its progress
                var _progressAchievement = Progress.onUnlockingProgressAchievements.First(_x => _x.achievementId == _achievementId);
                _progressAchievement.UpdateProgress(_index, _progress);
            }
            else
            {
                // If the achievement is not in progress, create a new progress achievement
                var _newProgressAchievement = new OnUnlockingProgressAchievement(_achievementId);
                _newProgressAchievement.UpdateProgress(_index, _progress);
                Progress.onUnlockingProgressAchievements.Add(_newProgressAchievement);
            }
            EventDispatcher.Instance.Dispatch(EventName.OnProcessAchievementProgress, _achievementId);

            UserDataManager.SaveData();
        }
        public static void ProcessAchievementAllProgress(AchievementId _achievementId, int _progress)
        {
            if (Progress.onUnlockingProgressAchievements.Any(_x => _x.achievementId == _achievementId))
            {
                // If the achievements is already in progress, update its progress
                var _progressAchievement = Progress.onUnlockingProgressAchievements.First(_x => _x.achievementId == _achievementId);
                _progressAchievement.UpdateAllProgress(_progress);
            }
            else
            {
                // If the achievement is not in progress, create a new progress achievement
                var _newProgressAchievement = new OnUnlockingProgressAchievement(_achievementId);
                _newProgressAchievement.UpdateAllProgress(_progress);
                Progress.onUnlockingProgressAchievements.Add(_newProgressAchievement);
            }
            EventDispatcher.Instance.Dispatch(EventName.OnProcessAchievementProgress, _achievementId);

            UserDataManager.SaveData();
        }
        public static OnUnlockingProgressAchievement GetProgressAchievement(AchievementId _achievementId)
        {
            if (Progress.onUnlockingProgressAchievements.Any(_x => _x.achievementId == _achievementId))
                return Progress.onUnlockingProgressAchievements.First(_x => _x.achievementId == _achievementId);
            return new OnUnlockingProgressAchievement(_achievementId);
        }
        public static void ClaimAchievement(AchievementId _achievementId)
        {
            var _onUnlockingProgressAchievement = GetProgressAchievement(_achievementId);
            _onUnlockingProgressAchievement.currentTarget++;
            EventDispatcher.Instance.Dispatch(EventName.OnProcessAchievementProgress, _achievementId);
        }
        public static Item GetCurrentReward(AchievementId _achievementId)
        {
            var _onUnlockingProgressAchievement = GetProgressAchievement(_achievementId);
            var _achievementData = Achievements.GetResourceData(_achievementId);
            return _achievementData.targets[_onUnlockingProgressAchievement.currentTarget].reward;
        }
        public static bool IsCanClaimAchievement(AchievementId _achievementId)
        {
            var _onUnlockingProgressAchievement = GetProgressAchievement(_achievementId);
            var _achievementData = Achievements.GetResourceData(_achievementId);    
            if (_onUnlockingProgressAchievement.IsClaimLastTarget)
                return false;
            var _currentTargetData = _achievementData.targets[_onUnlockingProgressAchievement.currentTarget];
            return _onUnlockingProgressAchievement.CurrentProgressValue() >= _currentTargetData.targetValue;
        }

        #endregion
    }

    [Serializable]
    public class OnUnlockingProgressAchievement
    {
        public AchievementId achievementId = AchievementId.None;
        public List<TargetProcess> targetsProgress = new List<TargetProcess>();        public int currentTarget;

        public bool IsClaimLastTarget => currentTarget >= Achievements.GetResourceData(achievementId).targets.Count;
        public void UpdateProgress(int _index, int _change)
        {
            var _data = Achievements.GetResourceData(achievementId);
            var _targetData = _data.targets;
            var _currentProcess = targetsProgress[_index];
            var _targetProcess = _targetData[_index];
            if (targetsProgress.Count < _targetData.Count)
            {
                // Add default values to reach the required count, preserving old values
                targetsProgress.AddRange(Enumerable.Repeat<TargetProcess>(new TargetProcess(), _targetData.Count - targetsProgress.Count));
            }
            _currentProcess.currentValue += _change;
           
       
        }
        public void UpdateAllProgress(int _change)
        {
            for (int i = 0; i < targetsProgress.Count; i++)
            {
                UpdateProgress(i, _change);
            }
        }
        public int CurrentProgressValue()
        {
            return targetsProgress[currentTarget].currentValue;
        }

        public OnUnlockingProgressAchievement(AchievementId _id)
        {
            var _resource = Achievements.GetResourceData(_id);
            if (_resource == null)
                return;

            achievementId = _id;
            targetsProgress = new List<TargetProcess>();
            int _targetCount = _resource.targets.Count;
            for (int i = 0; i < _targetCount; i++)
            {
                targetsProgress.Add(new TargetProcess());
            }
            currentTarget = 0;
        }

        [Serializable]
        public class TargetProcess
        {
            public int currentValue;
            public bool hasShownNotify;
        }
    }

}
