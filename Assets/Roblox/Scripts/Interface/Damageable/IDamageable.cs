using BreakInfinity;
using ExampleProject.Gameplay.Faction;
using System;
using UnityEngine;

namespace ExampleProject.Interface
{
    public interface IDamageable
    {
        #region Fields

        Transform GroundTransform { get; }
        Transform HitPosTransform { get; }
        FactionId Faction { get; }
        BigDouble CurrentHealth { get; }
        BigDouble MaxHealth { get; }
        bool IsDead { get; }
        bool IsInvulnerable { get; }
        DamageInfor CauseOfDeath { get; }
        float DamageableDistance { get; }

        event Action<DamageInfor> OnTakeDamage;
        event Action OnDie;

        #endregion

        #region Public Methods

        void SetHealth(BigDouble _health, FactionId _faction);
        void TakeDamage(DamageInfor _damageInfor);
        void StartInvulnerability(float _duration);

        #endregion
    }
}