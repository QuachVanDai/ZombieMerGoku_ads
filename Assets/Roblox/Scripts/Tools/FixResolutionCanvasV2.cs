using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.Tools
{
    [RequireComponent(typeof(CanvasScaler))]
    public class FixResolutionCanvasV2 : MonoBehaviour
    {
        #region Fields

        [SerializeField] Camera mainCamera;
        [Header("Match Width Or Height")]
        [SerializeField, Range(0f, 1f)] float portraitScreenMatch = 0.35f;
        [SerializeField] float longScreenMatch = 1;
        [SerializeField] float shortScreenMatch = 0;
        [Header("Ratio Thresholds")]
        [SerializeField] float wideScreenRatio = 16f / 9f;
        [SerializeField] float portraitScreenRatio = 1f;
        CanvasScaler canvasScaler;
        float lastWidth = -1f;
        float lastHeight = -1f;

        #endregion

        #region Properties

        CanvasScaler ThisCanvasScaler => canvasScaler = canvasScaler != null ? canvasScaler : GetComponent<CanvasScaler>();

        #endregion

        #region LifeCycle

        void Awake()
        {
            CacheCanvasScaler();
        }

        void Start()
        {
            SetCanvasScalerMatch(true);
        }

        void OnEnable()
        {
            ResetCachedScreenSize();
            SetCanvasScalerMatch();
        }

        void OnRectTransformDimensionsChange()
        {
            SetCanvasScalerMatch();
        }

        #endregion

        #region Private Methods       

        void CacheCanvasScaler()
        {
            if (canvasScaler == null)
                canvasScaler = GetComponent<CanvasScaler>();
        }

        void SetCanvasScalerMatch(bool force = false)
        {
            CanvasScaler scaler = ThisCanvasScaler;
            if (scaler == null)
                return;

            float width = GetScreenWidth();
            float height = GetScreenHeight();
            if (width <= 0 || height <= 0)
                return;

            if (!force && width == lastWidth && height == lastHeight)
                return;

            lastWidth = width;
            lastHeight = height;
            scaler.matchWidthOrHeight = GetMatchValue(width, height);
        }

        void ResetCachedScreenSize()
        {
            lastWidth = -1f;
            lastHeight = -1f;
        }

        bool IsWideScreen(float width, float height)
        {
            return GetScreenRatio(width, height) > wideScreenRatio;
        }

        bool IsPortraitScreen(float width, float height)
        {
            return GetScreenRatio(width, height) < portraitScreenRatio;
        }

        float GetMatchValue(float width, float height)
        {
            if (IsPortraitScreen(width, height))
                return portraitScreenMatch;

            if (IsWideScreen(width, height))
                return longScreenMatch;

            return shortScreenMatch;
        }

        float GetScreenRatio(float width, float height)
        {
            return width / height;
        }

        float GetScreenWidth()
        {
            if (mainCamera != null && mainCamera.pixelWidth > 0)
                return mainCamera.pixelWidth;

            return Screen.width;
        }

        float GetScreenHeight()
        {
            if (mainCamera != null && mainCamera.pixelHeight > 0)
                return mainCamera.pixelHeight;

            return Screen.height;
        }

        #endregion

        #region Public Methods

        public void Refresh()
        {
            ResetCachedScreenSize();
            SetCanvasScalerMatch(true);
        }


        #endregion
    }
}
