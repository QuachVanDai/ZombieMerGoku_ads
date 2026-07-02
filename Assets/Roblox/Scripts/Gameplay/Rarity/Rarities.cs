 using ExampleProject.Manager;
using ExampleProject.Tools;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace ExampleProject.Gameplay.Rarity
{
    [CreateAssetMenu(fileName = "Rarities", menuName = "ScriptableObjects/Rarity/Rarities")]
    public class Rarities : ScriptableObject
    {
        #region Fields
        [SerializeField] List<RarityData> resourceDataList = new List<RarityData>();        [SerializeField] string dataFolderPath;
        const string resourceFolderPath = "Data/Rarities";
        const string fallbackDataFolderPath = "Data/RaritiyData";
        readonly static ResourceLoader<Rarities> resourceLoader = new ResourceLoader<Rarities>(resourceFolderPath);
        static List<RarityData> fallbackResourceDataList;
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
            RarityData[] _loadedData = Resources.LoadAll<RarityData>(_cleanPath);
            if (_loadedData.Length > 0)
            {
                resourceDataList.AddRange(_loadedData);
                resourceDataList = resourceDataList.OrderBy(x => x.rarity).ToList();
            }
        }
        #endregion
        #region Public Methods
        public static List<RarityData> GetResourceDataList()
        {
            if (PlayableDataRegistry.Instance != null)
            {
                List<RarityData> registryData = PlayableDataRegistry.Instance.GetRarityDataList();
                if (registryData.Count > 0)
                    return registryData;
            }

            Rarities _resource = resourceLoader.Resource;
            if (_resource != null && _resource.resourceDataList != null && _resource.resourceDataList.Count > 0)
                return _resource.resourceDataList;

            if (fallbackResourceDataList == null)
            {
                fallbackResourceDataList = Resources.LoadAll<RarityData>(fallbackDataFolderPath)
                    .Where(x => x != null)
                    .OrderBy(x => x.rarity)
                    .ToList();
            }

            return fallbackResourceDataList;
        }
        public static RarityData GetResourceData(RarityId _rarity)
        {
            if (PlayableDataRegistry.Instance != null)
            {
                RarityData registryData = PlayableDataRegistry.Instance.GetRarityData(_rarity);
                if (registryData != null)
                    return registryData;
            }

            var _data = GetResourceDataList().Find(x => x != null && x.rarity.Equals(_rarity));
            return _data;
        }
        #endregion      
    }
    public enum RarityId
    {
        None = 0,
        Common = 1,
        Rare = 2,
        SuperRare = 3,
        Epic = 4,
        Legendary = 5,
        Mythical = 6,
        Shadow = 7,
        Inferno = 8,
    }
}
