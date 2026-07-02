using System.Collections.Generic;
using ExampleProject.Gameplay.Buffs;
using ExampleProject.Gameplay.Characters.Skin;
using ExampleProject.Gameplay.Faction;
using ExampleProject.Gameplay.Level;
using ExampleProject.Gameplay.Rarity;
using ExampleProject.Gameplay.Unit;
using ExampleProject.Gameplay.Weapon;
using UnityEngine;

namespace ExampleProject.Manager
{
    [DefaultExecutionOrder(-1000)]
    public class PlayableDataRegistry : MonoBehaviour
    {
        #region Fields

        static PlayableDataRegistry instance;

        [SerializeField] List<UnitData> unitDataList = new List<UnitData>();
        [SerializeField] List<TactixChestData> chestDataList = new List<TactixChestData>();
        [SerializeField] List<RarityData> rarityDataList = new List<RarityData>();
        [SerializeField] List<WeaponData> weaponDataList = new List<WeaponData>();
        [SerializeField] List<SkinData> skinDataList = new List<SkinData>();
        [SerializeField] List<BuffData> buffDataList = new List<BuffData>();
        [SerializeField] List<FactionData> factionDataList = new List<FactionData>();
        [SerializeField] List<LevelData> levelDataList = new List<LevelData>();

        #endregion

        #region Properties

        public static PlayableDataRegistry Instance => instance;

        #endregion

        #region LifeCycle

        void Awake()
        {
            instance = this;
        }

        void OnEnable()
        {
            instance = this;
        }

        void OnDisable()
        {
            if (instance == this)
                instance = null;
        }

        #endregion

        #region Public Methods

        public List<UnitData> GetUnitDataList(UnitType unitType)
        {
            List<UnitData> result = new List<UnitData>();
            for (int i = 0; i < unitDataList.Count; i++)
            {
                UnitData data = unitDataList[i];
                if (data != null && data.unitType == unitType)
                    result.Add(data);
            }

            return result;
        }

        public UnitData GetUnitData(UnitId id)
        {
            for (int i = 0; i < unitDataList.Count; i++)
            {
                UnitData data = unitDataList[i];
                if (data != null && data.unitId == id)
                    return data;
            }

            return null;
        }

        public List<TactixChestData> GetChestDataList()
        {
            List<TactixChestData> result = new List<TactixChestData>();
            for (int i = 0; i < chestDataList.Count; i++)
            {
                TactixChestData data = chestDataList[i];
                if (data != null)
                    result.Add(data);
            }

            return result;
        }

        public TactixChestData GetChestData(ChestId id)
        {
            for (int i = 0; i < chestDataList.Count; i++)
            {
                TactixChestData data = chestDataList[i];
                if (data != null && data.chestId == id)
                    return data;
            }

            return null;
        }

        public TactixChestData GetChestDataByRarity(RarityId rarity)
        {
            for (int i = 0; i < chestDataList.Count; i++)
            {
                TactixChestData data = chestDataList[i];
                if (data != null && data.rarity == rarity)
                    return data;
            }

            return null;
        }

        public List<RarityData> GetRarityDataList()
        {
            List<RarityData> result = new List<RarityData>();
            for (int i = 0; i < rarityDataList.Count; i++)
            {
                RarityData data = rarityDataList[i];
                if (data != null)
                    result.Add(data);
            }

            return result;
        }

        public RarityData GetRarityData(RarityId rarity)
        {
            for (int i = 0; i < rarityDataList.Count; i++)
            {
                RarityData data = rarityDataList[i];
                if (data != null && data.rarity == rarity)
                    return data;
            }

            return null;
        }

        public List<WeaponData> GetWeaponDataList()
        {
            List<WeaponData> result = new List<WeaponData>();
            for (int i = 0; i < weaponDataList.Count; i++)
            {
                WeaponData data = weaponDataList[i];
                if (data != null)
                    result.Add(data);
            }

            return result;
        }

        public WeaponData GetWeaponData(WeaponId id)
        {
            for (int i = 0; i < weaponDataList.Count; i++)
            {
                WeaponData data = weaponDataList[i];
                if (data != null && data.id == id)
                    return data;
            }

            return null;
        }

        public List<SkinData> GetSkinDataList()
        {
            List<SkinData> result = new List<SkinData>();
            for (int i = 0; i < skinDataList.Count; i++)
            {
                SkinData data = skinDataList[i];
                if (data != null)
                    result.Add(data);
            }

            return result;
        }

        public SkinData GetSkinData(SkinId id)
        {
            for (int i = 0; i < skinDataList.Count; i++)
            {
                SkinData data = skinDataList[i];
                if (data != null && data.id == id)
                    return data;
            }

            return null;
        }

        public List<BuffData> GetBuffDataList()
        {
            List<BuffData> result = new List<BuffData>();
            for (int i = 0; i < buffDataList.Count; i++)
            {
                BuffData data = buffDataList[i];
                if (data != null)
                    result.Add(data);
            }

            return result;
        }

        public BuffData GetBuffData(BuffType type)
        {
            for (int i = 0; i < buffDataList.Count; i++)
            {
                BuffData data = buffDataList[i];
                if (data != null && data.type == type)
                    return data;
            }

            return null;
        }

        public List<FactionData> GetFactionDataList()
        {
            List<FactionData> result = new List<FactionData>();
            for (int i = 0; i < factionDataList.Count; i++)
            {
                FactionData data = factionDataList[i];
                if (data != null)
                    result.Add(data);
            }

            return result;
        }

        public FactionData GetFactionData(FactionId id)
        {
            for (int i = 0; i < factionDataList.Count; i++)
            {
                FactionData data = factionDataList[i];
                if (data != null && data.id == id)
                    return data;
            }

            return null;
        }

        public List<LevelData> GetLevelDataList()
        {
            List<LevelData> result = new List<LevelData>();
            for (int i = 0; i < levelDataList.Count; i++)
            {
                LevelData data = levelDataList[i];
                if (data != null)
                    result.Add(data);
            }

            return result;
        }

        public LevelData GetLevelData(int level)
        {
            for (int i = 0; i < levelDataList.Count; i++)
            {
                LevelData data = levelDataList[i];
                if (data != null && data.levelNumber == level)
                    return data;
            }

            return null;
        }

        #endregion
    }
}
