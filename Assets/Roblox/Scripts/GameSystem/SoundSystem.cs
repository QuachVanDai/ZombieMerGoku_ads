using System;
using System.Collections;
using System.Collections.Generic;
using ExampleProject.Manager;
using ExampleProject.Tools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ExampleProject.GameSystem
{
    public class SoundSystem : Singleton<SoundSystem>
    {
        #region Fields

        [SerializeField] AudioSource shareAudioSource;
        [SerializeField] AudioSource uIAudioSource;

        [SerializeField] AudioClip uIOnClickAudioClip;
        [SerializeField] AudioClip errorAudioClip;
        [SerializeField] AudioClip unlockAudioClip;
        [SerializeField] AudioClip coinClaimAudioClip;

        #endregion

        #region Properties



        #endregion

        #region LifeCycle   

        private void OnEnable()
        {
            EventDispatcher.Instance.AddListener(EventName.OnPauseGame, OnPauseGameListener);
        }
        private void OnDisable()
        {
            EventDispatcher.Instance.RemoveListener(EventName.OnPauseGame, OnPauseGameListener);
        }


        #endregion

        #region Private Methods

        void OnPauseGameListener(EventName key, object data)
        {
            if (data is bool isPaused)
            {
                if (isPaused)
                    shareAudioSource.Pause();
                else
                    shareAudioSource.UnPause();
            }
        }

        #endregion

        #region Public Methods

        public void PlaySoundOneShot(AudioSource _audioSource, AudioClip _audioClip)
        {
            if (_audioSource == null || _audioClip == null)
                return;

            _audioSource.PlayOneShot(_audioClip, UserSetting.SoundVolume);
        }
        public void PlaySoundOneShot(AudioClip _audioClip)
        {
            if (shareAudioSource == null || _audioClip == null)
                return;

            shareAudioSource.PlayOneShot(_audioClip, UserSetting.SoundVolume);
        }
        public void PlaySoundOneShotRandom(List<AudioClip> _audioClips)
        {
            if (shareAudioSource == null || _audioClips == null || _audioClips.Count == 0)
                return;

            AudioClip _randomSound = null;
            for (int i = 0; i < _audioClips.Count; i++)
            {
                AudioClip _audioClip = _audioClips[Random.Range(0, _audioClips.Count)];
                if (_audioClip == null)
                    continue;

                _randomSound = _audioClip;
                break;
            }

            if (_randomSound != null)
                shareAudioSource.PlayOneShot(_randomSound, UserSetting.SoundVolume);
        }
        public void PlayUIClick()
        {
            uIAudioSource.PlayOneShot(uIOnClickAudioClip, UserSetting.SoundVolume);
        }
        public void PlayError()
        {
            uIAudioSource.PlayOneShot(errorAudioClip, UserSetting.SoundVolume);
        }
        public void PlayUnlock()
        {
            uIAudioSource.PlayOneShot(unlockAudioClip, UserSetting.SoundVolume);
        }
        public void PlayCoinClaim()
        {
            uIAudioSource.PlayOneShot(coinClaimAudioClip, UserSetting.SoundVolume);
        }
        public void SetSound(float _value)
        {
            UserSetting.SoundVolume = _value;
        }
        public LoopingSoundHandle PlayLoopingSound(AudioClip _clip)
        {
            if (_clip == null)
                return null;

            GameObject _go = new GameObject("LoopingSound_" + _clip.name);            _go.transform.SetParent(this.transform);
            AudioSource _source = _go.AddComponent<AudioSource>();
            _source.clip = _clip;
            _source.loop = true;
            _source.volume = UserSetting.SoundVolume;
            _source.Play();
            return new LoopingSoundHandle(_source);
        }

        #endregion
    }
    [Serializable]
    public class LoopingSoundHandle
    {
        [SerializeField] public AudioSource audioSource;

        public LoopingSoundHandle(AudioSource _source)
        {
            audioSource = _source;
            EventDispatcher.Instance.AddListener(EventName.OnPauseGame, OnPauseGameListener);
        }

        void OnPauseGameListener(EventName key, object data)
        {
            if (data is bool isPaused)
            {
                if (isPaused)
                    audioSource.Pause();
                else
                    audioSource.UnPause();
            }
        }

        public void Kill()
        {
            EventDispatcher.Instance.RemoveListener(EventName.OnPauseGame, OnPauseGameListener);
            if (audioSource != null)
            {
                audioSource.Stop();
                GameObject.Destroy(audioSource.gameObject);
                audioSource = null;
            }
        }
    }
}
