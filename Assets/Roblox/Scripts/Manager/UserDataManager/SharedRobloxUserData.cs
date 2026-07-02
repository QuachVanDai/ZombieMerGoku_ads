using System;
using UnityEngine;

namespace ExampleProject.Manager
{
    [Serializable]
    public class SharedRobloxUserData
    {
        #region Fields

      
        public ShopProgress shopProgress;
       
        public WeaponProgress weaponProgress;
        public WingProgress wingProgress;
        public SkinProgress skinProgress;
        public CurrencyProgress currencyProgress;
     
        public PlayerEquipment playerEquipment;
        public UserSetting userSetting;
        public DeveloperSetting developerSetting;

        public GameData gameData;

        // Not yet available in Roblox, but we want to save it in case we add it later
     
        public RateProgress rateProgress;
        public NotificationProgress notificationProgress;
        public AchievementProgress achievementProgress;
        public AllDailyTaskProgress allDailyTaskProgress;

        #endregion

        #region Properties

        public static SharedRobloxUserData Instance => UserDataManager.SharedRobloxUserData;
       
        public static ShopProgress ShopProgress
        {
            get => Instance.shopProgress;
            set
            {
                Instance.shopProgress = value;
                UserDataManager.SaveData();
            }
        }

        public static WeaponProgress WeaponProgress
        {
            get => Instance.weaponProgress;
            set
            {
                Instance.weaponProgress = value;
                UserDataManager.SaveData();
            }
        }
        public static WingProgress WingProgress
        {
            get => Instance.wingProgress;
            set
            {
                Instance.wingProgress = value;
                UserDataManager.SaveData();
            }
        }
        public static SkinProgress SkinProgress
        {
            get => Instance.skinProgress;
            set
            {
                Instance.skinProgress = value;
                UserDataManager.SaveData();
            }
        }
        public static CurrencyProgress CurrencyProgress
        {
            get => Instance.currencyProgress;
            set
            {
                Instance.currencyProgress = value;
                UserDataManager.SaveData();
            }
        }

        public static PlayerEquipment PlayerEquipment
        {
            get => Instance.playerEquipment;
            set
            {
                Instance.playerEquipment = value;
                UserDataManager.SaveData();
            }
        }

        public static UserSetting UserSetting
        {
            get => Instance.userSetting;
            set
            {
                Instance.userSetting = value;
                UserDataManager.SaveData();
            }
        }
        public static DeveloperSetting DeveloperSetting
        {
            get => Instance.developerSetting;
            set
            {
                Instance.developerSetting = value;
                UserDataManager.SaveData();
            }
        }

        public static GameData GameData
        {
            get => Instance.gameData;
            set
            {
                Instance.gameData = value;
                UserDataManager.SaveData();
            }
        }
        public static RateProgress RateProgress
        {
            get => Instance.rateProgress;
            set
            {
                Instance.rateProgress = value;
                UserDataManager.SaveData();
            }
        }
        public static NotificationProgress NotificationProgress
        {
            get => Instance.notificationProgress;
            set
            {
                Instance.notificationProgress = value;
                UserDataManager.SaveData();
            }
        }
        public static AchievementProgress AchievementProgress
        {
            get => Instance.achievementProgress;
            set
            {
                Instance.achievementProgress = value;
                UserDataManager.SaveData();
            }
        }
        public static AllDailyTaskProgress AllDailyTaskProgress
        {
            get => Instance.allDailyTaskProgress;
            set
            {
                Instance.allDailyTaskProgress = value;
                UserDataManager.SaveData();
            }
        }
  

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public SharedRobloxUserData()
        {
          
            shopProgress = new ShopProgress();
           
            weaponProgress = new WeaponProgress();
            wingProgress = new WingProgress();
            skinProgress = new SkinProgress();
         
            playerEquipment = new PlayerEquipment();
            currencyProgress = new CurrencyProgress();

            userSetting = new UserSetting();
            developerSetting = new DeveloperSetting();

            gameData = new GameData();

            rateProgress = new RateProgress();
            notificationProgress = new NotificationProgress();
            achievementProgress = new AchievementProgress();
            allDailyTaskProgress = new AllDailyTaskProgress();
           
        }

        #endregion
    }
}
