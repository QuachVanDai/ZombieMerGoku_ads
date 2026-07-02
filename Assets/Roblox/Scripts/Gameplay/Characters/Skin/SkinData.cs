using ExampleProject.Gameplay.Buffs;
using ExampleProject.Gameplay.Rarity;
using ExampleProject.Gameplay.Scenes;
using ExampleProject.Gameplay.Weapon;
using ExampleProject.Manager;
using ExampleProject.UI.Shared;
using System.Collections.Generic;
using UnityEngine;

namespace ExampleProject.Gameplay.Characters.Skin
{
    [CreateAssetMenu(fileName = "SkinData", menuName = "ScriptableObjects/Skin/SkinData")]
    public class SkinData : ScriptableObject
    {
        #region Fields

        public SkinId id;
        public RarityId rarity;
      public Sprite avatar;
        public Mesh mesh;
        public Material material;
        public AnimatorOverrideController playableAnimatorOverrideController;
        public AnimatorOverrideController displayAnimatorOverrideController;
        public DominantHand dominantHand = DominantHand.RightHand;
        public UnlockCondition unlockCondition = new UnlockCondition();

        public List<Buff> buffs;

        #endregion
    }

    public enum SkinId
    {
        None = 0,
        Steve = 1,
        HeartFace = 2,
        SquareFace = 3,
        CircleFace = 4,
        Fireman = 7,
        Jokker = 10,
        Rolambo = 13,
        Messee = 15,
        NeonRobot = 16,
        HumanRefernce = 18,
        ZombieReferee = 5001,

        MeleeGrunt01 = 1001,
        MeleeGrunt02 = 1002,
        MeleeGrunt03 = 1003,
        MeleeGrunt04 = 1004,
        MeleeGrunt05 = 1005,
        MeleeGrunt06 = 1006,
        MeleeGrunt07 = 1007,
        MeleeGrunt08 = 1008,
        MeleeGrunt09 = 1009,
        MeleeGrunt10 = 1010,

        RangeGrunt01 = 1101,
        RangeGrunt02 = 1102,
        RangeGrunt03 = 1103,
        RangeGrunt04 = 1104,
        RangeGrunt05 = 1105,
        RangeGrunt06 = 1106,
        RangeGrunt07 = 1107,
        RangeGrunt08 = 1108,
        RangeGrunt09 = 1109,
        RangeGrunt10 = 1110,

        MeleeZombie01 = 2001,
        MeleeZombie02 = 2002,
        MeleeZombie03 = 2003,
        MeleeZombie04 = 2004,
        MeleeZombie05 = 2005,
        MeleeZombie06 = 2006,
        MeleeZombie07 = 2007,
        MeleeZombie08 = 2008,
        MeleeZombie09 = 2009,
        MeleeZombie10 = 2010,

        RangeZombie01 = 2101,
        RangeZombie02 = 2102,
        RangeZombie03 = 2103,
        RangeZombie04 = 2104,
        RangeZombie05 = 2105,
        RangeZombie06 = 2106,
        RangeZombie07 = 2107,
        RangeZombie08 = 2108,
        RangeZombie09 = 2109,
        RangeZombie10 = 2110,

        Boss01 = 3001,
    }
    public enum DominantHand
    {
        None = 0,
        LeftHand = 1,
        RightHand = 2
    }
}
