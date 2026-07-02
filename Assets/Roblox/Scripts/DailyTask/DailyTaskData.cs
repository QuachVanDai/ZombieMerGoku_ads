using ExampleProject.Gameplay.Scenes;
using ExampleProject.UI.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.DailyTask
{
    [CreateAssetMenu(fileName = "DailyTaskData", menuName = "ScriptableObjects/DailyTask/DailyTaskData")]
    public class DailyTaskData : ScriptableObject
    {
        #region Fields

        public DailyTaskId id;
        public virtual string GetLocalizedDescription()
        {
            return "";
        }
        public virtual int GetTarget()
        {
            return 0;
        }
        public int point = 10;

        #endregion
    }

    public enum DailyTaskId
    {
        None = 0,
        ClaimDailyReward = 1,
        PlayCount1 = 2,
        PlayCount2 = 3,
        PlayTournament = 4,
        WinGame1 = 5,
        WinGame2 = 6,
        RandomAchievement = 7,
        PlayRandomGame = 8,
        SpinLuckyWheel = 9,
        PlayAdventure = 10,
        WatchRewardedAd = 11,
    }
}