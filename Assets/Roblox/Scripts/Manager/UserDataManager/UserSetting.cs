using ExampleProject.GameSystem;
using System;
using UnityEngine;
using VTLTools;

namespace ExampleProject.Manager
{
    [Serializable]
    public class UserSetting
    {
        #region Fields

        [SerializeField] string soundVolume;
        [SerializeField] string musicVolume;
        [SerializeField] bool isVFXOn;
        [SerializeField] string currentLanguage;
       
        [SerializeField] float cameraSensitivity;
        [SerializeField] CameraDistanceLevel cameraDistance;
        [SerializeField] bool isExpandHomeMenu;

        #endregion

        #region Properties

        public static float SoundVolume
        {
            get => float.Parse(Instance.soundVolume);
            set
            {
                Instance.soundVolume = value.ToString();
                UserDataManager.SaveData();
            }
        }
        public static float MusicVolume
        {
            get => float.Parse(Instance.musicVolume);
            set
            {
                Instance.musicVolume = value.ToString();
                UserDataManager.SaveData();
            }
        }

        public static bool IsVFXOn
        {
            get => Instance.isVFXOn;
            set
            {
                Instance.isVFXOn = value;
                UserDataManager.SaveData();
            }
        }
     

        public static string CurrentLanguage
        {
            get => Instance.currentLanguage;
            set
            {
                Instance.currentLanguage = value;
                UserDataManager.SaveData();
            }
        }
        public static float CameraSensitivity
        {
            get => Instance.cameraSensitivity;
            set
            {
                Instance.cameraSensitivity = value;
                UserDataManager.SaveData();
                EventDispatcher.Instance.Dispatch(EventName.OnCameraSensitivityChanged, null);
            }
        }
        public static CameraDistanceLevel CameraDistanceLevel
        {
            get => Instance.cameraDistance;
            set
            {
                Instance.cameraDistance = value;
                UserDataManager.SaveData();
            }
        }
        public static bool IsExpandHomeMenu
        {
            get => Instance.isExpandHomeMenu;
            set
            {
                Instance.isExpandHomeMenu = value;
                UserDataManager.SaveData();
            }
        }

        public static UserSetting Instance => SharedRobloxUserData.UserSetting;

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public UserSetting()
        {
            soundVolume = "1";
            musicVolume = "1";
         
            isVFXOn = true;
            currentLanguage = "English";
            cameraSensitivity = 15f;
            cameraDistance = CameraDistanceLevel.Close;
            isExpandHomeMenu = true;
        }

        #endregion
    }
}
