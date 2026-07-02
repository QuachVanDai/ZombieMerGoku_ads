using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.UI.UIAnimation
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PopupItemCanvasGroupFade : PopupItem
    {
        #region Fields

        [SerializeField] float showAlpha = 1;
        [SerializeField] float hideAlpha;
        [SerializeField] Ease easeShow = Ease.Linear;
        [SerializeField] Ease easeHide = Ease.Linear;

        CanvasGroup canvasGroup;

        #endregion

        #region Properties

        CanvasGroup ThisCanvasGroup => canvasGroup = canvasGroup != null ? canvasGroup : GetComponent<CanvasGroup>();


        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public override Tween GetShowTween()
        {
            return ShowTween = DOTween.To(() => ThisCanvasGroup.alpha, _alpha => ThisCanvasGroup.alpha = _alpha, showAlpha, timeShow)
                .SetEase(easeShow).SetDelay(delayShow);
        }
        public override Tween GetHideTween()
        {
            return HideTween = DOTween.To(() => ThisCanvasGroup.alpha, _alpha => ThisCanvasGroup.alpha = _alpha, hideAlpha, timeHide)
                .SetEase(easeHide).SetDelay(delayHide);
        }
        public override void HideImmediately()
        {
            ThisCanvasGroup.alpha = hideAlpha;
        }
        public override void ShowImmediately()
        {
            ThisCanvasGroup.alpha = showAlpha;
        }
        public override void SetThisAsShow()
        {
            showAlpha = ThisCanvasGroup.alpha;
        }
        public override void SetThisAsHide()
        {
            hideAlpha = ThisCanvasGroup.alpha;
        }

        #endregion       
    }
}
