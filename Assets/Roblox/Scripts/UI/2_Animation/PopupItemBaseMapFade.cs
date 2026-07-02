using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.UI.UIAnimation
{

    public class PopupItemBaseMapFade : PopupItem
    {
        #region Fields

        [SerializeField] float showAlpha;
        [SerializeField] float hideAlpha;
        [SerializeField] Ease easeShow = Ease.Linear;
        [SerializeField] Ease easeHide = Ease.Linear;

        Image image;

        #endregion

        #region Properties

        Image ThisImage => image = image != null ? image : GetComponent<Image>();
        Material Material => ThisImage.material;


        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public override Tween GetShowTween()
        {
            return ShowTween = DOTween.ToAlpha(() => Material.color, _color => Material.color = _color, showAlpha, timeShow)
                .SetEase(easeShow).SetDelay(delayShow);
        }
        public override Tween GetHideTween()
        {
            return HideTween = DOTween.ToAlpha(() => Material.color, _color => Material.color = _color, hideAlpha, timeHide)
                .SetEase(easeHide).SetDelay(delayHide);
        }
        public override void HideImmediately()
        {
            if (Material == null)
                return;
            Color _tempColor = Material.color;
            _tempColor.a = hideAlpha;
            Material.color = _tempColor;
        }
        public override void ShowImmediately()
        {
            if (Material == null)
                return;
            Color _tempColor = Material.color;
            _tempColor.a = showAlpha;
            Material.color = _tempColor;
        }
        public override void SetThisAsShow()
        {
            if (Material == null)
                return;
            showAlpha = Material.color.a;
        }
        public override void SetThisAsHide()
        {
            if (Material == null)
                return;
            hideAlpha = Material.color.a;
        }

        #endregion       
    }
}
