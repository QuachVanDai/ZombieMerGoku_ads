using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExampleProject.Manager;
using UnityEngine;
using ExampleProject.Tools;
using UnityEngine.SceneManagement;
using ExampleProject.Gameplay.Rarity;
using ExampleProject.UI.Shared;
namespace ExampleProject.Gameplay.Characters.Skin
{
    [CreateAssetMenu(fileName = "Skins", menuName = "ScriptableObjects/Skin/Skins")]
    public class Skins : ScriptableObject
    {
        #region Fields
        [SerializeField] List<SkinData> resourceDataList = new List<SkinData>();        [SerializeField] SkinId defaultSkinId = SkinId.Steve;
        [SerializeField] UI3DSkinDisplay ui3DSkinDisplayPrefab;
        [SerializeField] string dataFolderPath;
        const string resourceFolderPath = "Data/Skins";
        const string fallbackSkinDataFolderPath = "Data/SkinData";
        readonly static ResourceLoader<Skins> resourceLoader = new ResourceLoader<Skins>(resourceFolderPath);
        static List<SkinData> fallbackResourceDataList;
        #endregion
        #region Properties
        static Skins Resource => resourceLoader.Resource;
        public static SkinId DefaultSkin => Resource != null ? Resource.defaultSkinId : SkinId.Steve;
        #endregion
        #region LifeCycle   
        #endregion
        #region Private Methods
        void LoadAllData()
        {
            resourceDataList.Clear();
            // Ensure path is relative to Resources folder (strip Assets/Resources/ prefix if present)
            string _cleanPath = dataFolderPath.Replace("Assets/Resources/", "").Replace("\\", "/");
            SkinData[] _loadedData = Resources.LoadAll<SkinData>(_cleanPath);
            if (_loadedData.Length > 0)
            {
                resourceDataList.AddRange(_loadedData);
                resourceDataList = resourceDataList.OrderBy(x => x.id).ToList();
            }
        }
        #endregion
        #region Public Methods
        public static List<SkinData> GetResourceDataList()
        {
            List<SkinData> result = new List<SkinData>();
            if (PlayableDataRegistry.Instance != null)
            {
                List<SkinData> registryData = PlayableDataRegistry.Instance.GetSkinDataList();
                if (registryData.Count > 0)
                    result.AddRange(registryData);
            }

            Skins _resource = Resource;
            if (_resource != null && _resource.resourceDataList != null)
                AddUniqueSkinData(result, _resource.resourceDataList);

            AddUniqueSkinData(result, GetFallbackResourceDataList());

            return result;
        }
        public static SkinData GetResourceData(SkinId _id)
        {
            if (PlayableDataRegistry.Instance != null)
            {
                SkinData registryData = PlayableDataRegistry.Instance.GetSkinData(_id);
                if (registryData != null)
                    return registryData;
            }

            var _data = GetResourceDataList().Find(x => x != null && x.id.Equals(_id));
            return _data;
        }
        static List<SkinData> GetFallbackResourceDataList()
        {
            if (fallbackResourceDataList == null)
            {
                fallbackResourceDataList = Resources.LoadAll<SkinData>(fallbackSkinDataFolderPath)
                    .Where(x => x != null)
                    .OrderBy(x => x.id)
                    .ToList();
            }

            return fallbackResourceDataList;
        }
        static void AddUniqueSkinData(List<SkinData> target, List<SkinData> source)
        {
            if (target == null || source == null)
                return;

            for (int i = 0; i < source.Count; i++)
            {
                SkinData data = source[i];
                if (data != null && !target.Any(x => x != null && x.id == data.id))
                    target.Add(data);
            }
        }
        public static List<Item> GetItemList()
        {
            List<Item> _items = new List<Item>();            foreach (var _data in GetResourceDataList())
            {
                Item _item = new Item()                {
                    type = ItemType.Skin,
                    skinId = _data.id
                };
                _items.Add(_item);
            }
            return _items;
        }
        public static List<Item> GetItemList(List<SkinId> _skinIds)
        {
            List<Item> _items = new List<Item>();            foreach (var _id in _skinIds)
            {
                Item _item = new Item()                {
                    type = ItemType.Skin,
                    skinId = _id
                };
                _items.Add(_item);
            }
            return _items;
        }
        public static Item GetItem(SkinId _skinId)
        {
            Item _item = new Item()            {
                type = ItemType.Skin,
                skinId = _skinId
            };
            return _item;
        }
        public static List<Item> GetRandomLockedSkin(int _count)
        {
            var _unlockedSkinIds = SkinProgress.UnlockedSkins;
            var _lockedSkins = GetResourceDataList()
                .Where(x => !_unlockedSkinIds.Contains(x.id) && x.unlockCondition.IsCurrencyUnlock)
                .ToList();
            // Shuffle and take _count random locked skins, then convert to Item list
            var _randomLockedSkins = _lockedSkins
                .OrderBy(x => Random.value)
                .Take(_count)
                .Select(x => GetItem(x.id))
                .ToList();
            return _randomLockedSkins;
        }
        public static List<SkinId> GetFreeSkins()
        {
            return GetResourceDataList().Where(x => x.unlockCondition.method == UnlockMethod.Free).Select(x => x.id).ToList();
        }
        public static GameObject GetSkin3DModel(SkinId _skinId)
        {
            if (_skinId == SkinId.None)
                return null;
            Skins _resource = Resource;
            if (_resource == null || _resource.ui3DSkinDisplayPrefab == null)
                return null;

            var _model = Instantiate(_resource.ui3DSkinDisplayPrefab, Vector3.zero, Quaternion.identity);
            _model.Init(_skinId);
            return _model.gameObject;
        }
        #endregion
    }
}
