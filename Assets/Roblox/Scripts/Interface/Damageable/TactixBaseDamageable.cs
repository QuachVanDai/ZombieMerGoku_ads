using System.Linq;
using ExampleProject.Gameplay.Faction;
using ExampleProject.Gameplay.Scenes;
using ExampleProject.Gameplay.Unit;
using ExampleProject.GameSystem;
using ExampleProject.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject
{
    public class TactixBaseDamageable : BaseDamageable
    {
        #region Fields
        [SerializeField] AudioClip unitDieAudioClip;
        [SerializeField] AudioClip zombieDieAudioClip;
        [SerializeField] private AudioClip towerDieAudioClip;
        [SerializeField] AudioSource dieAudioSource;
        [SerializeField] Text textHealth;
        [SerializeField] bool showTactixHealthBar = true;
        #endregion

        #region Properties
        FactionId DamageFaction => GetComponent<BaseUnit>() != null ? GetComponent<BaseUnit>().Faction : FactionId.Zombie;
        protected override bool ShouldShowHealthBar => showTactixHealthBar;


        #endregion

        #region LifeCycle   


        protected void OnEnable()
        {
            OnDie += PlayDieSound;
        }

        protected void OnDisable()
        {
            OnDie -= PlayDieSound;
        }
        #endregion

        #region Private Methods
        
        private void UpdateTextHealth()
        {
            if(textHealth == null)
                return;
            textHealth.text =
                System.Math.Round(CurrentHealth.ToDouble()) + "/" +
                System.Math.Round(MaxHealth.ToDouble()); 
        }
        private void PlayDieSound()
        {

            if(DamageFaction == FactionId.Zombie)
            {
                if (GetComponent<Tower>() == null)
                {
                    SoundSystem.Instance.PlaySoundOneShot(dieAudioSource, zombieDieAudioClip);
                }
                else
                {
                    SoundSystem.Instance.PlaySoundOneShot(dieAudioSource, towerDieAudioClip);
                }
                
            }
            else
            {
                SoundSystem.Instance.PlaySoundOneShot(dieAudioSource, unitDieAudioClip);
            }
            if(textHealth != null)
                textHealth.gameObject.SetActive(false);
        }

        #endregion

        #region Public Methods
        
        public override void TakeDamage(DamageInfor _damageInfor)
        {
            if(GameplayController.Instance.GameplayState != GameplayState.Fight)
                return;
            if(Faction == FactionId.Zombie)
            {
                bool _allDeadUnit = GameplayController.Instance.GruntCreepUnit.All(g => g == null || g.IsDead);
                if (_allDeadUnit)
                    return;
            }
            else if(Faction == FactionId.Grunt)
            {
                bool _allDeadZombie = GameplayController.Instance.ZombieCreepUnit.All(g => g == null || g.IsDead);
                bool _allDeadTower = GameplayController.Instance.Towers.All(g => g == null || g.IsDead);
                if(_allDeadTower && _allDeadZombie)
                    return;
            }
            base.TakeDamage(_damageInfor);
            UpdateTextHealth();
        }
        #endregion     
    }
}
