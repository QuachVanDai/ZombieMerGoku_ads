using ExampleProject.Gameplay.Characters.Skin;
using ExampleProject.Gameplay.Currency;
using ExampleProject.Gameplay.Rarity;
using ExampleProject.Tools;
using ExampleProject.UI.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ExampleProject.Manager
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        #region Fields

        const string resourceFolderPath = "Data/GameConfig";
        readonly static ResourceLoader<GameConfig> resourceLoader = new ResourceLoader<GameConfig>(resourceFolderPath);
        [SerializeField] int coinInterAds;

        [SerializeField] float shopRefreshTime;
        [SerializeField] CurrencyValue shopRefreshCost;
        [SerializeField] int maxRefreshStack;
        [SerializeField] List<DuplicateToCurrency> duplicateToCurrencyList = new List<DuplicateToCurrency>();
        [SerializeField] int luckyWheelTicketCost;
        [SerializeField] int luckyWheelGuaranteeSpin;
        [SerializeField] int maxRewardTicketAdPerDay;

        [SerializeField] List<CameraDistanceData> cameraDistanceData;

        #endregion

        #region Properties

        public static int CameraDistanceDataCount => resourceLoader.Resource.cameraDistanceData.Count;

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public static int CoinInterAds => resourceLoader.Resource.coinInterAds;
        public static float ShopRefreshTime => resourceLoader.Resource.shopRefreshTime;
        public static CurrencyValue ShopRefreshCost => resourceLoader.Resource.shopRefreshCost;
        public static int MaxRefreshStack => resourceLoader.Resource.maxRefreshStack;
        public static CurrencyValue GetDuplicateToCurrency(RarityId _rarity)
        {
            var _data = resourceLoader.Resource.duplicateToCurrencyList.FirstOrDefault(x => x.rarity == _rarity);
            if (_data != null)
                return _data.currency;
            else
                return null;
        }
        public static int LuckyWheelTicketCost => resourceLoader.Resource.luckyWheelTicketCost;
        public static int LuckyWheelGuaranteeSpin => resourceLoader.Resource.luckyWheelGuaranteeSpin;
        public static int MaxRewardTicketAdPerDay => resourceLoader.Resource.maxRewardTicketAdPerDay;
        public static float GetCameraDistanceByLevel(CameraDistanceLevel _level)
        {
            var _data = resourceLoader.Resource.cameraDistanceData.FirstOrDefault(x => x.level == _level);
            if (_data != null)
                return _data.distance;
            else
                return 0f;
        }
        public static CameraDistanceLevel GetNextLevelCameraDistance(CameraDistanceLevel _currentLevel)
        {
            var _currentData = resourceLoader.Resource.cameraDistanceData.FirstOrDefault(x => x.level == _currentLevel);
            if (_currentData != null)
            {
                var _currentIndex = resourceLoader.Resource.cameraDistanceData.IndexOf(_currentData);
                var _nextIndex = resourceLoader.Resource.cameraDistanceData.Count > _currentIndex + 1 ? _currentIndex + 1 : 0;
                return resourceLoader.Resource.cameraDistanceData[_nextIndex].level;
            }
            else
                return CameraDistanceLevel.None;
        }

        #endregion
    }

    [Serializable]
    public class DuplicateToCurrency
    {
        public CurrencyValue currency;
        public RarityId rarity;
    }
    [Serializable]
    public class CameraDistanceData
    {
        public CameraDistanceLevel level;
        public float distance;
    }
    public enum CameraDistanceLevel
    {
        None,
        Close,
        Medium,
        Far,
        VeryFar,
        ExtremeFar
    }
}