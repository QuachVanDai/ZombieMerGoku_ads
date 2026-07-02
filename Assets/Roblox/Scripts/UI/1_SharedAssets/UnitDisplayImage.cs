using ExampleProject.Gameplay.Unit;
using ExampleProject.Tools;
using UnityEngine;
using UnityEngine.UI;
using Units = ExampleProject.Gameplay.Unit.Units;

namespace ExampleProject.UI.Shared
{
    public class UnitDisplayImage : MonoBehaviour
    {
        #region LifeCycle

        private void OnEnable()
        {
            display.SetRenderSize(renderImage.rectTransform.rect.size);
            renderImage.material = display.MaterialInstance;
            renderImage.texture = display.RenderTextureInstance;
        }

        #endregion

        #region Fields

        [SerializeField] private RawImage renderImage;
        [SerializeField] private UI3DDisplay display;
        [SerializeField] private float scale = 1.5f;
        [SerializeField] private Vector3 rotation;
        [SerializeField] private Vector3 position;
        [SerializeField] private Vector3 receiveVfxPosition;
        [SerializeField] private Vector3 receiveVfxRotation;
        [SerializeField] private Vector3 receiveVfxScale = Vector3.one;
        private GameObject receiveVfxInstance;

        #endregion

        #region Public Methods

        public void Spawn(UnitId _id)
        {
            Helpers.DestroyAllChilds(display.ModelPlacement);
            var _model = Units.GetUnit3DModel(_id);
            if (_model == null)
                return;

            _model.transform.SetParent(display.ModelPlacement, false);
            var _scale = Vector3.one * scale;
            var _rotation = Quaternion.Euler(rotation);
            var _position = position;

            _model.transform.localScale = _scale;
            _model.transform.localPosition = _position;
            _model.transform.localRotation = _rotation;
            Helpers.SetLayerAllChildren(_model.transform, StringsSafeAccess.LAYER_UI_3D);

            var unit = _model.GetComponent<CreepUnit>();
            if (unit != null)
                unit.PlayIdleAnim();
        }

        public void SpawnReceiveVfx(GameObject prefab)
        {
            if (receiveVfxInstance != null)
                Destroy(receiveVfxInstance);

            if (prefab == null || display == null || display.ModelPlacement == null)
                return;

            receiveVfxInstance = Instantiate(prefab, display.ModelPlacement);
            receiveVfxInstance.transform.localPosition = receiveVfxPosition;
            receiveVfxInstance.transform.localRotation = Quaternion.Euler(receiveVfxRotation);
            receiveVfxInstance.transform.localScale = receiveVfxScale;
            Helpers.SetLayerAllChildren(receiveVfxInstance.transform, StringsSafeAccess.LAYER_UI_3D);
        }

        public void SetColor(Color _color)
        {
            if (renderImage != null)
                renderImage.color = _color;
        }

        #endregion
    }
}
