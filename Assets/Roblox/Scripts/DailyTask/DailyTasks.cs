using ExampleProject.Achievement;
using ExampleProject.DailyTask;
using ExampleProject.Manager;
using ExampleProject.Tools;
using ExampleProject.UI.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace ExampleProject.DailyTask
{
    [CreateAssetMenu(fileName = "DailyTasks", menuName = "ScriptableObjects/DailyTask/DailyTasks")]
    public class DailyTasks : ScriptableObject
    {
        #region Fields
        [SerializeField] List<DailyTaskData> resourceDataList = new List<DailyTaskData>();        [SerializeField] List<DailyTaskRewardData> weeklyReward = new List<DailyTaskRewardData>();        [SerializeField] List<DailyTaskRewardData> dailyReward = new List<DailyTaskRewardData>();        const string resourceFolderPath = "Data/DailyTasks";
        readonly static ResourceLoader<DailyTasks> resourceLoader = new ResourceLoader<DailyTasks>(resourceFolderPath);
        [SerializeField] string dataFolderPath;
        #endregion
        #region Properties
        #endregion
        #region LifeCycle   
        #endregion
        #region Private Methods
        void LoadAllData()
        {
            resourceDataList.Clear();
            // Ensure path is relative to Resources folder (strip Assets/Resources/ prefix if present)
            string _cleanPath = dataFolderPath.Replace("Assets/Resources/", "").Replace("\\", "/");
            DailyTaskData[] _loadedData = Resources.LoadAll<DailyTaskData>(_cleanPath);
            if (_loadedData.Length > 0)
            {
                resourceDataList.AddRange(_loadedData);
                resourceDataList = resourceDataList.OrderBy(x => x.id).ToList();
            }
        }
        #endregion
        #region Public Methods
        public static List<DailyTaskData> GetResourceDataList()
        {
            return resourceLoader.Resource.resourceDataList;
        }
        public static DailyTaskData GetResourceData(DailyTaskId _id)
        {
            var _data = GetResourceDataList().Find(x => x.id.Equals(_id));
            return _data;
        }
        public static List<DailyTaskRewardData> GetWeeklyRewardList()
        {
            return resourceLoader.Resource.weeklyReward;
        }
        public static List<DailyTaskRewardData> GetDailyRewardList()
        {
            return resourceLoader.Resource.dailyReward;
        }
        public static DailyTaskRewardData GetWeeklyReward(int _index)
        {
            return resourceLoader.Resource.weeklyReward[_index];
        }
        public static DailyTaskRewardData GetDailyReward(int _index)
        {
            return resourceLoader.Resource.dailyReward[_index];
        }
        #endregion
    }
    [Serializable]
    public class DailyTaskRewardData
    {
        public CurrencyValue reward;
        public int point;
    }
}