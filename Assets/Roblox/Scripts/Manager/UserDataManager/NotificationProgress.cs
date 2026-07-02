using System;
using UnityEngine;

namespace ExampleProject.Manager
{
    [Serializable]
    public class NotificationProgress
    {
        #region Fields

        [SerializeField] bool hasAskedForNotifyPermission;
        [SerializeField] bool isGrantedNotifyPermission;

        #endregion

        #region Properties

        public static bool HasAskedForNotifyPermission
        {
            get => Instance.hasAskedForNotifyPermission;
            set
            {
                Instance.hasAskedForNotifyPermission = value;
                UserDataManager.SaveData();
            }
        }
        public static bool IsGrantedNotifyPermission
        {
            get => Instance.isGrantedNotifyPermission;
            set
            {
                Instance.isGrantedNotifyPermission = value;
                UserDataManager.SaveData();
            }
        }
        public static NotificationProgress Instance => SharedRobloxUserData.NotificationProgress;

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public NotificationProgress()
        {
            hasAskedForNotifyPermission = false;
            isGrantedNotifyPermission = false;
        }

        #endregion
    }
}