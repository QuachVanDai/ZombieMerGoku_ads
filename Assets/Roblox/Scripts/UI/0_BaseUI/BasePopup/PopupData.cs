using ExampleProject.Gameplay.Scenes;
using UnityEngine;

namespace ExampleProject.UI.BaseUI.BasePopup
{
    [CreateAssetMenu(fileName = "PopupData", menuName = "ScriptableObjects/Popup/PopupData")]
    public class PopupData : ScriptableObject
    {
        #region Fields

        public PopupId id;
        public BasePopup basePopupPrefab;

        #endregion
    }

    public enum PopupId
    {
        None = 0,

        LoadingScene = 1,
        Home = 2,
        Gameplay = 4,

        DevMode = 6,

        Input = 7,

        Notification = 11,

        PurchaseEquipmentSuccess = 12,
        PurchaseCurrencySuccess = 13,
        PurchaseRemoveForceAdsSuccess = 14,
        PurchaseFail = 15,

        LoadingNormal = 16,
        LoadingInterAds = 17,

        RefreshStoreOffer = 18,
        DailyReward = 19,

        EffectSpawner = 20,
        AdGifts = 21,

        TactixGameplay = 1001,
        GetTactixUnit = 1002,
        TactixWinLose = 1003,
        TactixTutorial = 1005,
        TacTixReconPopup = 1006,
        TacTixReconQuickClaimPopup = 1007,
        TactixComic = 1008,
        TactixDialogueBox = 1009,
        OnlineRewardPopup = 1010,
        IndexPopup = 1011,
        TactixBuyAutoTime = 1014,

        PlayableChestReward = 2000,
        PlayableCTA = 2001,
        PlayableEndcard = 2002,
        
        EventSchedule = 100000,
        LuckyWheel = 100001,
    }
}
