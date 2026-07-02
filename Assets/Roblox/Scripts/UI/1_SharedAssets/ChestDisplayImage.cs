using System.Collections;
using System.Collections.Generic;
using ExampleProject.Tools;
using ExampleProject.UI.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject
{
    public class ChestDisplayImage : MonoBehaviour
    {
        #region Fields

        [SerializeField] RawImage renderImage;
        [SerializeField] UI3DDisplay display;
        [SerializeField] float scale = 1.5f;
        [SerializeField] Vector3 rotation;
        [SerializeField] Vector3 position;

        #endregion

        #region Properties


        #endregion

        #region LifeCycle   

        private void OnEnable()
        {
            display.SetRenderSize(renderImage.rectTransform.rect.size);
            renderImage.material = display.MaterialInstance;
            renderImage.texture = display.RenderTextureInstance;
        }

        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public void Spawn(ChestId _id)
        {
            Helpers.DestroyAllChilds(display.ModelPlacement);
            var _model = TactixChests.GetChest3DModel(_id);
            _model.transform.SetParent(display.ModelPlacement);
            var _scale = Vector3.one * scale;
            var _rotation = Quaternion.Euler(rotation);
            var _position = position;
           
            _model.transform.localScale = _scale;
            _model.transform.localPosition = _position;
            _model.transform.localRotation = _rotation;
            Helpers.SetLayerAllChildren(_model.transform, StringsSafeAccess.LAYER_UI_3D);
        }

        #endregion
    }
}
