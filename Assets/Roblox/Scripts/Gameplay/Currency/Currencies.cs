using ExampleProject.Gameplay.Faction;

using ExampleProject.Manager;
using ExampleProject.Tools;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace ExampleProject.Gameplay.Currency
{
    [CreateAssetMenu(fileName = "Currencies", menuName = "ScriptableObjects/Currency/Currencies")]
    public class Currencies : ScriptableObject
    {
        #region Fields
        [SerializeField] List<CurrencyData> resourceDataList = new List<CurrencyData>();        [SerializeField] string dataFolderPath;
        const string resourceFolderPath = "Data/Currencies";
        readonly static ResourceLoader<Currencies> resourceLoader = new ResourceLoader<Currencies>(resourceFolderPath);
        #endregion
        #region Properties
        #endregion
        #region LifeCycle   
        #endregion
        #region Private Methods
        void LoadAllData()
        {
            resourceDataList.Clear();
            // Ensure path is relative to Resources folder (strip Assets/Resources/ prefix if present)
            string _cleanPath = dataFolderPath.Replace("Assets/Resources/", "").Replace("\\", "/");
            CurrencyData[] _loadedData = Resources.LoadAll<CurrencyData>(_cleanPath);
            if (_loadedData.Length > 0)
            {
                resourceDataList.AddRange(_loadedData);
                resourceDataList = resourceDataList.OrderBy(x => x.type).ToList();
            }
        }
        #endregion
        #region Public Methods
        public static List<CurrencyData> GetResourceDataList()
        {
            return resourceLoader.Resource.resourceDataList;
        }
        public static CurrencyData GetResourceData(CurrencyType _type)
        {
            var _data = GetResourceDataList().Find(x => x.type.Equals(_type));
            return _data;
        }
        #endregion
    }
}