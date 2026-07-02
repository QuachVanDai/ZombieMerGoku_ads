using BreakInfinity;
using ExampleProject.Gameplay.Weapon;
using ExampleProject.Tools;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExampleProject.Gameplay.Buffs;
using ExampleProject.Manager;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
namespace ExampleProject.Gameplay.Level
{
    [CreateAssetMenu(fileName = "Levels", menuName = "ScriptableObjects/Level/Levels")]
    public class Levels : ScriptableObject
    {
        #region Fields
        [SerializeField] List<LevelData> resourceDataList = new List<LevelData>();        [SerializeField] string dataFolderPath;
        const string resourceFolderPath = "Data/Levels";
        const string fallbackLevelDataFolderPath = "Data/LevelData";
        readonly static ResourceLoader<Levels> resourceLoader = new ResourceLoader<Levels>(resourceFolderPath);
        static List<LevelData> fallbackResourceDataList;
        #endregion
        #region Editor - Add New Level (Deep Copy)
#if UNITY_EDITOR
        private void AddNewLevelButton()
        {
            if (resourceDataList == null || resourceDataList.Count == 0)
            {
                Debug.LogError("resourceDataList is empty! Cannot create new level.");
                return;
            }
            if (string.IsNullOrEmpty(dataFolderPath))
            {
                Debug.LogError("dataFolderPath is not set!");
                return;
            }
            int maxLevel = resourceDataList.Max(x => x.levelNumber);
            LevelData source = resourceDataList.Find(x => x.levelNumber == maxLevel);
            if (source == null)
            {
                Debug.LogError("Cannot find highest level data.");
                return;
            }
            int newLevelNumber = maxLevel + 1;
            // Tạo LevelData mới và deep copy
            LevelData newLevel = CreateDeepCopy(source, newLevelNumber);
            string assetName = $"LevelData {newLevelNumber}";
            string fullPath = $"{dataFolderPath}/{assetName}.asset";
            fullPath = AssetDatabase.GenerateUniqueAssetPath(fullPath);
            // Tạo asset
            AssetDatabase.CreateAsset(newLevel, fullPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"✅ Created new level: <color=green>{assetName}</color> | Path: {fullPath}");
            LoadAllData();
        }
        private LevelData CreateDeepCopy(LevelData source, int newLevelNumber)
        {
            LevelData clone = ScriptableObject.CreateInstance<LevelData>();
            clone.levelNumber = newLevelNumber;
            clone.coinWinLevel = source.coinWinLevel;
            clone.gemWinLevel = source.gemWinLevel;
            clone.levelZombieBuffs = new List<Buff>();
            if (source.levelZombieBuffs != null)
            {
                foreach (var buff in source.levelZombieBuffs)
                {
                    Buff newBuff = new Buff
                    {
                        buffType = buff.buffType,
                        value = buff.value,
                        modifierType = buff.modifierType
                    };
                    clone.levelZombieBuffs.Add(newBuff);
                }
            }
            clone.levelTowerBuffs = new List<Buff>();
            if (source.levelTowerBuffs != null)
            {
                foreach (var buff in source.levelTowerBuffs)
                {
                    Buff newBuff = new Buff
                    {
                        buffType = buff.buffType,
                        value = buff.value,
                        modifierType = buff.modifierType
                    };
                    clone.levelTowerBuffs.Add(newBuff);
                }
            }
            clone.levelPhase = new List<LevelPhase>();
            if (source.levelPhase != null)
            {
                foreach (var phase in source.levelPhase)
                {
                    clone.levelPhase.Add(DeepCopyLevelPhase(phase));
                }
            }
            return clone;
        }
        private LevelPhase DeepCopyLevelPhase(LevelPhase source)
        {
            if (source == null) return null;
            LevelPhase clone = new LevelPhase
            {
                towerUnitId = source.towerUnitId,
                phaseTowerBuffs = source.phaseTowerBuffs != null ? new List<Buff>(source.phaseTowerBuffs) : new List<Buff>(),
                waves = new List<Wave>()
            };
            if (source.waves != null)
            {
                foreach (var wave in source.waves)
                {
                    clone.waves.Add(DeepCopyWave(wave));
                }
            }
            return clone;
        }
        private Wave DeepCopyWave(Wave source)
        {
            if (source == null) return null;
            Wave clone = new Wave
            {
                waveBuffs = source.waveBuffs != null ? new List<Buff>(source.waveBuffs) : new List<Buff>(),
                units = new List<WaveInfo>()
            };
            if (source.units != null)
            {
                foreach (var unitInfo in source.units)
                {
                    clone.units.Add(new WaveInfo
                    {
                        unitId = unitInfo.unitId,
                        quantity = unitInfo.quantity
                    });
                }
            }
            return clone;
        }
#endif
        #endregion
        #region Private Methods
        void LoadAllData()
        {
            resourceDataList.Clear();
            string _cleanPath = dataFolderPath.Replace("Assets/Resources/", "").Replace("\\", "/");
            LevelData[] _loadedData = Resources.LoadAll<LevelData>(_cleanPath);
            if (_loadedData.Length > 0)
            {
                resourceDataList.AddRange(_loadedData);
                resourceDataList = resourceDataList.OrderBy(x => x.levelNumber).ToList();
            }
        }
        #endregion
        #region Public Methods
        // (Giữ nguyên toàn bộ phần Public Methods như cũ)
        public static int GetWinLevelBonus(int _level)
        {
            var _data = GetResourceDataList().Find(x => x.levelNumber == _level);
            return _data != null ? _data.coinWinLevel : 0;
        }
        public static int GetWinLevelGemBonus(int _level)
        {
            var _data = GetResourceDataList().Find(x => x.levelNumber == _level);
            return _data != null ? _data.gemWinLevel : 0;
        }
        public static List<LevelData> GetResourceDataList()
        {
            if (PlayableDataRegistry.Instance != null)
            {
                List<LevelData> registryData = PlayableDataRegistry.Instance.GetLevelDataList();
                if (registryData.Count > 0)
                    return registryData;
            }

            Levels _resource = resourceLoader.Resource;
            if (_resource != null && _resource.resourceDataList != null && _resource.resourceDataList.Count > 0)
                return _resource.resourceDataList;

            if (fallbackResourceDataList == null)
            {
                fallbackResourceDataList = Resources.LoadAll<LevelData>(fallbackLevelDataFolderPath)
                    .Where(x => x != null)
                    .OrderBy(x => x.levelNumber)
                    .ToList();
            }

            return fallbackResourceDataList;
        }
        public static LevelData GetResourceData(int _id)
        {
            if (PlayableDataRegistry.Instance != null)
            {
                LevelData registryData = PlayableDataRegistry.Instance.GetLevelData(_id);
                if (registryData != null)
                    return registryData;
            }

            var _data = GetResourceDataList().Find(x => x != null && x.levelNumber.Equals(_id));
            return _data;
        }
        public static List<Buff> GetPhaseTowerBuffs(int _level, int _phase)
        {
            var _data = GetResourceDataList().Find(x => x.levelNumber == _level);
            return _data?.GetPhaseTowerBuffs(_phase);
        }
        public static List<Buff> GetLevelTowerBuffs(int _level)
        {
            var _data = GetResourceDataList().Find(x => x.levelNumber == _level);
            return _data?.GetLevelTowerBuffs();
        }
        public static List<Buff> GetWaveZombieBuffs(int _level, int _phase, int _wave)
        {
            var _data = GetResourceDataList().Find(x => x.levelNumber == _level);
            return _data?.GetWaveZombieBuffs(_phase, _wave);
        }
        public static List<Buff> GetLevelZombieBuffs(int _level)
        {
            var _data = GetResourceDataList().Find(x => x.levelNumber == _level);
            return _data?.GetLevelZombieBuffs();
        }
        public static LevelPhase GetPhase(int _level, int _phase)
        {
            var _data = GetResourceDataList().Find(x => x.levelNumber == _level);
            return _data?.GetLevelPhase(_phase);
        }
        public static List<Wave> GetWaves(int _level, int _phase)
        {
            return GetPhase(_level, _phase)?.waves;
        }
        public static Wave GetWave(int _level, int _phase, int _wave)
        {
            return GetPhase(_level, _phase)?.GetWave(_wave);
        }
        public static List<WaveInfo> GetWaveInfos(int _level, int _phase, int _wave)
        {
            var _data = GetResourceDataList().Find(x => x.levelNumber == _level);
            return _data?.GetWaveInfos(_phase, _wave);
        }
        public static bool IsOutOfWaves(int _level, int _phase, int _wave)
        {
            var _data = GetResourceDataList().Find(x => x.levelNumber == _level);
            return _data?.IsOutOfWave(_phase, _wave) ?? true;
        }
        public static bool IsOutOfPhase(int _level, int _phase)
        {
            var _data = GetResourceDataList().Find(x => x.levelNumber == _level);
            return _data?.IsOutOfPhase(_phase) ?? true;
        }
        #endregion
    }
}
