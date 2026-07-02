using ExampleProject.Tools;
using ExampleProject.UI.BaseUI.BasePopup;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.Manager
{
    [Serializable]
    public class ScenePopupReference
    {
        public PopupId id;
        public BasePopup popup;
    }

    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(RectTransform))]
    public class SharedCanvas : Singleton<SharedCanvas>
    {
        #region Fields

        readonly Canvas canvas;
        readonly RectTransform rectTransform;
        [SerializeField] List<ScenePopupReference> scenePopups = new List<ScenePopupReference>();

        #endregion

        #region Properties

        public Canvas Canvas => canvas != null ? canvas : GetComponent<Canvas>();
        public RectTransform RectTransform => rectTransform != null ? rectTransform : GetComponent<RectTransform>();
        public Vector2 CanvasSize => RectTransform == null ? Vector2.zero : RectTransform.sizeDelta;
        public float CanvasScaleFactor => Canvas == null ? 1f : Canvas.scaleFactor;
        public List<ScenePopupReference> ScenePopups => scenePopups;


        #endregion

        #region LifeCycle   

        void OnEnable()
        {
            SanitizeGraphics();
        }

        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public void AsignCamera(Camera _cam)
        {
            SanitizeGraphics();
            Canvas.worldCamera = _cam;
        }

        public void SanitizeGraphics()
        {
            Graphic[] graphics = GetComponentsInChildren<Graphic>(true);
            for (int i = 0; i < graphics.Length; i++)
            {
                Graphic graphic = graphics[i];
                if (graphic == null)
                    continue;

                if (graphic.GetComponent<CanvasRenderer>() != null)
                    continue;

                graphic.gameObject.AddComponent<CanvasRenderer>();
                Debug.LogWarning($"Added missing CanvasRenderer to UI Graphic: {GetHierarchyPath(graphic.transform)}", graphic);
            }
        }

        string GetHierarchyPath(Transform target)
        {
            if (target == null)
                return string.Empty;

            string path = target.name;
            Transform current = target.parent;
            while (current != null)
            {
                path = current.name + "/" + path;
                current = current.parent;
            }

            return path;
        }

        public void SetActive(bool _active)
        {
            this.gameObject.SetActive(_active);
        }

        #endregion
    }
}
