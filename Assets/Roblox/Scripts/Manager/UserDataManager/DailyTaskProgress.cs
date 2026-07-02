using ExampleProject.Achievement;
using ExampleProject.DailyTask;
using ExampleProject.GameSystem;
using ExampleProject.Tools;
using ExampleProject.UI.BaseUI.BasePopup;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExampleProject.Manager
{
    [Serializable]
    public class AllDailyTaskProgress
    {
        #region Fields

        [SerializeField] List<DailyTaskProgress> onProcessDailyTasks = new List<DailyTaskProgress>();        [SerializeField] int weeklyPoint;
        [SerializeField] int dailyPoint;
        [SerializeField] List<int> claimedWeeklyRewardIndex = new List<int>();        [SerializeField] List<int> claimedDailyRewardIndex = new List<int>();
        #endregion

        #region Properties

        public static AllDailyTaskProgress Progress
        {
            get => SharedRobloxUserData.AllDailyTaskProgress;
            set => SharedRobloxUserData.AllDailyTaskProgress = value;
        }

        public static int WeeklyPoint
        {
            get => Progress.weeklyPoint;
            set
            {
                Progress.weeklyPoint = value;
                UserDataManager.SaveData();
            }
        }

        public static int DailyPoint
        {
            get => Progress.dailyPoint;
            set
            {
                Progress.dailyPoint = value;
                UserDataManager.SaveData();
            }
        }

        public static bool IsHaveNotification
        {
            get { return IsCanClaimAnyWeeklyReward() || IsCanClaimAnyDailyReward() || IsCanClaimAnyDailyTask(); }
        }

        #endregion

        #region LifeCycle

        public AllDailyTaskProgress()
        {
            onProcessDailyTasks = new List<DailyTaskProgress>();
            weeklyPoint = 0;
            dailyPoint = 0;
            claimedWeeklyRewardIndex = new List<int>();
            claimedDailyRewardIndex = new List<int>();
        }

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        public static void ProcessDailyTaskProgress(DailyTaskId _id, int _progress)
        {
            if (Progress.onProcessDailyTasks.Any(_x => _x.id == _id))
            {
                var _task = Progress.onProcessDailyTasks.First(_x => _x.id == _id);
                _task.UpdateProgress(_progress);
            }
            else
            {
                var _task = new DailyTaskProgress
                {
                    id = _id
                };
                Progress.onProcessDailyTasks.Add(_task);
                _task.UpdateProgress(_progress);
            }

            EventDispatcher.Instance.Dispatch(EventName.OnProcessDailyTaskProgress, null);
            UserDataManager.SaveData();
        }

        public static bool IsClaimedWeeklyReward(int _index)
        {
            return Progress.claimedWeeklyRewardIndex.Contains(_index);
        }

        public static bool IsClaimedDailyReward(int _index)
        {
            return Progress.claimedDailyRewardIndex.Contains(_index);
        }

        public static bool IsCanClaimWeeklyReward(int _index)
        {
            var _reward = DailyTasks.GetWeeklyReward(_index);
            if (_reward == null)
                return false;
            return WeeklyPoint >= _reward.point && !IsClaimedWeeklyReward(_index);
        }

        public static bool IsCanClaimDailyReward(int _index)
        {
            var _reward = DailyTasks.GetDailyReward(_index);
            if (_reward == null)
                return false;
            return DailyPoint >= _reward.point && !IsClaimedDailyReward(_index);
        }

        public static void ClaimWeeklyReward(int _index)
        {
            if (!IsCanClaimWeeklyReward(_index))
                return;
            var _reward = DailyTasks.GetWeeklyReward(_index);
            if (_reward == null)
                return;
            CurrencyProgress.AddCurrency(_reward.reward, StringsSafeAccess.LOG_DAILY_TASK, 0);
            Progress.claimedWeeklyRewardIndex.Add(_index);
            EventDispatcher.Instance.Dispatch(EventName.OnProcessDailyTaskProgress, null);
            UserDataManager.SaveData();
        }

        public static void ClaimDailyReward(int _index)
        {
            if (!IsCanClaimDailyReward(_index))
                return;
            var _reward = DailyTasks.GetDailyReward(_index);
            if (_reward == null)
                return;
            CurrencyProgress.AddCurrency(_reward.reward, StringsSafeAccess.LOG_DAILY_TASK, 0);
            Progress.claimedDailyRewardIndex.Add(_index);
            EventDispatcher.Instance.Dispatch(EventName.OnProcessDailyTaskProgress, null);
            UserDataManager.SaveData();
        }

        public static void AddDailyPoint(int _point)
        {
            DailyPoint += _point;
            WeeklyPoint += _point;
            EventDispatcher.Instance.Dispatch(EventName.OnProcessDailyTaskProgress, null);
        }

        public static int GetDailyTaskCurrentProgress(DailyTaskId _id)
        {
            if (Progress.onProcessDailyTasks.Any(_x => _x.id == _id))
            {
                return Progress.onProcessDailyTasks.First(_x => _x.id == _id).currentProgress;
            }

            return 0;
        }

        public static bool IsDailyTaskComplete(DailyTaskId _id)
        {
            if (Progress.onProcessDailyTasks.Any(_x => _x.id == _id))
            {
                return Progress.onProcessDailyTasks.First(_x => _x.id == _id).IsComplete && Progress.onProcessDailyTasks.First(_x => _x.id == _id).isClaimed;
            }

            return false;
        }

        public static bool IsCanClaimDailyTask(DailyTaskId _id)
        {
            if (Progress.onProcessDailyTasks.Any(_x => _x.id == _id))
            {
                return Progress.onProcessDailyTasks.First(_x => _x.id == _id).IsComplete && !Progress.onProcessDailyTasks.First(_x => _x.id == _id).isClaimed;
            }

            return false;
        }

        public static void ClaimDailyTask(DailyTaskId _id)
        {
            if (!IsCanClaimDailyTask(_id))
                return;

            SoundSystem.Instance.PlayCoinClaim();

            var _task = Progress.onProcessDailyTasks.First(_x => _x.id == _id);
            _task.isClaimed = true;

            var _point = DailyTasks.GetResourceData(_id).point;
            AddDailyPoint(_point);
            UserDataManager.SaveData();
        }

        public static void ResetDailyTasks()
        {
            Progress.onProcessDailyTasks.Clear();
            Progress.dailyPoint = 0;
            Progress.claimedDailyRewardIndex.Clear();
            UserDataManager.SaveData();
        }

        public static void ResetWeeklyTasks()
        {
            Progress.weeklyPoint = 0;
            Progress.claimedWeeklyRewardIndex.Clear();
            UserDataManager.SaveData();
        }

        static bool IsCanClaimAnyWeeklyReward()
        {
            for (int i = 0; i < DailyTasks.GetWeeklyRewardList().Count; i++)
            {
                if (IsCanClaimWeeklyReward(i))
                    return true;
            }

            return false;
        }

        static bool IsCanClaimAnyDailyReward()
        {
            for (int i = 0; i < DailyTasks.GetDailyRewardList().Count; i++)
            {
                if (IsCanClaimDailyReward(i))
                    return true;
            }

            return false;
        }

        static bool IsCanClaimAnyDailyTask()
        {
            foreach (var _task in Progress.onProcessDailyTasks)
            {
                if (_task.IsComplete && !_task.isClaimed)
                    return true;
            }

            return false;
        }

        #endregion
    }

    [Serializable]
    public class DailyTaskProgress
    {
        public DailyTaskId id;
        public int currentProgress;
        public bool isClaimed;

        public bool IsComplete
        {
            get
            {
                var _max = DailyTasks.GetResourceData(id).GetTarget();
                return currentProgress >= _max;
            }
        }

        public void UpdateProgress(int _process)
        {
            var _max = DailyTasks.GetResourceData(id).GetTarget();
            if (!IsComplete)
                currentProgress = Mathf.Min(currentProgress + _process, _max);
            else
                return;

            if (IsComplete)
            {
                var _content = DailyTasks.GetResourceData(id).GetLocalizedDescription();
              
            }
        }
    }
}
