using System.Collections.Generic;

public enum EventName
{
    NONE,
    OnBeforeGameStateChange,
    OnAfterGameStateChange,

    OnUpdateCurrency,

    OnUnFocused,
    OnPauseGame,

    OnRemovedAds,

    InternetConnectivityChanged,

    OnUnlockSkin,
    OnProcessSkinProgress,

    OnUnlockWeapon,
    OnProcessWeaponProgress,

    OnUnlockWing,
    OnProcessWingProgress,

    OnUnlockAvatar,
    OnProcessAvatarProgress,

    OnUnlockFrame,
    OnProcessFrameProgress,

    OnEquipSkin,
    OnEquipWing,
    OnEquipWeapon,

    OnClaimDailyReward,
    OnChangeShowingBanner,

    OnBackpackChangeShowingItem,
    OnPlayerFinishCalculateStats,

    OnCameraSensitivityChanged,
    OnMidnight,
    OnAdGiftWatched,

    OnProcessAchievementProgress,
    OnProcessDailyTaskProgress,
    LuckyWheelOutOfFree,
    OnIndexProgressChanged,
    OnIndexChangeShowingEntry
}

public class EventTypeComparer : IEqualityComparer<EventName>
{
    public bool Equals(EventName x, EventName y)
    {
        return x == y;
    }

    public int GetHashCode(EventName t)
    {
        return (int)t;
    }
}
