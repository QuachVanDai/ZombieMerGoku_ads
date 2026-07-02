using System;
using UnityEngine;

public enum IAP_Product : byte
{
    zb_removeforceads = 0,
    zb_coinpack1 = 1,
    zb_coinpack2 = 2,
    zb_coinpack3 = 3,
    zb_gempack1 = 4,
    zb_gempack2 = 5,
    zb_gempack3 = 6,
    None = 99,
}

public sealed class CC_Interface
{
    public static readonly CC_Interface current = new CC_Interface();
    public bool isJustLaunch;
    public bool isSharingScreenShot;

    public void ShowInterAds(string placement, int level, Action<bool> onComplete)
    {
        onComplete?.Invoke(true);
    }

    public void ShowRewardAds(string placement, int level, Action<bool> onComplete)
    {
        onComplete?.Invoke(true);
    }

    public void ShowBanner()
    {
    }

    public void HideBanner()
    {
    }

    public void PurchaseItem(IAP_Product product, string where)
    {
    }

    public void DelayChangeIsSharingScreenShot()
    {
        isSharingScreenShot = false;
    }
}

public sealed class CC_IAP
{
    public static readonly CC_IAP current = new CC_IAP();
    public string Localized(string productId)
    {
        return string.Empty;
    }

    public void LocalizedPrice()
    {
    }
}

public sealed class CC_Ads
{
    public static readonly CC_Ads Current = new CC_Ads();
    public bool isShowingBanner;

    public void ShowBanner()
    {
        isShowingBanner = false;
    }

    public void HideBanner()
    {
        isShowingBanner = false;
    }

    public void ShowMRec()
    {
    }

    public void DestroyMREC()
    {
    }
}

public sealed class Firebase_Analytics
{
    public static readonly Firebase_Analytics current = new Firebase_Analytics();
    public bool Is_Show_Mrec;
    public bool Is_Show_Banner;
}

public sealed class ATT : MonoBehaviour
{
    public void Init(GameObject appsFlyerObject)
    {
    }
}
