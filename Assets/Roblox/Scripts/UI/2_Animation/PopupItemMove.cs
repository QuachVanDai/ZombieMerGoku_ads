using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace ExampleProject.UI.UIAnimation
{
    public class PopupItemMove : PopupItem
    {
        #region Fields

        [SerializeField] Vector3 showPos;
        [SerializeField] Vector3 hidePos;
        [SerializeField] Ease easeShow = Ease.Linear;
        [SerializeField] Ease easeHide = Ease.Linear;

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
            Vector2 _showPos = new Vector2(showPos.x, showPos.y);
            return ShowTween = DOTween.To(
                () => ThisRectTransform.anchoredPosition,
                _pos => ThisRectTransform.anchoredPosition = _pos,
                _showPos,
                timeShow).SetEase(easeShow).SetDelay(delayShow);
        }
        public override Tween GetHideTween()
        {
            Vector2 _hidePos = new Vector2(hidePos.x, hidePos.y);
            return HideTween = DOTween.To(
                () => ThisRectTransform.anchoredPosition,
                _pos => ThisRectTransform.anchoredPosition = _pos,
                _hidePos,
                timeHide).SetEase(easeHide).SetDelay(delayHide);
        }
        public override void HideImmediately()
        {
            ThisRectTransform.anchoredPosition = hidePos;
        }
        public override void ShowImmediately()
        {
            ThisRectTransform.anchoredPosition = showPos;
        }
        public override void SetThisAsShow()
        {
            showPos = ThisRectTransform.anchoredPosition;
        }
        public override void SetThisAsHide()
        {
            hidePos = ThisRectTransform.anchoredPosition;
        }

        #endregion       
    }
}
