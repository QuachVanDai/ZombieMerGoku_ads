using System;
using UnityEngine;

namespace ExampleProject.Manager
{
    [Serializable]
    public class RateProgress
    {
        #region Fields

        [SerializeField] bool isRated;
        [SerializeField] bool isShowRateAtLeastOne;
        [SerializeField] int currentIndexShowRate;

        #endregion

        #region Properties

        public static bool IsRated
        {
            get => Instance.isRated;
            set
            {
                Instance.isRated = value;
                UserDataManager.SaveData();
            }
        }
        public static bool IsShowRateAtLeastOne
        {
            get => Instance.isShowRateAtLeastOne;
            set
            {
                Instance.isShowRateAtLeastOne = value;
                UserDataManager.SaveData();
            }
        }
        public static int CurrentIndexShowRate
        {
            get => Instance.currentIndexShowRate;
            set
            {
                Instance.currentIndexShowRate = value;
                UserDataManager.SaveData();
            }
        }
        public static RateProgress Instance => SharedRobloxUserData.RateProgress;

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public RateProgress()
        {
            isRated = false;
            isShowRateAtLeastOne = false;
            currentIndexShowRate = 0;
        }

        #endregion
    }
}