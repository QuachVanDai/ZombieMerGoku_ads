using ExampleProject.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.UI.Shared
{
    public class ItemDisplayImage : MonoBehaviour
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
        [SerializeField] private Quaternion skinRotation;
        [SerializeField] private Quaternion weaponRotation;
        [SerializeField] private Quaternion wingRotation;
        [SerializeField] private float skinScale;
        [SerializeField] private float weaponScale;
        [SerializeField] private float wingScale;
        [SerializeField] private Vector3 skinPosition;
        [SerializeField] private Vector3 weaponPosition;
        [SerializeField] private Vector3 wingPosition;

        #endregion

        #region Public Methods

        public void Spawn(Item _item)
        {
            Helpers.DestroyAllChilds(display.ModelPlacement);
            var _model = _item.GetModel();
            if (_model == null)
                return;

            _model.transform.SetParent(display.ModelPlacement);
            var _scale = Vector3.one;
            var _rotation = Quaternion.identity;
            var _position = Vector3.zero;
            switch (_item.type)
            {
                case ItemType.Skin:
                    _scale = Vector3.one * skinScale;
                    _rotation = skinRotation;
                    _position = skinPosition;
                    break;
                case ItemType.Weapon:
                    _scale = Vector3.one * weaponScale;
                    _rotation = weaponRotation;
                    _position = weaponPosition;
                    break;
                case ItemType.Wing:
                    _scale = Vector3.one * wingScale;
                    _rotation = wingRotation;
                    _position = wingPosition;
                    break;
            }

            _model.transform.localScale = _scale;
            _model.transform.localPosition = _position;
            _model.transform.localRotation = _rotation;
            Helpers.SetLayerAllChildren(_model.transform, StringsSafeAccess.LAYER_UI_3D);
        }

        public void SetActive(bool _isActive)
        {
            display.gameObject.SetActive(_isActive);
        }

        public void SetColor(Color _color)
        {
            if (renderImage != null)
                renderImage.color = _color;
        }

        #endregion
    }
}
