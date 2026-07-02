using ExampleProject.Gameplay.Currency;
using ExampleProject.Manager;
using ExampleProject.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
namespace ExampleProject.Achievement
{
    [CreateAssetMenu(fileName = "Achievements", menuName = "ScriptableObjects/Achievement/Achievements")]
    public class Achievements : ScriptableObject
    {
        #region Fields
        [SerializeField] List<AchievementData> resourceDataList = new List<AchievementData>();        [SerializeField] string dataFolderPath;
        const string resourceFolderPath = "Data/Achievements";
        readonly static ResourceLoader<Achievements> resourceLoader = new ResourceLoader<Achievements>(resourceFolderPath);
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
            AchievementData[] _loadedData = Resources.LoadAll<AchievementData>(_cleanPath);
            if (_loadedData.Length > 0)
            {
                resourceDataList.AddRange(_loadedData);
                resourceDataList = resourceDataList.OrderBy(x => x.id).ToList();
            }
        }
        #endregion
        #region Public Methods
        public static List<AchievementData> GetResourceDataList()
        {
            return resourceLoader.Resource.resourceDataList;
        }
        public static AchievementData GetResourceData(AchievementId _id)
        {
            var _data = GetResourceDataList().Find(x => x.id.Equals(_id));
            return _data;
        }
        public static List<AchievementId> GetAllAchievementIds()
        {
            return GetResourceDataList().Select(x => x.id).ToList();
        }
        #endregion
    }
}