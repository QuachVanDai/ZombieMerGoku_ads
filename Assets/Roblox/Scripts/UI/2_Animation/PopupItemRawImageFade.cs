using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.UI.UIAnimation
{

    public class PopupItemRawImageFade : PopupItem
    {
        #region Fields

        [SerializeField] float showAlpha;
        [SerializeField] float hideAlpha;
        [SerializeField] Ease easeShow = Ease.Linear;
        [SerializeField] Ease easeHide = Ease.Linear;

        RawImage image;

        #endregion

        #region Properties

        RawImage ThisImage => image = image != null ? image : GetComponent<RawImage>();


        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public override Tween GetShowTween()
        {
            return ShowTween = DOTween.ToAlpha(() => ThisImage.color, _color => ThisImage.color = _color, showAlpha, timeShow)
                .SetEase(easeShow).SetDelay(delayShow);
        }
        public override Tween GetHideTween()
        {
            return HideTween = DOTween.ToAlpha(() => ThisImage.color, _color => ThisImage.color = _color, hideAlpha, timeHide)
                .SetEase(easeHide).SetDelay(delayHide);
        }
        public override void HideImmediately()
        {
            Color _tempColor = ThisImage.color;
            _tempColor.a = hideAlpha;
            ThisImage.color = _tempColor;
        }
        public override void ShowImmediately()
        {
            Color _tempColor = ThisImage.color;
            _tempColor.a = showAlpha;
            ThisImage.color = _tempColor;
        }
        public override void SetThisAsShow()
        {
            showAlpha = ThisImage.color.a;
        }
        public override void SetThisAsHide()
        {
            hideAlpha = ThisImage.color.a;
        }

        #endregion       
    }
}
