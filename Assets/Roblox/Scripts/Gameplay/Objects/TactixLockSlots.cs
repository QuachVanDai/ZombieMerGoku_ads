using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExampleProject.Tools;
using UnityEngine;
namespace ExampleProject
{
    [CreateAssetMenu(fileName = "TactixLockSlots ", menuName = "ScriptableObjects/Tactix/LockSlots")]
    public class TactixLockSlots : ScriptableObject
    {
        #region Fields
        [SerializeField] List<TactixLockSlotData> slotResourceDataList = new List<TactixLockSlotData>();        const string resourceFolderPath = "Data/Tactix/TactixLockSlots";
        readonly static ResourceLoader<TactixLockSlots> resourceLoader = new ResourceLoader<TactixLockSlots>(resourceFolderPath);
        #endregion
        #region Properties
        #endregion
        #region LifeCycle   
        #endregion
        #region Private Methods
        private static void SortSlotDataById()
        {
            resourceLoader.Resource.slotResourceDataList =
                resourceLoader.Resource.slotResourceDataList
                    .OrderBy(x => x.id)
                    .ToList();
        }
        #endregion
        #region Public Methods
        public static List<TactixLockSlotData> GetSlotResourceDataList()
        {
            return resourceLoader.Resource.slotResourceDataList;
        }
        public static TactixLockSlotData GetSlotResourceData(int _id)
        {
            var _data = GetSlotResourceDataList().Find(x => x.id.Equals(_id));
            if (_data == null)
                return null;
            return _data;
        }
        public static bool HasSlotData(int _id)
        {
            return GetSlotResourceDataList().Exists(x => x.id.Equals(_id));
        }
        #endregion      
    }
    [Serializable]
    public enum TactixLockSlotUnlockType
    {
        Ads,
        Level,
        Coin,
        Gem
    }
    [Serializable]
    public class TactixLockSlotData 
    {
        #region Fields
        public int id;
        public int cost;
        public int levelUnlock;
        public int adsUnlock;
        public TactixLockSlotUnlockType unlockType;
        #endregion
        #region Properties
        #endregion
        #region LifeCycle   
        #endregion
        #region Private Methods
        #endregion
        #region Public Methods
        #endregion
    }
}
