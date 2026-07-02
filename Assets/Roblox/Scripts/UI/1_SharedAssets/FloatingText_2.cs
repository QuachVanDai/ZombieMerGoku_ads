using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using ExampleProject.Gameplay.Currency;

namespace ExampleProject.UI.Shared
{
    public class FloatingText_2 : MonoBehaviour
    {
        #region Fields

        [SerializeField] CurrencyColorText currencyColorText;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] RectTransform rectTransform;

        [SerializeField] Ease moveEase = Ease.OutQuad;
        [SerializeField] float duration = 1.5f;

        Sequence sequence;
        Vector3 endPos;

        #endregion

        #region Properties


        #endregion

        #region LifeCycle

        private void OnDisable()
        {
            sequence?.Kill();
        }

        #endregion

        #region Public Methods

        public void Show(CurrencyType _type, string _text)
        {
            sequence?.Kill();

            currencyColorText.Init(_type);
            currencyColorText.SetText(_text);

            // Reset alpha in case object is reused
            canvasGroup.alpha = 1f;


            sequence = DOTween.Sequence();

            // Parabolic jump: up then down, with random left/right movement
            sequence.Append(rectTransform.DOMove(endPos, duration).SetEase(moveEase));

            // Fade out during second half
            sequence.Join(DOTween.To(() => canvasGroup.alpha, _alpha => canvasGroup.alpha = _alpha, 0f, duration * 0.5f).SetDelay(duration * 0.5f));

            sequence.OnComplete(() => Destroy(gameObject));
        }
        public void SetAnchorPos(Vector3 _pos)
        {
            rectTransform.anchoredPosition = _pos;
        }
        public void SetEndPos(Vector3 _endPos)
        {
            endPos = _endPos;
        }

        #endregion
    }
}
