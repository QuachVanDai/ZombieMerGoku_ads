using ExampleProject.GameSystem;
using System;
using System.Globalization;
using UnityEngine;
using VTLTools;

namespace ExampleProject.Manager
{
    [Serializable]
    public class DeveloperSetting
    {
        #region Fields

        [SerializeField] bool isCheatNoAds;
        [SerializeField] bool isShowFps;
        [SerializeField] bool isShowDebug;
        [SerializeField] bool creativeServe;
        [SerializeField] bool showMaxMediation;
        [SerializeField] bool noAds;
        [SerializeField] bool noStore;

        #endregion

        #region Properties

        public static bool IsCheatNoAds
        {
            get => Instance.isCheatNoAds;
            set
            {
                Instance.isCheatNoAds = value;
                UserDataManager.SaveData();
            }
        }
        public static bool IsShowFps
            {
            get => Instance.isShowFps;
            set
            {
                Instance.isShowFps = value;
                UserDataManager.SaveData();
            }
        }
        public static bool IsShowDebug
            {
            get => Instance.isShowDebug;
            set
            {
                Instance.isShowDebug = value;
                UserDataManager.SaveData();
            }
        }
        public static bool CreativeServe
        {
            get => Instance.creativeServe;
            set
            {
                Instance.creativeServe = value;
                UserDataManager.SaveData();
            }
        }
        public static bool ShowMaxMediation
            {
            get => Instance.showMaxMediation;
            set
            {
                Instance.showMaxMediation = value;
                UserDataManager.SaveData();
            }
        }
        public static bool NoAds
        {
            get => Instance.noAds;
            set
            {
                Instance.noAds = value;
                UserDataManager.SaveData();
            }
        }
        public static bool NoStore
        {
            get => Instance.noStore;
            set
            {
                Instance.noStore = value;
                UserDataManager.SaveData();
            }
        }

        public static DeveloperSetting Instance => SharedRobloxUserData.DeveloperSetting;

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public DeveloperSetting()
        {
            isCheatNoAds = false;
            isShowFps = false;
            isShowDebug = false;
            creativeServe = false;
            showMaxMediation = false;
            noAds = false;
            noStore = false;
        }

        #endregion
    }
}