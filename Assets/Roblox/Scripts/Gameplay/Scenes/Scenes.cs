using ExampleProject.Tools;
using ExampleProject.UI.BaseUI.BasePopup;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace ExampleProject.Gameplay.Scenes
{
    [CreateAssetMenu(fileName = "Scenes", menuName = "ScriptableObjects/Scene/Scenes")]
    public class Scenes : ScriptableObject
    {
        #region Fields
        [SerializeField] List<SceneData> resourceDataList = new List<SceneData>();        [SerializeField] string dataFolderPath;
        const string resourceFolderPath = "Data/Scenes";
        readonly static ResourceLoader<Scenes> resourceLoader = new ResourceLoader<Scenes>(resourceFolderPath);
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
            SceneData[] _loadedData = Resources.LoadAll<SceneData>(_cleanPath);
            if (_loadedData.Length > 0)
            {
                resourceDataList.AddRange(_loadedData);
                resourceDataList = resourceDataList.OrderBy(x => x.id).ToList();
            }
        }
        #endregion
        #region Public Methods
        public static List<SceneData> GetResourceDataList()
        {
            return resourceLoader.Resource.resourceDataList;
        }
        public static SceneData GetResourceData(SceneId _id)
        {
            var _data = GetResourceDataList().Find(x => x.id.Equals(_id));
            return _data;
        }
        public static Scene GetUnityScene(SceneId _id)
        {
            var _data = GetResourceDataList().Find(x => x.id.Equals(_id)).sceneName;
            return SceneManager.GetSceneByName(_data);
        }
        #endregion
    }
}