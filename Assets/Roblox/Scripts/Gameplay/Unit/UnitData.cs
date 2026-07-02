using ExampleProject.Gameplay.Rarity;
using ExampleProject.Gameplay.Unit;
using ExampleProject.UI.BaseUI.BasePopup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExampleProject.Gameplay.Unit
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObjects/Unit/UnitData")]
    public class UnitData : ScriptableObject
    {
        #region Fields

        public UnitId unitId;
        public UnitType unitType;
        public string localizedName;
        public string localizedDescription;
        public BaseUnit unitPrefab;
        public AnimatorOverrideController animatorOverride;
        public Sprite icon;
        public Mesh mesh;
        public Material material;

        public float scale = 1;
        public RarityId rarity;
        public float health;
        public float damage;
        public float criticalChance = 0;
        public float criticalMultiplier = 1.5f;
        public float attackSpeed;
        public float attackRange;
        public float moveSpeed;

        #endregion

        #region Properties

        public double HealthToDouble => health;
        public double DamageToDouble => damage;

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods



        #endregion
    }

    public enum UnitId
    {
        None = 0,

        MeleeGrunt_1 = 1001,
        MeleeGrunt_2 = 1002,
        MeleeGrunt_3 = 1003,
        MeleeGrunt_4 = 1004,
        MeleeGrunt_5 = 1005,
        MeleeGrunt_6 = 1006,
        MeleeGrunt_7 = 1007,
        MeleeGrunt_8 = 1008,
        MeleeGrunt_9 = 1009,
        MeleeGrunt_10 = 1010,
        
        RangedGrunt_1 = 2001,
        RangedGrunt_2 = 2002,
        RangedGrunt_3 = 2003,
        RangedGrunt_4 = 2004,
        RangedGrunt_5 = 2005,
        RangedGrunt_6 = 2006,
        RangedGrunt_7 = 2007,
        RangedGrunt_8 = 2008,
        RangedGrunt_9 = 2009,
        RangedGrunt_10 = 2010,

        MeeleeZombie_1 = 3001,
        MeeleeZombie_2 = 3002,
        MeeleeZombie_3 = 3003,
        MeeleeZombie_4 = 3004,
        MeeleeZombie_5 = 3005,
        MeeleeZombie_6 = 3006,

        RangedZombie_1 = 4001,
        RangedZombie_2 = 4002,
        RangedZombie_3 = 4003,
        RangedZombie_4 = 4004,
        RangedZombie_5 = 4005,
        RangedZombie_6 = 4006,

        Boss_1 = 5001,
        Boss_2 = 5002,
        Boss_3 = 5003,
        Boss_4 = 5004,

        Tower_1 = 6001,
        Tower_2 = 6002,
        Tower_3 = 6003,
        Tower_4 = 6004,

        ZombieReferee = 7001,
    }
    public enum UnitType
    {
        None = 0,
        MeleeGrunt,
        RangedGrunt,
        MeeleeZombie,
        RangedZombie,
        Boss,
        Tower
    }
}
