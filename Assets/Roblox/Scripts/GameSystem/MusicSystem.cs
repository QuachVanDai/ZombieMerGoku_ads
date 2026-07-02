using System.Collections;
using System.Collections.Generic;
using ExampleProject.Manager;
using ExampleProject.Tools;
using UnityEngine;


namespace ExampleProject.GameSystem
{
    public class MusicSystem : Singleton<MusicSystem>
    {
        #region Fields

        [SerializeField] private AudioSource musicAudioSource;

        #endregion

        #region Properties



        #endregion

        #region LifeCycle   

        private void OnEnable()
        {
            musicAudioSource.volume = UserSetting.MusicVolume;
            
        }

        #endregion

        #region Private Methods



        #endregion

        #region Protected Methods



        #endregion

        #region Public Methods

        public void PlayMusic(AudioClip _audioClip)
        {

            musicAudioSource.clip = _audioClip;
            musicAudioSource.Play();
        }
        public void SetMusic(float _value)
        {
            UserSetting.MusicVolume = _value;
            musicAudioSource.volume = UserSetting.MusicVolume;
        }

        #endregion
    }
}