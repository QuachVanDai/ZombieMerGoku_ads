using ExampleProject.Gameplay.Characters.Skin;
using ExampleProject.Gameplay.Weapon;
using ExampleProject.Gameplay.Wing;
using ExampleProject.Tools;
using ExampleProject.UI.BaseUI.BasePopup;
using ExampleProject.UI.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExampleProject.Manager
{
    [Serializable]
    public class ShopProgress
    {
        #region Fields

        [SerializeField] List<IAP_Product> purchasedProduct = new List<IAP_Product>();
        [SerializeField] List<RemoveAdsType> purchasedRemoveAds = new List<RemoveAdsType>();
        [SerializeField] List<StoreTabRefreshData> storeTabRefreshData = new List<StoreTabRefreshData>();
        #endregion

        #region Properties

        public static ShopProgress Progress
        {
            get => SharedRobloxUserData.ShopProgress;
            set => SharedRobloxUserData.ShopProgress = value;
        }

        public static bool IsRemovedForceAds
        {
            get => Progress.purchasedRemoveAds.Any(x => x == RemoveAdsType.RemoveForceAds);
        }

        public static bool IsRemovedRewardAds
        {
            get => Progress.purchasedRemoveAds.Any(x => x == RemoveAdsType.RemoveRewardAds);
        }

        public int GetPurchasedProductCount()
        {
            return purchasedProduct.Count;
        }

        #endregion

        #region LifeCycle

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        public static void SavePurchasedProduct(IAP_Product _product)
        {
            Progress.purchasedProduct.Add(_product);
            UserDataManager.SaveData();
        }
        public static void RemoveAds(RemoveAdsType _adsType)
        {
            Progress.purchasedRemoveAds.Add(_adsType);
            EventDispatcher.Instance.Dispatch(EventName.OnRemovedAds, null);
            UserDataManager.SaveData();
        }
        public static bool IsHaveProduct(IAP_Product _product)
        {
            return Progress.purchasedProduct.Contains(_product);
        }
      
      

        #endregion
    }

    public enum RemoveAdsType
    {
        None = 0,
        RemoveForceAds = 1,
        RemoveRewardAds = 2,
    }
    public class StoreTabRefreshData
    {
        public string lastRefreshTime = DateTime.MinValue.ToString("O");
       
        public List<Item> buyableItem = new List<Item>();
     
    }
}
