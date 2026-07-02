using ExampleProject.Tools;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace ExampleProject.UI.BaseUI.BasePopup
{
    [CreateAssetMenu(fileName = "Popups", menuName = "ScriptableObjects/Popup/Popups")]
    public class Popups : ScriptableObject
    {
        #region Fields
        [SerializeField] List<PopupData> resourceDataList = new List<PopupData>();        [SerializeField] string dataFolderPath;
        const string resourceFolderPath = "Data/Popups";
        readonly static ResourceLoader<Popups> resourceLoader = new ResourceLoader<Popups>(resourceFolderPath);
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
            PopupData[] _loadedData = Resources.LoadAll<PopupData>(_cleanPath);
            if (_loadedData.Length > 0)
            {
                resourceDataList.AddRange(_loadedData);
                resourceDataList = resourceDataList.OrderBy(x => x.id).ToList();
            }
        }
        #endregion
        #region Public Methods
        public static List<PopupData> GetResourceDataList()
        {
            if (resourceLoader.Resource == null || resourceLoader.Resource.resourceDataList == null)
                return new List<PopupData>();

            return resourceLoader.Resource.resourceDataList;
        }
        public static PopupData GetResourceData(PopupId _id)
        {
            var _data = GetResourceDataList().Find(x => x.id.Equals(_id));
            return _data;
        }
        #endregion      
    }
}
