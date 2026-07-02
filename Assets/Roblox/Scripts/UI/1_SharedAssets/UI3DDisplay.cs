using UnityEngine;
namespace ExampleProject.UI.Shared
{
    public class UI3DDisplay : MonoBehaviour
    {
        #region Fields
        [SerializeField] Camera cam;
        [SerializeField] Transform modelPlacement;
        [SerializeField] Material templateMaterial;
        [SerializeField] int renderTextureWidth = 512;
        [SerializeField] int renderTextureHeight = 512;
        #endregion
        #region Properties
        public Material MaterialInstance { get; private set; }
        public RenderTexture RenderTextureInstance { get; private set; }
        public Transform ModelPlacement => modelPlacement;
        #endregion
        #region LifeCycle   
        void OnDisable()
        {
            CleanupResources();
        }
        #endregion
        #region Private Methods
        private void CreateRenderTexture()
        {
            RenderTextureInstance = new RenderTexture(renderTextureWidth, renderTextureHeight, 24)
            {
                name = "Item3D_RenderTexture",
            };
        }
        private void CreateMaterialInstance()
        {
            if (templateMaterial == null)
                return;

            MaterialInstance = new Material(templateMaterial)
            {
                mainTexture = RenderTextureInstance,
                name = "Item3D_Material_Instance"
            };
        }
        private void SetupCamera()
        {
            cam.targetTexture = RenderTextureInstance;
        }
        private void CleanupResources()
        {
            if (cam != null)
            {
                cam.targetTexture = null;
            }
            if (MaterialInstance != null)
            {
                Destroy(MaterialInstance);
                MaterialInstance = null;
            }
            if (RenderTextureInstance != null)
            {
                Destroy(RenderTextureInstance);
                RenderTextureInstance = null;
            }
        }
        #endregion
        #region Public Methods
        public void SetRenderSize(Vector2 _newSize)
        {
            renderTextureHeight = (int)_newSize.y;
            renderTextureWidth = (int)_newSize.x;
            CreateRenderTexture();
            CreateMaterialInstance();
            SetupCamera();
        }
        public void RotateY(float _value)
        {
            modelPlacement.eulerAngles = new Vector3(modelPlacement.eulerAngles.x, _value, modelPlacement.eulerAngles.z);
        }
        public void SetOffsetX(float _offset)
        {
            this.transform.localPosition += new Vector3(_offset, 0, 0);
        }
        #endregion
    }
}
