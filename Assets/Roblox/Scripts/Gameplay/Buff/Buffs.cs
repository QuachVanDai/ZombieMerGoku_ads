using ExampleProject.Gameplay.Faction;

using ExampleProject.Manager;
using ExampleProject.Tools;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace ExampleProject.Gameplay.Buffs
{
    [CreateAssetMenu(fileName = "Buffs", menuName = "ScriptableObjects/Buff/Buffs")]
    public class Buffs : ScriptableObject
    {
        #region Fields
        [SerializeField] List<BuffData> resourceDataList = new List<BuffData>();        [SerializeField] string dataFolderPath;
        const string resourceFolderPath = "Data/Buffs";
        const string fallbackDataFolderPath = "Data/BuffData";
        readonly static ResourceLoader<Buffs> resourceLoader = new ResourceLoader<Buffs>(resourceFolderPath);
        static List<BuffData> fallbackResourceDataList;
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
            BuffData[] _loadedData = Resources.LoadAll<BuffData>(_cleanPath);
            if (_loadedData.Length > 0)
            {
                resourceDataList.AddRange(_loadedData);
                resourceDataList = resourceDataList.OrderBy(x => x.type).ToList();
            }
        }
        #endregion
        #region Public Methods
        public static List<BuffData> GetResourceDataList()
        {
            if (PlayableDataRegistry.Instance != null)
            {
                List<BuffData> registryData = PlayableDataRegistry.Instance.GetBuffDataList();
                if (registryData.Count > 0)
                    return registryData;
            }

            Buffs _resource = resourceLoader.Resource;
            if (_resource != null && _resource.resourceDataList != null && _resource.resourceDataList.Count > 0)
                return _resource.resourceDataList;

            if (fallbackResourceDataList == null)
            {
                fallbackResourceDataList = Resources.LoadAll<BuffData>(fallbackDataFolderPath)
                    .Where(x => x != null)
                    .OrderBy(x => x.type)
                    .ToList();
            }

            return fallbackResourceDataList;
        }
        public static BuffData GetResourceData(BuffType _type)
        {
            if (PlayableDataRegistry.Instance != null)
            {
                BuffData registryData = PlayableDataRegistry.Instance.GetBuffData(_type);
                if (registryData != null)
                    return registryData;
            }

            var _data = GetResourceDataList().Find(x => x != null && x.type.Equals(_type));
            return _data;
        }
        #endregion
    }
}
