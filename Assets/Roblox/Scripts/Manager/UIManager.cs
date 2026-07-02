using ExampleProject.Tools;
using ExampleProject.UI.Input;
using ExampleProject.UI.BaseUI.BasePopup;
using ExampleProject.UI.Playable;
using System.Collections.Generic;
using UnityEngine;
namespace ExampleProject.Manager
{
    public class UIManager : Singleton<UIManager>
    {
        #region Fields
        readonly Dictionary<PopupId, BasePopup> existingPopupDictionary = new Dictionary<PopupId, BasePopup>();
        [Header("Direct Playable Popups")]
        [SerializeField] InputPopup inputPopup;
        [SerializeField] ChestRewardPopup chestRewardPopup;
        [SerializeField] GetTactixUnitPopup getTactixUnitPopup;
        [SerializeField] PlayableCtaPopup playableCtaPopup;
        [SerializeField] PlayableEndcardPopup playableEndcardPopup;

        [Header("Fallback Prefabs")]
        [SerializeField] bool allowPopupPrefabFallback;

        SharedCanvas sharedCanvas;
        #endregion
        #region Properties
        public Camera UICam;
        public float CanvasScaleFactor => SharedCanvas.CanvasScaleFactor;
        public SharedCanvas SharedCanvas => sharedCanvas != null ? sharedCanvas : SharedCanvas.instance;
        public InputPopup InputPopup => GetDirectPopup(inputPopup, PopupId.Input);
        // public ChestRewardPopup ChestRewardPopup => GetDirectPopup(chestRewardPopup, PopupId.PlayableChestReward);
        public GetTactixUnitPopup GetTactixUnitPopup => GetDirectPopup(getTactixUnitPopup, PopupId.GetTactixUnit);
        public PlayableCtaPopup PlayableCtaPopup => GetDirectPopup(playableCtaPopup, PopupId.PlayableCTA);
        public PlayableEndcardPopup PlayableEndcardPopup => GetDirectPopup(playableEndcardPopup, PopupId.PlayableEndcard);
        #endregion
        #region LifeCycle
        public void Init(Camera _uiCam, SharedCanvas _sharedCanvas)
        {
            this.UICam = _uiCam;
            sharedCanvas = _sharedCanvas;
            RegisterScenePopups();
            SharedCanvas?.SanitizeGraphics();
        }
        #endregion
        #region Private Methods
        void RegisterScenePopups()
        {
            existingPopupDictionary.Clear();

            RegisterDirectPopups();

            if (SharedCanvas == null)
                return;

            if (SharedCanvas.ScenePopups != null)
            {
                foreach (var scenePopup in SharedCanvas.ScenePopups)
                {
                    if (scenePopup == null)
                        continue;

                    RegisterScenePopup(scenePopup.id, scenePopup.popup);
                }
            }

            var childPopups = SharedCanvas.GetComponentsInChildren<BasePopup>(true);
            foreach (var childPopup in childPopups)
                RegisterScenePopup(childPopup.Id, childPopup);
        }

        void RegisterDirectPopups()
        {
            RegisterScenePopup(PopupId.Input, inputPopup);
            RegisterScenePopup(PopupId.PlayableChestReward, chestRewardPopup);
            RegisterScenePopup(PopupId.GetTactixUnit, getTactixUnitPopup);
            RegisterScenePopup(PopupId.PlayableCTA, playableCtaPopup);
            RegisterScenePopup(PopupId.PlayableEndcard, playableEndcardPopup);
        }

        void RegisterScenePopup(PopupId popupId, BasePopup popup)
        {
            if (popupId == PopupId.None || popup == null)
                return;

            if (existingPopupDictionary.ContainsKey(popupId))
            {
                if (existingPopupDictionary[popupId] != popup)
                    Debug.LogWarning($"Duplicate scene popup id ignored: {popupId}");
                return;
            }

            popup.SetId(popupId);
            popup.SetActive(false);
            existingPopupDictionary.Add(popupId, popup);
        }




        T GetDirectPopup<T>(T directPopup, PopupId popupId) where T : BasePopup
        {
            if (directPopup != null)
                return directPopup;

            return GetPopup<T>(popupId);
        }
        #endregion
        #region Public Methods
        public BasePopup GetPopup(PopupId _popupId)
        {
            if (existingPopupDictionary.TryGetValue(_popupId, out BasePopup _value))
            {
                return _value;
            }
            else return null;

        }
        public T GetPopup<T>(PopupId _popupId) where T : BasePopup
        {
            return GetPopup(_popupId) as T;
        }
        public void RemovePopup(PopupId _popupId)
        {
            existingPopupDictionary.Remove(_popupId);
        }
        public bool IsHasPopup(PopupId _popupId, out BasePopup _popup)
        {
            if (existingPopupDictionary.TryGetValue(_popupId, out _popup))
                return true;
            else
            {
                _popup = null; // ensure _popup is set to null if not found
                return false;
            }
        }
        public bool IsHasPopup<T>(PopupId _popupId, out T _popup) where T : BasePopup
        {
            if (IsHasPopup(_popupId, out BasePopup _basePopup))
            {
                _popup = _basePopup as T;
                return _popup != null;
            }
            else
            {
                _popup = null;
                return false;
            }
        }
        public void StackCamera(Camera _mainCam)
        {
            if (_mainCam == null || UICam == null)
                return;
            UICam.depth = _mainCam.depth + 1;
        }
        #endregion
    }
}
