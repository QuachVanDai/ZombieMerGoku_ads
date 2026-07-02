using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BreakInfinity;
using ExampleProject.Gameplay.Rarity;
using UnityEngine;

namespace ExampleProject
{
    [CreateAssetMenu(fileName = "TactixChestData ", menuName = "ScriptableObjects/Tactix/ChestData")]
    public class TactixChestData : ScriptableObject
    {
        #region Fields

        public ChestId chestId;
        public BaseTactixChest chestPrefab;
        public GameObject chestVFXOpenPrefab;

        public Vector3 chestLidPos;
        //public AnimatorOverrideController animatorOverride;
        public Sprite icon;
        public Color color;
        public List<Mesh> mesh;
        public Material material;

        public RarityId rarity;
        public float spawnChance;
        public List<RarityChances> rarityChancesList;
        public float baseCooldown;
        public float timePerSkip;
        public int cost;
        public int gemCostToSkip;

        #endregion

        #region Properties



        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods
        private void OnValidate()
        {
            // Lấy tất cả RarityId trừ None
            var rarities = Enum.GetValues(typeof(RarityId))
                .Cast<RarityId>()
                .Where(r => r != RarityId.None)
                .ToList();

            if (rarityChancesList == null)
                rarityChancesList = new List<RarityChances>();

            // Reset list nếu số lượng không khớp
            if (rarityChancesList.Count != rarities.Count)
            {
                rarityChancesList.Clear();

                foreach (var rarity in rarities)
                {
                    rarityChancesList.Add(new RarityChances { rarity = rarity, chance = 0f });
                }
            }
            else
            {
                // Đồng bộ lại thứ tự rarity
                for (int i = 0; i < rarities.Count; i++)
                {
                    rarityChancesList[i].rarity = rarities[i];
                }
            }
        }


        #endregion

        #region Public Methods



        #endregion
    }
    public enum ChestId
    {
        None = 0,

        Chest_1 = 2001,
        Chest_2 = 2002,
        Chest_3 = 2003,
        Chest_4 = 2004,
        Chest_5 = 2005,
        Chest_6 = 2006,
        Chest_7 = 2007,
        Chest_8 = 2008,
       
    }
    [Serializable]
    public class RarityChances
    {
        public RarityId rarity;
        public float chance;
    }
}
