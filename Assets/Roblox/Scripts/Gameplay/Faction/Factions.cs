using ExampleProject.Gameplay.Scenes;
using ExampleProject.Manager;
using ExampleProject.Tools;
using ExampleProject.UI.BaseUI.BasePopup;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace ExampleProject.Gameplay.Faction
{
    [CreateAssetMenu(fileName = "Factions", menuName = "ScriptableObjects/Faction/Factions")]
    public class Factions : ScriptableObject
    {
        #region Fields
        [SerializeField] List<FactionData> resourceDataList = new List<FactionData>();        [SerializeField] string dataFolderPath;
        const string resourceFolderPath = "Data/Factions";
        const string fallbackDataFolderPath = "Data/FactionData";
        readonly static ResourceLoader<Factions> resourceLoader = new ResourceLoader<Factions>(resourceFolderPath);
        static List<FactionData> fallbackResourceDataList;
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
            FactionData[] _loadedData = Resources.LoadAll<FactionData>(_cleanPath);
            if (_loadedData.Length > 0)
            {
                resourceDataList.AddRange(_loadedData);
                resourceDataList = resourceDataList.OrderBy(x => x.id).ToList();
            }
        }
        #endregion
        #region Public Methods
        public static List<FactionData> GetResourceDataList()
        {
            if (PlayableDataRegistry.Instance != null)
            {
                List<FactionData> registryData = PlayableDataRegistry.Instance.GetFactionDataList();
                if (registryData.Count > 0)
                    return registryData;
            }

            Factions _resource = resourceLoader.Resource;
            if (_resource != null && _resource.resourceDataList != null && _resource.resourceDataList.Count > 0)
                return _resource.resourceDataList;

            if (fallbackResourceDataList == null)
            {
                fallbackResourceDataList = Resources.LoadAll<FactionData>(fallbackDataFolderPath)
                    .Where(x => x != null)
                    .OrderBy(x => x.id)
                    .ToList();
            }

            return fallbackResourceDataList;
        }
        public static FactionData GetResourceData(FactionId _id)
        {
            if (PlayableDataRegistry.Instance != null)
            {
                FactionData registryData = PlayableDataRegistry.Instance.GetFactionData(_id);
                if (registryData != null)
                    return registryData;
            }

            var _data = GetResourceDataList().Find(x => x != null && x.id.Equals(_id));
            return _data;
        }
        #endregion
    }
}
