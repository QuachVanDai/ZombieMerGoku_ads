using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.UI.Playable
{
    [RequireComponent(typeof(Image))]
    public class PlayableFadeImage : MonoBehaviour
    {
        #region Fields

        [SerializeField] Image image;
        [SerializeField] Color fadeColor = Color.black;
        [Range(0f, 1f)]
        [SerializeField] float startAlpha;
        [SerializeField] bool blockRaycastWhenVisible = true;
        [SerializeField] Ease fadeEase = Ease.Linear;
        [SerializeField] float fadeOutDuration = 0.18f;
        [SerializeField] float holdBlackDuration = 0.03f;
        [SerializeField] float fadeInDuration = 0.22f;

        Tween fadeTween;
        const float VisibleThreshold = 0.001f;

        #endregion

        #region Properties

        Image ThisImage => image != null ? image : image = GetComponent<Image>();
        public float Alpha => GetAlpha();

        #endregion

        #region LifeCycle



        #endregion

        #region Public Methods

        public Coroutine StartFadeSequence(Action onBlack = null)
        {
            ActivateForFade();
            return StartCoroutine(PlayFadeSequence(onBlack));
        }

        public IEnumerator PlayFadeSequence(Action onBlack = null)
        {
            yield return FadeTo(1f, fadeOutDuration);

            onBlack?.Invoke();

            if (holdBlackDuration > 0f)
                yield return new WaitForSeconds(holdBlackDuration);

            yield return FadeTo(0f, fadeInDuration);
        }

        public void FadeBlack(float duration)
        {
            ActivateForFade();
            StartCoroutine(FadeTo(1f, duration));
        }

        public void FadeClear(float duration)
        {
            ActivateForFade();
            StartCoroutine(FadeTo(0f, duration));
        }

        public IEnumerator FadeTo(float targetAlpha, float duration)
        {
            KillTween();

            if (duration <= 0f)
            {
                SetAlpha(targetAlpha);
                yield break;
            }

            gameObject.SetActive(true);
            fadeTween = DOTween.To(GetAlpha, SetAlpha, Mathf.Clamp01(targetAlpha), duration)
                .SetEase(fadeEase)
                .SetTarget(this);

            yield return fadeTween.WaitForCompletion();
        }

        public void SetAlpha(float alpha)
        {
            Image target = ThisImage;
            if (target == null)
                return;

            Color color = fadeColor;
            color.a = Mathf.Clamp01(alpha);
            target.color = color;
            target.raycastTarget = blockRaycastWhenVisible && color.a > VisibleThreshold;
            gameObject.SetActive(color.a > VisibleThreshold);
        }

        #endregion

        #region Private Methods

        float GetAlpha()
        {
            return ThisImage != null ? ThisImage.color.a : 0f;
        }

        void KillTween()
        {
            if (fadeTween != null)
            {
                fadeTween.Kill();
                fadeTween = null;
            }
        }

        void ActivateForFade()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
        }

        #endregion
    }
}
