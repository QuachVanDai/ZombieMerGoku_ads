using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using ExampleProject.Gameplay.Faction;

namespace ExampleProject.UI.Shared
{
    public class HealthBar : MonoBehaviour
    {
        #region Fields

        [SerializeField] FactionId faction;

     //   [SerializeField] Slider slider;
        [SerializeField] Image fillImage;
        [SerializeField] Image ghostImage;
        [SerializeField] Color fillColor = Color.green;
        [SerializeField] Color ghostColor = new Color(0.35f, 0.35f, 0.35f, 1f);
        [SerializeField] float animationDuration = 0.3f;
        [SerializeField] Ease animationEase = Ease.OutCubic;
        [SerializeField] float ghostDelay = 0.2f;
        [SerializeField] bool hideGhostImage = true;

        float maxHealth = 100f;
        float currentHealth = 100f;
        Sequence healthTween;

        #endregion

        #region Properties

        public float MaxHealth => maxHealth;

        public float CurrentHealth => currentHealth;

        #endregion

        #region LifeCycle           

        void Awake()
        {
            CacheReferences();
            ApplyColors();
            SetRatio(1f, 1f);
        }

        private void OnDestroy()
        {
            healthTween?.Kill();
        }

        #endregion

        #region Private Methods

        void CacheReferences()
        {
            
            ApplyGhostVisibility();
        }

        private void UpdateHealthColor()
        {
            ApplyColors();
        }

        void ApplyColors()
        {
            if (fillImage != null)
                fillImage.color = fillColor;

            if (ghostImage != null)
                ghostImage.color = ghostColor;
        }

        void ApplyGhostVisibility()
        {
            if (ghostImage != null)
                ghostImage.gameObject.SetActive(!hideGhostImage);
        }

        float GetHealthRatio()
        {
            if (maxHealth <= 0f)
                return 0f;

            return Mathf.Clamp01(currentHealth / maxHealth);
        }

        float GetImageRatio(Image image)
        {
            if (image == null)
                return 0f;

            if (image.type == Image.Type.Filled)
                return Mathf.Clamp01(image.fillAmount);

            return Mathf.Clamp01(image.rectTransform.anchorMax.x);
        }

        void SetImageRatio(Image image, float ratio)
        {
            if (image == null)
                return;

            ratio = Mathf.Clamp01(ratio);

            EnsureImageVisible(image);

            if (image.type == Image.Type.Filled)
            {
                image.fillAmount = ratio;
            }

            RectTransform rect = image.rectTransform;
            Vector2 anchorMax = rect.anchorMax;
            anchorMax.x = ratio;
            rect.anchorMax = anchorMax;
        }

        void EnsureImageVisible(Image image)
        {
            if (image == null)
                return;

            Transform current = image.transform;
            while (current != null && current != transform)
            {
                if (!current.gameObject.activeSelf)
                    current.gameObject.SetActive(true);

                current = current.parent;
            }

            if (!image.gameObject.activeSelf)
                image.gameObject.SetActive(true);
        }

        void SetRatio(float fillRatio, float ghostRatio)
        {
            CacheReferences();


            SetImageRatio(fillImage, fillRatio);
            if (!hideGhostImage)
                SetImageRatio(ghostImage, ghostRatio);
        }

        void SetHealth(float _healthPercent, bool _animate = true)
        {
            CacheReferences();
            _healthPercent = Mathf.Clamp(_healthPercent, 0f, maxHealth);
            currentHealth = _healthPercent;

            healthTween?.Kill();
            ApplyColors();

            float targetRatio = GetHealthRatio();

            if (_animate && animationDuration > 0f)
            {
                float fillRatio = GetImageRatio(fillImage);
                healthTween = DOTween.Sequence();
                healthTween.Append(
                    DOTween.To(() => fillRatio, _value =>
                    {
                        fillRatio = _value;
                      

                        SetImageRatio(fillImage, _value);
                    }, targetRatio, animationDuration)
                    .SetEase(animationEase)
                    );
                if (!hideGhostImage && ghostImage != null)
                {
                    float ghostRatio = GetImageRatio(ghostImage);
                    healthTween.Append(
                        DOTween.To(() => ghostRatio, _value =>
                        {
                            ghostRatio = _value;
                            SetImageRatio(ghostImage, _value);
                        }, targetRatio, animationDuration)
                        .SetEase(animationEase).SetDelay(ghostDelay)
                        );
                }

                healthTween.Play();
            }
            else
            {
                SetRatio(targetRatio, targetRatio);
            }
        }

        #endregion

        #region Public Methods

        public void SetActive(bool _active)
        {
            gameObject.SetActive(_active);
        }
        public void SetHealthPercent(float _percent, bool _animate = true)
        {
            SetHealth(_percent, _animate);
        }
        public void Initialize(FactionId _faction)
        {
            CacheReferences();
            gameObject.SetActive(true);
            faction = _faction;

            // Default to 100 percent max health
            maxHealth = 100;
       

            SetHealth(100, false);
        }

        #endregion
    }
}
