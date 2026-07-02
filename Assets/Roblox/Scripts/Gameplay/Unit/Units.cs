using BreakInfinity;
using ExampleProject.Gameplay.Characters.Skin;
using ExampleProject.Manager;
using ExampleProject.Tools;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExampleProject.Gameplay.Unit
{
    [CreateAssetMenu(fileName = "Units", menuName = "ScriptableObjects/Unit/Units")]
    public class Units : ScriptableObject
    {
        #region Fields

        [SerializeField] List<UnitData> creepResourceDataList = new List<UnitData>();        [SerializeField] List<UnitData> towerResourceDataList = new List<UnitData>();        [SerializeField] List<UnitData> bossResourceDataList = new List<UnitData>();
        [SerializeField] string creepDataFolderPath;
        [SerializeField] string towerDataFolderPath;
        [SerializeField] string bossDataFolderPath;

        const string resourceFolderPath = "Data/Units";
        const string fallbackCreepDataFolderPath = "Data/UnitData/Creeps";
        const string fallbackTowerDataFolderPath = "Data/UnitData/Towers";
        const string fallbackBossDataFolderPath = "Data/UnitData/Bosses";
        readonly static ResourceLoader<Units> resourceLoader = new ResourceLoader<Units>(resourceFolderPath);
        static List<UnitData> fallbackCreepResourceDataList;
        static List<UnitData> fallbackTowerResourceDataList;
        static List<UnitData> fallbackBossResourceDataList;
        #endregion

        #region Properties



        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods

        void LoadAllData()
        {
            LoadData(creepResourceDataList, creepDataFolderPath);
            LoadData(towerResourceDataList, towerDataFolderPath);
            LoadData(bossResourceDataList, bossDataFolderPath);
        }
        void LoadData(List<UnitData> _list, string _folderPath)
        {
            _list.Clear();

            // Ensure path is relative to Resources folder (strip Assets/Resources/ prefix if present)
            string _cleanPath = _folderPath.Replace("Assets/Resources/", "").Replace("\\", "/");

            UnitData[] _loadedData = Resources.LoadAll<UnitData>(_cleanPath);

            if (_loadedData.Length > 0)
            {
                _list.AddRange(_loadedData);
                _list = _list.OrderBy(x => x.unitId).ToList();
            }
        }

        #endregion

        #region Public Methods

        public static List<UnitData> GetCreepResourceDataList()
        {
            if (PlayableDataRegistry.Instance != null)
            {
                List<UnitData> registryData = PlayableDataRegistry.Instance.GetUnitDataList(UnitType.RangedGrunt);
                registryData.AddRange(PlayableDataRegistry.Instance.GetUnitDataList(UnitType.MeleeGrunt));
                registryData.AddRange(PlayableDataRegistry.Instance.GetUnitDataList(UnitType.RangedZombie));
                registryData.AddRange(PlayableDataRegistry.Instance.GetUnitDataList(UnitType.MeeleeZombie));
                if (registryData.Count > 0)
                    return registryData;
            }

            Units _resource = resourceLoader.Resource;
            if (_resource != null && _resource.creepResourceDataList != null && _resource.creepResourceDataList.Count > 0)
                return _resource.creepResourceDataList;

            if (fallbackCreepResourceDataList == null)
                fallbackCreepResourceDataList = LoadFallbackUnitData(fallbackCreepDataFolderPath);

            return fallbackCreepResourceDataList;
        }
        public static List<UnitData> GetTowerResourceDataList()
        {
            if (PlayableDataRegistry.Instance != null)
            {
                List<UnitData> registryData = PlayableDataRegistry.Instance.GetUnitDataList(UnitType.Tower);
                if (registryData.Count > 0)
                    return registryData;
            }

            Units _resource = resourceLoader.Resource;
            if (_resource != null && _resource.towerResourceDataList != null && _resource.towerResourceDataList.Count > 0)
                return _resource.towerResourceDataList;

            if (fallbackTowerResourceDataList == null)
                fallbackTowerResourceDataList = LoadFallbackUnitData(fallbackTowerDataFolderPath);

            return fallbackTowerResourceDataList;
        }
        public static List<UnitData> GetBossResourceDataList()
        {
            if (PlayableDataRegistry.Instance != null)
            {
                List<UnitData> registryData = PlayableDataRegistry.Instance.GetUnitDataList(UnitType.Boss);
                if (registryData.Count > 0)
                    return registryData;
            }

            Units _resource = resourceLoader.Resource;
            if (_resource != null && _resource.bossResourceDataList != null && _resource.bossResourceDataList.Count > 0)
                return _resource.bossResourceDataList;

            if (fallbackBossResourceDataList == null)
                fallbackBossResourceDataList = LoadFallbackUnitData(fallbackBossDataFolderPath);

            return fallbackBossResourceDataList;
        }
        static List<UnitData> LoadFallbackUnitData(string path)
        {
            return Resources.LoadAll<UnitData>(path)
                .Where(x => x != null)
                .OrderBy(x => x.unitId)
                .ToList();
        }
        public static UnitData GetUnitData(UnitId _id)
        {
            if (PlayableDataRegistry.Instance != null)
            {
                UnitData registryData = PlayableDataRegistry.Instance.GetUnitData(_id);
                if (registryData != null)
                    return registryData;
            }

            var _data = GetCreepResourceData(_id);
            if (_data != null)
                return _data;
            _data = GetTowerResourceData(_id);
            if (_data != null)
                return _data;
            return GetBossResourceData(_id);
        }
        public static UnitData GetCreepResourceData(UnitId _id)
        {
            if (PlayableDataRegistry.Instance != null)
            {
                UnitData registryData = PlayableDataRegistry.Instance.GetUnitData(_id);
                if (registryData != null && (registryData.unitType == UnitType.MeleeGrunt || registryData.unitType == UnitType.RangedGrunt || registryData.unitType == UnitType.MeeleeZombie || registryData.unitType == UnitType.RangedZombie))
                    return registryData;
            }

            var _data = FindUnitData(GetCreepResourceDataList(), _id);
            if (_data != null)
                return _data;

            return FindUnitData(LoadFallbackUnitData(fallbackCreepDataFolderPath), _id);
        }
        public static UnitData GetTowerResourceData(UnitId _id)
        {
            if (PlayableDataRegistry.Instance != null)
            {
                UnitData registryData = PlayableDataRegistry.Instance.GetUnitData(_id);
                if (registryData != null && (registryData.unitType == UnitType.Tower || IsTowerUnitId(registryData.unitId)))
                    return registryData;
            }

            var _data = FindUnitData(GetTowerResourceDataList(), _id);
            if (_data != null)
                return _data;

            return FindUnitData(LoadFallbackUnitData(fallbackTowerDataFolderPath), _id);
        }
        public static UnitData GetBossResourceData(UnitId _id)
        {
            if (PlayableDataRegistry.Instance != null)
            {
                UnitData registryData = PlayableDataRegistry.Instance.GetUnitData(_id);
                if (registryData != null && registryData.unitType == UnitType.Boss)
                    return registryData;
            }

            var _data = FindUnitData(GetBossResourceDataList(), _id);
            if (_data != null)
                return _data;

            return FindUnitData(LoadFallbackUnitData(fallbackBossDataFolderPath), _id);
        }
        static UnitData FindUnitData(List<UnitData> dataList, UnitId id)
        {
            if (dataList == null)
                return null;

            return dataList.Find(x => x != null && x.unitId.Equals(id));
        }
        static bool IsTowerUnitId(UnitId id)
        {
            return id == UnitId.Tower_1 || id == UnitId.Tower_2 || id == UnitId.Tower_3 || id == UnitId.Tower_4;
        }
        public static BigDouble GetCombatPoint(UnitId _id, float _ratioDamage, float _ratioHealth)
        {
            var _data = GetCreepResourceData(_id); // Assuming you want to get the data for a creep, adjust as needed
            if (_data == null)
                return 0;

            BigDouble _damage = (BigDouble)_data.damage;
            BigDouble _maxHealth = (BigDouble)_data.health;

            return (_ratioDamage * _damage * 5 + _ratioHealth * _maxHealth) * 10;
        }
        public static GameObject GetUnit3DModel(UnitId _skinId)
        {
            UnitData data = GetUnitData(_skinId);
            if (data == null)
            {
                Debug.LogError($"GetUnit3DModel failed: UnitData not found. unitId={_skinId}");
                return null;
            }

            if (data.unitPrefab == null)
            {
                Debug.LogError($"GetUnit3DModel failed: unitPrefab is missing. unitId={_skinId}");
                return null;
            }

            var _model = Instantiate(data.unitPrefab, Vector3.zero, Quaternion.identity) as CreepUnit;
            if (_model == null)
            {
                Debug.LogError($"GetUnit3DModel failed: prefab is not CreepUnit. unitId={_skinId}, prefab={data.unitPrefab.name}");
                return null;
            }

            _model.SetData(_skinId);
            _model.SetSkin();
            _model.SetAnimatorOverrideController();
            _model.PlayIdleAnim();
            return _model.gameObject;
        }

        #endregion      
    }
}
