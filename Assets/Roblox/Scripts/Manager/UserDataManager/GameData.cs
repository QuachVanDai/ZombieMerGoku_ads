using ExampleProject.GameSystem;
using System;
using UnityEngine;
using VTLTools;

namespace ExampleProject.Manager
{
    [Serializable]
    public class GameData
    {
        #region Fields

        [SerializeField] int countGamePlayed;
        [SerializeField] string lastOnlineTime;
        [SerializeField] string firstTimeOpenGame;
        [SerializeField] int currentLevel = 1;

        #endregion

        #region Properties

        public static int CountGamePlayed
        {
            get => Instance.countGamePlayed;
            set
            {
                Instance.countGamePlayed = value;
                UserDataManager.SaveData();
            }
        }
        public static DateTime LastOnlineTime
        {
            get => DateTime.Parse(Instance.lastOnlineTime);
            set
            {
                Instance.lastOnlineTime = value.ToString("O");
                UserDataManager.SaveData();
            }
        }
        public static string FirstTimeOpenGame
        {
            get => Instance.firstTimeOpenGame;
            set
            {
                Instance.firstTimeOpenGame = value;
                UserDataManager.SaveData();
            }
        }
        public static int CurrentLevel
        {
            get => Instance.currentLevel <= 0 ? 1 : Instance.currentLevel;
            set
            {
                Instance.currentLevel = Mathf.Max(1, value);
            }
        }
        public static GameData Instance => SharedRobloxUserData.GameData;

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public GameData()
        {
            countGamePlayed = 0;
            lastOnlineTime = DateTime.Now.ToString("O");
            firstTimeOpenGame = DateTime.Now.ToString("O");
            currentLevel = 1;
        }

        #endregion
    }
}
