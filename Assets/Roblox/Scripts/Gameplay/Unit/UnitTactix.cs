using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExampleProject.Gameplay.Unit;
using ExampleProject.Tools;
using UnityEngine;
using Units = ExampleProject.Gameplay.Unit.Units;
namespace ExampleProject
{
    [CreateAssetMenu(fileName = "TactixUnits", menuName = "ScriptableObjects/Tactix/Units")]
    public class UnitTactix : ScriptableObject
    {
         #region Fields
        [SerializeField] List<UnitTactixData> unitTactixesDataList = new List<UnitTactixData>();        
        [SerializeField] string unitTactixesDataFolderPath;
        const string resourceFolderPath = "Data/Tactix/TactixUnits";
        readonly static ResourceLoader<UnitTactix> resourceLoader = new ResourceLoader<UnitTactix>(resourceFolderPath);
        #endregion
        #region Properties
        #endregion
        #region LifeCycle   
        #endregion
        #region Private Methods
        void LoadAllData()
        {
            LoadData(unitTactixesDataList, unitTactixesDataFolderPath);
        }
        void LoadData(List<UnitTactixData> _list, string _folderPath)
        {
            _list.Clear();
            // Ensure path is relative to Resources folder (strip Assets/Resources/ prefix if present)
            string _cleanPath = _folderPath.Replace("Assets/Resources/", "").Replace("\\", "/");
            UnitTactixData[] _loadedData = Resources.LoadAll<UnitTactixData>(_cleanPath);
            if (_loadedData.Length > 0)
            {
                _list.AddRange(_loadedData);
                _list = _list.OrderBy(x => x.unitID).ToList();
            }
        }
        #endregion
        #region Public Methods
        public static List<UnitTactixData> GetUnitTactixResourceDataList()
        {
            return resourceLoader.Resource.unitTactixesDataList;
        }
        public static float GetSellPriceByUnitID(UnitId _unitID, int _level)
        {
            var _data = GetUnitTactixResourceDataList().Find(x => x.unitID == _unitID);
            if (_data != null)
                return (_data.SellingPriceX * _level * TactixChests.GetChestResourceDataByRarity(Units.GetCreepResourceData(_unitID).rarity).cost);
            else return 0;
        }
        #endregion      
    }
}
