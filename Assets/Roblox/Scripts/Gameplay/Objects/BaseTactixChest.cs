using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ExampleProject.GameSystem;
using ExampleProject.Gameplay.Rarity;
using ExampleProject.UI.Shared;
using UnityEngine;

namespace ExampleProject
{
    public class BaseTactixChest : MonoBehaviour
    {
        #region Fields
        [SerializeField] ChestId chestId;
       
       
     
        [SerializeField] List<MeshRenderer> chestMeshRenderers;
        [SerializeField] List<MeshFilter> chestMeshFilters;
        [SerializeField] Transform chestLidTransform, chestOpenEffectSpawnPoint;
        [SerializeField] float durationOpenChest = 0.5f;
        [SerializeField] RarityTitle rarityTitle;
        [SerializeField] GameObject floatingCanvas;
        [SerializeField] private GameObject effectChest;
        [SerializeField] private bool spawnOpenChestVfx;
        [SerializeField] bool playShakeOnEnable = true;
        [SerializeField] GameObject meshShakeObject;
        [SerializeField] float shakeAngle = 5f;
        [SerializeField] float shakeDuration = 0.45f;
        [SerializeField] float shakeDelay = 0.5f;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip openChestAudio;
        Tween shakeTween;
        Vector3 shakeStartLocalPosition;
        Quaternion shakeStartLocalRotation;
        bool hasShakeStartTransform;
        #endregion

        #region LifeCycle
        void OnEnable()
        {
            StartShake();
        }

        void OnDisable()
        {
            StopShake();
        }
        private void Start()
        {
          //  InteractionPrompt ="Buy chest";
        }

        private void OnListenChangeState(EventName _name = EventName.NONE, object _obj = null)
        {
            
        }


        #endregion

        #region Private Methods

        /*
        private void SetSkin(TactixChestData _chestData)
        {
            if (_chestData == null)
                return;

            int count = Mathf.Min(chestMeshRenderers.Count, chestMeshFilters.Count);
            for (int i = 0; i < count; i++)
            {
                if (chestMeshRenderers[i] != null && _chestData.material != null)
                    chestMeshRenderers[i].sharedMaterial = _chestData.material;

                if (chestMeshFilters[i] != null && _chestData.mesh != null && i < _chestData.mesh.Count && _chestData.mesh[i] != null)
                    chestMeshFilters[i].mesh = _chestData.mesh[i];
            }

            if (chestLidTransform != null)
                chestLidTransform.localPosition = _chestData.chestLidPos;
        }
        */

        void StartShake()
        {
            if (!playShakeOnEnable || meshShakeObject == null || shakeAngle <= 0f || shakeDuration <= 0f)
                return;

            Transform target = meshShakeObject.transform;
            StopShake();

            float angle = 0f;
            shakeStartLocalPosition = target.localPosition;
            shakeStartLocalRotation = target.localRotation;
            hasShakeStartTransform = true;
            shakeTween = DOTween.Sequence()
                .Append(DOTween.To(() => angle, value =>
                {
                    angle = value;
                    if (target != null)
                        target.localRotation = shakeStartLocalRotation * Quaternion.Euler(0f, 0f, value);
                }, shakeAngle, shakeDuration * 0.5f))
                .Append(DOTween.To(() => angle, value =>
                {
                    angle = value;
                    if (target != null)
                        target.localRotation = shakeStartLocalRotation * Quaternion.Euler(0f, 0f, value);
                }, -shakeAngle, shakeDuration))
                .Append(DOTween.To(() => angle, value =>
                {
                    angle = value;
                    if (target != null)
                        target.localRotation = shakeStartLocalRotation * Quaternion.Euler(0f, 0f, value);
                }, 0f, shakeDuration * 0.5f))
                .AppendInterval(Mathf.Max(0f, shakeDelay))
                .SetLoops(-1)
                .SetEase(Ease.InOutSine)
                .SetTarget(target);
        }

        void StopShake()
        {
            if (shakeTween != null)
            {
                shakeTween.Kill();
                shakeTween = null;
            }

            Transform target = meshShakeObject != null ? meshShakeObject.transform : null;
            if (hasShakeStartTransform && target != null)
            {
                target.localPosition = shakeStartLocalPosition;
                target.localRotation = shakeStartLocalRotation;
            }
        }
       
        #endregion

        #region Public Methods


   
        public void OpenChestLid()
        {
            StopShake();
            PlayOpenChestSound();
            StartCoroutine(OpenLidCoroutine());
            IEnumerator OpenLidCoroutine()
            {
                if (chestLidTransform == null)
                    yield break;

                float _elapsedTime = 0f;
                Quaternion initialRotation = chestLidTransform.localRotation;
                Quaternion targetRotation = Quaternion.Euler(-70f, 0f, 0f);

                TactixChestData chestData = TactixChests.GetChestResourceData(chestId);
                if (spawnOpenChestVfx && chestData != null && chestData.chestVFXOpenPrefab != null && chestOpenEffectSpawnPoint != null)
                {
                    GameObject _chestOpenEffect = Instantiate(chestData.chestVFXOpenPrefab, chestOpenEffectSpawnPoint.position, Quaternion.identity);
                    _chestOpenEffect.transform.SetParent(chestOpenEffectSpawnPoint);
                }

                while (_elapsedTime < durationOpenChest)
                {
                    chestLidTransform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, _elapsedTime / durationOpenChest);
                    _elapsedTime += Time.deltaTime;
                    yield return null;
                }
                chestLidTransform.localRotation = targetRotation;
            }
        }
        public void PlayOpenAnimationOnly()
        {
            OpenChestLid();
        }
        public void HandleChestOpenState()
        {
            HideFloatingCanvas();
        }
        public void HideFloatingCanvas()
        {
            if (floatingCanvas != null)
                floatingCanvas.gameObject.SetActive(false);
            if(effectChest != null)
                effectChest.SetActive(false);
        }
        public void HideChest()
        {
            StopShake();
            HideFloatingCanvas();
            Destroy(gameObject);
        }

        public void PlayOpenChestSound()
        {
            if (openChestAudio == null || SoundSystem.Instance == null)
                return;

            if (audioSource != null)
                SoundSystem.Instance.PlaySoundOneShot(audioSource, openChestAudio);
            else
                SoundSystem.Instance.PlaySoundOneShot(openChestAudio);
        }
        /*public void SetChestId(ChestId _chestId)
        {
            chestId = _chestId;
            TactixChestData _data = TactixChests.GetChestResourceData(chestId);
            if (_data == null)
                return;

         //   Instantiate(GameplayController.Instance.GetRarityData(_data.rarity).rarityText, rarityTextParent.transform);
            if (rarityTitle != null)
                rarityTitle.SetRarity(_data.rarity);

            RarityData _rarityData = Rarities.GetResourceData(_data.rarity);
            if (rarityTitle != null && _rarityData != null && !GradientUtility.IsWhiteGradient(_rarityData.gradient2))
            {
                Gradient _gradientComponent = rarityTitle.GetComponent<Gradient>();
                if (_gradientComponent != null)
                {
                    _gradientComponent.EffectGradient = _rarityData.gradient2;
                  //  _gradientComponent.Angle = 270f;
                }
            }
            SetSkin(_data);
        }*/
        #endregion      
    }
}
