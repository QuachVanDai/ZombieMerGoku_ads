using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace ExampleProject.UI.UIAnimation
{
    public class PopupItemScale : PopupItem
    {
        #region Fields

        [SerializeField] Vector3 showScale;
        [SerializeField] Vector3 hideScale;
        [SerializeField] AnimationCurve easeShow;
        [SerializeField] AnimationCurve easeHide;

        RectTransform rectTransform;

        #endregion

        #region Properties

        RectTransform ThisRectTransform => rectTransform = rectTransform != null ? rectTransform : GetComponent<RectTransform>();

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public override Tween GetShowTween()
        {
            return ShowTween = ThisRectTransform.DOScale(showScale, timeShow).SetEase(easeShow).SetDelay(delayShow);
        }
        public override Tween GetHideTween()
        {
            return HideTween = ThisRectTransform.DOScale(hideScale, timeHide).SetEase(easeHide).SetDelay(delayHide);
        }
        public override void HideImmediately()
        {
            ThisRectTransform.localScale = hideScale;
        }
        public override void ShowImmediately()
        {
            ThisRectTransform.localScale = showScale;
        }
        public override void SetThisAsShow()
        {
            showScale = ThisRectTransform.localScale;
        }
        public override void SetThisAsHide()
        {
            hideScale = ThisRectTransform.localScale;
        }

        #endregion
    }
}
