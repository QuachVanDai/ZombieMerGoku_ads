using ExampleProject.Gameplay.Scenes;
using ExampleProject.UI.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.Achievement
{
    [CreateAssetMenu(fileName = "AchievementData", menuName = "ScriptableObjects/Achievement/AchievementData")]
    public class AchievementData : ScriptableObject
    {
        #region Fields

        public AchievementId id;
        public List<AchievementTarget> targets;
        [SerializeField] protected string localizedDescription;

        public virtual string GetLocalizedDescription(int _targetIndex)
        {
            try
            {
                return string.Format(localizedDescription, targets[_targetIndex].targetValue,
                    targets[_targetIndex].secondTargetValue);
            }
            catch (FormatException)
            {
                return localizedDescription.ToString().Replace("}0{", targets[_targetIndex].targetValue.ToString())
                    .Replace("}1{", targets[_targetIndex].secondTargetValue.ToString());
            }
            catch
            {
                return "";
            }
        }

        #endregion
    }

    public enum AchievementId
    {
        None = 0,

        Hurdle = 1,
        MissileBattle = 2,
        BikeRace = 3,
        NinjaRun = 4,
        FlashHand = 5,
        PoleClimb = 6,
        BalloonBurst = 7,
        TeamHammer = 8,
        TugOfWar = 9,
        WindUp = 10,
        BombPick = 11,
        DrumChain = 12,
        SquidGame = 13,
        ExplosiveTennis = 14,
        SkySkate = 15,
        RedLine = 16,
        CrabTrap = 17,
        Baseball = 18,
        ColorShot = 19,
        ColorSwitch = 20,
        BoomTiles = 21,
        PlaneBattle = 22,
        CandyHunter = 23,
        BurgerTime = 24,
        LawnParty = 25,
        MagnetMaster = 26,
        SingSprint = 27,
        PrisonBreak = 28,
        TileTumble = 29,
        FallingFloor = 30,
        Gladiator = 31,
        TrafficJam = 32,
        RacingCars = 33,
        JetpackRace = 34,
        GoldCatcher = 35,
        TheCrusher = 36,
        ZombieSurvival = 37,
        Paintball = 38,
        GoShopping = 39,
        PoliceChase = 40,
        RaftRace = 41,
        FishCatch = 42,
        ChopWoods = 43,
        Bowling = 44,
        DuoDash = 45,
        CatchFruit = 46,

        CakeFactory = 48,
        SortFruit = 49,
        RopeCut = 50,
        BombCounting = 51,
        GardenDefense = 52,
        ColorClaim = 53,
        SoupParty = 55,
        FruitRush = 56,

        Hurdling = 58,
        Nails = 59,
        FanDodge = 64,

        PlayRound = 10001,
        BuyAny = 10002,
        UnlockSkinColor = 10003,
        UnlockSkin = 10004,
    }

    [Serializable]
    public class AchievementTarget
    {
        public int targetValue;
        public int secondTargetValue;
        public Sprite icon;
        public Item reward;
    }
}