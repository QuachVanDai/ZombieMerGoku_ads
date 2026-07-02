using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExampleProject.Gameplay.Rarity;
using ExampleProject.Gameplay.Unit;
using ExampleProject.Manager;
using ExampleProject.Tools;
using UnityEngine;
using Units = ExampleProject.Gameplay.Unit.Units;
namespace ExampleProject
{
    [CreateAssetMenu(fileName = "TactixChests ", menuName = "ScriptableObjects/Tactix/Chests")]
    public class TactixChests : ScriptableObject
    {
         #region Fields
        [SerializeField] List<TactixChestData> chestResourceDataList = new List<TactixChestData>();
        [SerializeField] string tactixChestsDataFolderPath;
        const string resourceFolderPath = "Data/Tactix/TactixChests";
        const string fallbackChestDataFolderPath = "Data/Tactix/TactixChestData";
        readonly static ResourceLoader<TactixChests> resourceLoader = new ResourceLoader<TactixChests>(resourceFolderPath);
        static List<TactixChestData> fallbackChestResourceDataList;
        #endregion
        #region Properties
        #endregion
        #region LifeCycle   
        #endregion
        #region Private Methods
        void LoadAllData()
        {
            LoadData(chestResourceDataList, tactixChestsDataFolderPath);
        }
        void LoadData(List<TactixChestData> _list, string _folderPath)
        {
            _list.Clear();
            // Ensure path is relative to Resources folder (strip Assets/Resources/ prefix if present)
            string _cleanPath = _folderPath.Replace("Assets/Resources/", "").Replace("\\", "/");
            TactixChestData[] _loadedData = Resources.LoadAll<TactixChestData>(_cleanPath);
            if (_loadedData.Length > 0)
            {
                _list.AddRange(_loadedData);
                _list = _list.OrderBy(x => x.chestId).ToList();
            }
        }
        #endregion
        #region Public Methods
        public static List<TactixChestData> GetChestResourceDataList()
        {
            if (PlayableDataRegistry.Instance != null)
            {
                List<TactixChestData> registryData = PlayableDataRegistry.Instance.GetChestDataList();
                if (registryData.Count > 0)
                    return registryData;
            }

            TactixChests _resource = resourceLoader.Resource;
            if (_resource != null && _resource.chestResourceDataList != null && _resource.chestResourceDataList.Count > 0)
                return _resource.chestResourceDataList;

            if (fallbackChestResourceDataList == null)
            {
                fallbackChestResourceDataList = Resources.LoadAll<TactixChestData>(fallbackChestDataFolderPath)
                    .Where(x => x != null)
                    .OrderBy(x => x.chestId)
                    .ToList();
            }

            return fallbackChestResourceDataList;
        }
        public static TactixChestData GetChestResourceData(ChestId _id)
        {
            if (PlayableDataRegistry.Instance != null)
            {
                TactixChestData registryData = PlayableDataRegistry.Instance.GetChestData(_id);
                if (registryData != null)
                    return registryData;
            }

            var _data = GetChestResourceDataList().Find(x => x != null && x.chestId.Equals(_id));
            return _data;
        }
        public static TactixChestData GetChestResourceDataByRarity(RarityId _rarity)
        {
            if (PlayableDataRegistry.Instance != null)
            {
                TactixChestData registryData = PlayableDataRegistry.Instance.GetChestDataByRarity(_rarity);
                if (registryData != null)
                    return registryData;
            }

            var _data = GetChestResourceDataList().Find(x => x != null && x.rarity.Equals(_rarity));
            return _data;
        }
        public static TactixChestData GetChestResourceDataByUnitID(UnitId _unitId)
        {
            UnitData _unitData = Units.GetCreepResourceData(_unitId);
            if (_unitData == null)
                return null;

            RarityId _rarity = _unitData.rarity;
            return GetChestResourceDataByRarity(_rarity);
        }
        public static List<TactixChestData> GetChestsResourceDataById(ChestId _chestId)
        {
            List<TactixChestData> _chestDataList = GetChestResourceDataList().Where(x => x != null && x.chestId.Equals(_chestId)).ToList();
            return _chestDataList;
        }
        public static RarityId GetRandomChestRarityByUnitID(RarityId _rarity)
        {
            TactixChestData _chestData = GetChestResourceDataByRarity(_rarity);
            if (_chestData == null || _chestData.rarityChancesList == null || _chestData.rarityChancesList.Count == 0)
                return RarityId.None;
            float totalWeight = 0f;
            foreach (var item in _chestData.rarityChancesList)
            {
                totalWeight += item.chance;
            }
            if (totalWeight <= 0f)
                return RarityId.None;
            float rand = UnityEngine.Random.value * totalWeight;
            float current = 0f;
            foreach (var item in _chestData.rarityChancesList)
            {
                current += item.chance;
                if (rand <= current)
                {
                    return item.rarity;
                }
            }
            return _chestData.rarityChancesList[_chestData.rarityChancesList.Count - 1].rarity;
        }
        public static GameObject GetChest3DModel(ChestId _chestId)
        {
            TactixChestData _data = GetChestResourceData(_chestId);
            if (_data == null || _data.chestPrefab == null)
                return null;

            var _model = Instantiate(_data.chestPrefab, Vector3.zero, Quaternion.identity) as BaseTactixChest;
            if (_model == null)
                return null;

          //  _model.SetChestId(_chestId);
            _model.HideFloatingCanvas();
            return _model.gameObject;
        }
        #endregion      
    }
}
