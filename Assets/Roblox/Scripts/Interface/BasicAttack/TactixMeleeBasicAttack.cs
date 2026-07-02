using System.Collections;
using System.Collections.Generic;
using ExampleProject.Gameplay.Faction;
using ExampleProject.Gameplay.Scenes;
using ExampleProject.Gameplay.Unit;
using ExampleProject.GameSystem;
using ExampleProject.Interface;
using UnityEngine;

namespace ExampleProject
{
    public class TactixMeleeBasicAttack : MeleeBasicAttack
    {
        #region Fields

        [SerializeField] AudioClip unitAttackAudioClip;
        [SerializeField] AudioClip bossAttackAudioClip;
        [SerializeField] AudioSource audioSource;
        
        TactixBaseDamageable damageable;
        #endregion

        #region Properties
         public UnitId UnitId => GetComponent<BaseUnit>().UnitId;
        TactixBaseDamageable Damageable
        {
            get
            {
                if (damageable == null)
                    damageable = GetComponent<TactixBaseDamageable>();
                return damageable;
            }
        }
        

        #endregion

        #region LifeCycle   
        protected override void OnEnable()
        {
            base.OnEnable();
            OnStartAttack += PlayAttackSound;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            OnStartAttack -= PlayAttackSound;
        }
        #endregion

        #region Private Methods

        private void PlayAttackSound()
        {
            if(Damageable.Faction == FactionId.Zombie)
            {
            }
            else
            {
                SoundSystem.Instance.PlaySoundOneShot(audioSource, unitAttackAudioClip);
            }
        }

        #endregion

        #region Protected Methods

        public override void OnAttackPointReachedListener()
        {
            base.OnAttackPointReachedListener();
            if (Damageable.Faction == FactionId.Zombie)
            {
                SoundSystem.Instance.PlaySoundOneShot(audioSource, bossAttackAudioClip);
            }
        }

        #endregion

        #region Public Methods
        public void DisableEventAttackSound()
        {
            OnStartAttack -= PlayAttackSound;
            Debug.Log("Disable Attack Sound");
        }

        #endregion
    }
}
