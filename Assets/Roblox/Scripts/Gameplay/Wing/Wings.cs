using ExampleProject.Gameplay.Weapon;
using ExampleProject.Manager;
using ExampleProject.Tools;
using ExampleProject.UI.Shared;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace ExampleProject.Gameplay.Wing
{
    [CreateAssetMenu(fileName = "Wings", menuName = "ScriptableObjects/Wing/Wings")]
    public class Wings : ScriptableObject
    {
        #region Fields
        [SerializeField] List<WingData> resourceDataList = new List<WingData>();        [SerializeField] string dataFolderPath;
        const string resourceFolderPath = "Data/Wings";
        readonly static ResourceLoader<Wings> resourceLoader = new ResourceLoader<Wings>(resourceFolderPath);   
        #endregion
        #region Properties
        static Wings Resource => resourceLoader.Resource;
        #endregion
        #region LifeCycle   
        #endregion
        #region Private Methods
        void LoadAllData()
        {
            resourceDataList.Clear();
            // Ensure path is relative to Resources folder (strip Assets/Resources/ prefix if present)
            string _cleanPath = dataFolderPath.Replace("Assets/Resources/", "").Replace("\\", "/");
            WingData[] _loadedData = Resources.LoadAll<WingData>(_cleanPath);
            if (_loadedData.Length > 0)
            {
                resourceDataList.AddRange(_loadedData);
                resourceDataList = resourceDataList.OrderBy(x => x.id).ToList();
            }
        }
        #endregion
        #region Public Methods
        public static List<WingData> GetResourceDataList()
        {
            Wings _resource = Resource;
            if (_resource == null || _resource.resourceDataList == null)
                return new List<WingData>();

            return _resource.resourceDataList;
        }
        public static WingData GetResourceData(WingId _id)
        {
            var _data = GetResourceDataList().Find(x => x.id.Equals(_id));
            return _data;
        }
        public static List<Item> GetItemList()
        {
            List<Item> _items = new List<Item>();            foreach (var _data in GetResourceDataList())
            {
                Item _item = new Item()                {
                    type = ItemType.Wing,
                    wingId = _data.id
                };
                _items.Add(_item);
            }
            return _items;
        }
        public static List<Item> GetItemList(List<WingId> _ids)
        {
            List<Item> _items = new List<Item>();            foreach (var _id in _ids)
            {
                var _data = GetResourceData(_id);
                if (_data == null)
                    continue;
                Item _item = new Item()                {
                    type = ItemType.Wing,
                    wingId = _data.id
                };
                _items.Add(_item);
            }
            return _items;
        }
        public static Item GetItem(WingId _id)
        {
            var _data = GetResourceData(_id);
            if (_data == null)
                return null;
            Item _item = new Item()            {
                type = ItemType.Wing,
                wingId = _data.id
            };
            return _item;
        }
        public static List<Item> GetRandomLockedWings(int _count)
        {
            var _unlockedWingIds = WingProgress.UnlockedWings;
            var _lockedWings = GetResourceDataList()
                .Where(x => !_unlockedWingIds.Contains(x.id) && x.unlockCondition.IsCurrencyUnlock)
                .ToList();
            // Shuffle and take _count random locked wings, then convert to Item list
            var _randomLockedWings = _lockedWings
                .OrderBy(x => Random.value)
                .Take(_count)
                .Select(x => GetItem(x.id))
                .ToList();
            return _randomLockedWings;
        }
        public static List<WingId> GetFreeWings()
        {
            return GetResourceDataList().Where(x => x.unlockCondition.method == UnlockMethod.Free).Select(x => x.id).ToList();
        }
        public static GameObject GetWing3DModel(WingId _id)
        {
            if(_id == WingId.None)
                return null;

            WingData _data = GetResourceData(_id);
            if (_data == null || _data.prefab == null)
                return null;

            return Instantiate(_data.prefab).gameObject;
        }
        #endregion      
    }
    public enum WingId
    {
        None = 0,
        wing1 = 1,
        wing2 = 2,
        wing3 = 3,
        wing4 = 4,
        wing5 = 5,
    }
}
