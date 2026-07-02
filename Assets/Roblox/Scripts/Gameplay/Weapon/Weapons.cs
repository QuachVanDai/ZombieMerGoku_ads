using ExampleProject.Gameplay.Wing;
using ExampleProject.Manager;
using ExampleProject.Tools;
using ExampleProject.UI.Shared;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace ExampleProject.Gameplay.Weapon
{
    [CreateAssetMenu(fileName = "Weapons", menuName = "ScriptableObjects/Weapon/Weapons")]
    public class Weapons : ScriptableObject
    {
        #region Fields
        [SerializeField] List<WeaponData> resourceDataList = new List<WeaponData>();        [SerializeField] WeaponId defaultWeapon;
        [SerializeField] string dataFolderPath;
        const string resourceFolderPath = "Data/Weapons";
        readonly static ResourceLoader<Weapons> resourceLoader = new ResourceLoader<Weapons>(resourceFolderPath);    
        #endregion
        #region Properties
        static Weapons Resource => resourceLoader.Resource;
        public static WeaponId DefaultWeapon => Resource != null ? Resource.defaultWeapon : WeaponId.weapon3;
        #endregion
        #region LifeCycle   
        #endregion
        #region Private Methods
        void LoadAllData()
        {
            resourceDataList.Clear();
            // Ensure path is relative to Resources folder (strip Assets/Resources/ prefix if present)
            string _cleanPath = dataFolderPath.Replace("Assets/Resources/", "").Replace("\\", "/");
            WeaponData[] _loadedData = Resources.LoadAll<WeaponData>(_cleanPath);
            if (_loadedData.Length > 0)
            {
                resourceDataList.AddRange(_loadedData);
                resourceDataList = resourceDataList.OrderBy(x => x.id).ToList();
            }
        }
        #endregion
        #region Public Methods
        public static List<WeaponData> GetResourceDataList()
        {
            if (PlayableDataRegistry.Instance != null)
            {
                List<WeaponData> registryData = PlayableDataRegistry.Instance.GetWeaponDataList();
                if (registryData.Count > 0)
                    return registryData;
            }

            Weapons _resource = Resource;
            if (_resource == null || _resource.resourceDataList == null)
                return new List<WeaponData>();

            return _resource.resourceDataList;
        }
        public static WeaponData GetResourceData(WeaponId _id)
        {
            if (PlayableDataRegistry.Instance != null)
            {
                WeaponData registryData = PlayableDataRegistry.Instance.GetWeaponData(_id);
                if (registryData != null)
                    return registryData;
            }

            var _data = GetResourceDataList().Find(x => x != null && x.id.Equals(_id));
            return _data;
        }
        public static List<Item> GetItemList()
        {
            List<Item> _items = new List<Item>();            foreach (var _data in GetResourceDataList())
            {
                Item _item = new Item()                {
                    type = ItemType.Weapon,
                    weaponId = _data.id
                };
                _items.Add(_item);
            }
            return _items;
        }
        public static List<Item> GetItemList(List<WeaponId> _ids)
        {
            List<Item> _items = new List<Item>();            foreach (var _id in _ids)
            {
                var _data = GetResourceData(_id);
                if (_data == null)
                    continue;
                Item _item = new Item()                {
                    type = ItemType.Weapon,
                    weaponId = _data.id
                };
                _items.Add(_item);
            }
            return _items;
        }
        public static Item GetItem(WeaponId _id)
        {
            var _data = GetResourceData(_id);
            if (_data == null)
                return null;
            Item _item = new Item()            {
                type = ItemType.Weapon,
                weaponId = _data.id
            };
            return _item;
        }
        public static List<Item> GetRandomLockedWeapons(int _count)
        {
            var _unlockedWeaponIds = WeaponProgress.UnlockedWeapons;
            var _lockedWeapons = GetResourceDataList()
                .Where(x => !_unlockedWeaponIds.Contains(x.id) && x.unlockCondition.IsCurrencyUnlock)
                .ToList();
            // Shuffle and take _count random locked wings, then convert to Item list
            var _randomLockedWeapons = _lockedWeapons
                .OrderBy(x => Random.value)
                .Take(_count)
                .Select(x => GetItem(x.id))
                .ToList();
            return _randomLockedWeapons;
        }
        public static List<WeaponId> GetFreeWeapons()
        {
            return GetResourceDataList().Where(x => x.unlockCondition.method == UnlockMethod.Free).Select(x => x.id).ToList();
        }
        public static GameObject GetWeapon3DModel(WeaponId _id)
        {
            if (_id == WeaponId.None)
                return null;

            WeaponData _data = GetResourceData(_id);
            if (_data == null || _data.prefab == null)
                return null;

            return Instantiate(_data.prefab).gameObject;
        }
        #endregion      
    }
    public enum WeaponId
    {
        None = 0,
        weapon1 = 1,
        weapon2 = 2,
        weapon3 = 3, // Default weapon
        weapon4 = 4,
        weapon5 = 5,
        weapon6 = 6,
        weapon7 = 7,
        weapon8 = 8,
        weapon9 = 9,
        weapon10 = 10,
        meeleeGruntWeapon2 = 1002,
        meeleeGruntWeapon3 = 1003,
        meeleeGruntWeapon4 = 1004,
        meeleeGruntWeapon5 = 1005,
        meeleeGruntWeapon6 = 1006,
        meeleeGruntWeapon7 = 1007,
        meeleeGruntWeapon8 = 1008,
        meeleeGruntWeapon9 = 1009,
        meeleeGruntWeapon10 = 1010,
    }
}
