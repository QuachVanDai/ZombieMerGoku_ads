using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace ExampleProject.UI.Shared
{
    public class FloatingText : MonoBehaviour
    {
        #region Fields

        [SerializeField] RectTransform rectTransform;
        [SerializeField] Text damageText;
        [SerializeField] float jumpPower = 45f;                 // arc height
        [SerializeField] float fallDistance = 30f;              // landing below start
        [SerializeField] float horizontalDistanceMin = 20f;     // random side drift min
        [SerializeField] float horizontalDistanceMax = 45f;     // random side drift max
        [SerializeField] float duration = 0.8f;
        [SerializeField] Ease jumpEase = Ease.Linear;
        [SerializeField] CanvasGroup canvasGroup;

        Sequence sequence;

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

        public void Show(string _text)
        {
            sequence?.Kill();

            damageText.text = _text;

            // Reset alpha in case object is reused
            var _color = damageText.color;
            _color.a = 1f;
            damageText.color = _color;

            Vector2 _startPos = rectTransform.anchoredPosition;

            float _sideSign = Random.value < 0.5f ? -1f : 1f;
            float _xOffset = Random.Range(horizontalDistanceMin, horizontalDistanceMax) * _sideSign;

            Vector2 _endPos = new Vector2(
                _startPos.x + _xOffset,
                _startPos.y - fallDistance
            );

            sequence = DOTween.Sequence();

            Vector2 _peakPos = new Vector2((_startPos.x + _endPos.x) * 0.5f, _startPos.y + jumpPower);
            sequence.Append(DOTween.To(
                () => rectTransform.anchoredPosition,
                _pos => rectTransform.anchoredPosition = _pos,
                _peakPos,
                duration * 0.5f).SetEase(jumpEase));
            sequence.Append(DOTween.To(
                () => rectTransform.anchoredPosition,
                _pos => rectTransform.anchoredPosition = _pos,
                _endPos,
                duration * 0.5f).SetEase(jumpEase));

            // Fade out during second half
            sequence.Join(DOTween.To(() => canvasGroup.alpha, _alpha => canvasGroup.alpha = _alpha, 0f, duration * 0.5f).SetDelay(duration * 0.5f));

            sequence.OnComplete(() => Destroy(gameObject));
        }

        public void SetAnchorPos(Vector2 _pos)
        {
            rectTransform.anchoredPosition = _pos;
        }

        #endregion
    }
}
