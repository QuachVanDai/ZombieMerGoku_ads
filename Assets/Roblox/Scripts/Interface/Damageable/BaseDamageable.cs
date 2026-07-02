using BreakInfinity;
using ExampleProject.Gameplay.Faction;
using ExampleProject.Interface;
using ExampleProject.Tools;
using ExampleProject.UI.Shared;
using System;
using System.Collections;
using UnityEngine;

namespace ExampleProject.Interface
{
    public class BaseDamageable : MonoBehaviour, IDamageable
    {
        #region Fields

        [SerializeField] HealthBar healthBar;
        [SerializeField] bool showHealthBar = false;
        [SerializeField] FloatingTextSpawner floatingTextSpawner;
        [SerializeField] Transform hitPosTransform;
        [SerializeField] float damageableDistance;
        [SerializeField, Range(0f, 1f)] float floatingDamageTextChance = 0.25f;
        [SerializeField] float floatingDamageTextCooldown = 0.12f;
        [SerializeField] bool alwaysShowCriticalDamageText = true;
        [SerializeField] bool alwaysShowKillingDamageText = true;

        float nextFloatingDamageTextTime;

        public event Action<DamageInfor> OnTakeDamage;
        public event Action OnDie;

        #endregion

        #region Properties

        public Transform GroundTransform => transform;
        public float DamageableDistance => damageableDistance;
        public Transform HitPosTransform => hitPosTransform == null ? GroundTransform : hitPosTransform;
        public FactionId Faction { get; private set; }
        public bool IsDead => CurrentHealth <= 0;
        public bool IsInvulnerable { get; private set; }
        public BigDouble CurrentHealth { get; private set; }
        public BigDouble MaxHealth { get; private set; }
        public DamageInfor CauseOfDeath { get; private set; }
        protected virtual bool ShouldShowHealthBar => showHealthBar;

        #endregion

        #region LifeCycle   


        #endregion

        #region Private Methods

        protected virtual BigDouble CalculateFinalDamage(DamageInfor _damageInfor)
        {
            BigDouble _finalDamage = _damageInfor.damage;
            _damageInfor.CalculateCritical();

            // Calculate critical hit
            if (_damageInfor.IsCritical)
            {
                _finalDamage *= _damageInfor.criticalDamage;
            }

            // Override in derived classes to add resistance/armor calculations
            // Example:
            // if (_damageInfor.damageType == DamageType.Fire)
            //     finalDamage = (int)(finalDamage * fireResistance);

            return _finalDamage;
        }

        protected virtual void ApplyDamageEffects(DamageInfor _damageInfor)
        {
            // Override in derived classes to add elemental effects
            // Example:
            // if (_damageInfor.damageType == DamageType.Fire)
            //     ApplyBurnEffect();
            // if (_damageInfor.damageType == DamageType.Ice)
            //     ApplySlowEffect();
        }

        bool ShouldSpawnFloatingDamageText(DamageInfor damageInfor)
        {
            if (floatingTextSpawner == null)
                return false;

            if (alwaysShowCriticalDamageText && damageInfor.IsCritical)
                return true;

            if (alwaysShowKillingDamageText && CurrentHealth <= 0)
                return true;

            if (Time.time < nextFloatingDamageTextTime)
                return false;

            return UnityEngine.Random.value <= floatingDamageTextChance;
        }

        void SpawnFloatingDamageText(DamageInfor damageInfor, BigDouble finalDamage)
        {
            if (!ShouldSpawnFloatingDamageText(damageInfor))
                return;

            nextFloatingDamageTextTime = Time.time + floatingDamageTextCooldown;

            if (damageInfor.IsCritical)
                floatingTextSpawner.SpawnFloatingCriticalText(BigDouble.FormatShorthand(finalDamage));
            else
                floatingTextSpawner.SpawnFloatingText(BigDouble.FormatShorthand(finalDamage));
        }

        #endregion

        #region Public Methods

        public void SetHealth(BigDouble _maxHealth, FactionId _faction)
        {
            Faction = _faction;
            MaxHealth = _maxHealth;
            CurrentHealth = MaxHealth;
            CauseOfDeath = null;
            if (healthBar != null && ShouldShowHealthBar)
                healthBar.Initialize(Faction);
            else if (healthBar != null)
                healthBar.SetActive(false);
        }

        public virtual void TakeDamage(DamageInfor _damageInfor)
        {
            if (IsDead)
                return;
            if (IsInvulnerable)
                return;
            if (Faction == _damageInfor.faction)
                return;

            // Calculate final damage (with resistances)
            BigDouble _finalDamage = CalculateFinalDamage(_damageInfor);

            // Apply damage
            CurrentHealth -= _finalDamage;

            // Update health bar
            var _healthPercent = (float)(CurrentHealth / MaxHealth * 100).ToDouble();
            if (healthBar != null && ShouldShowHealthBar)
                healthBar.SetHealthPercent(_healthPercent);

            // Trigger after health is updated so listeners can read CurrentHealth reliably.
            OnTakeDamage?.Invoke(_damageInfor);

            SpawnFloatingDamageText(_damageInfor, _finalDamage);

            // Apply elemental effects
            ApplyDamageEffects(_damageInfor);

            // Check for death
            if (CurrentHealth <= 0)
            {
                CauseOfDeath = _damageInfor;
                if (healthBar != null)
                    healthBar.SetActive(false);
                CurrentHealth = 0;
                OnDie?.Invoke();
            }
        }

        public void StartInvulnerability(float duration)
        {
            IsInvulnerable = true;

            StartCoroutine(_IEDelayStop());
            IEnumerator _IEDelayStop()
            {
                yield return new WaitForSeconds(duration);
                IsInvulnerable = false;
            }
        }

        #endregion
    }
}
