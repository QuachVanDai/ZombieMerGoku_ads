using BreakInfinity;
using System;
using UnityEngine;

namespace ExampleProject.Interface
{
    public interface IBasicAttack
    {
        event Action OnStartAttack;
        IDamageable Target { get; }
        DamageInfor DamageInfor { get; }
        float AttackSpeed { get; }
        float Interval { get; }
        float LastAttackTime { get; }
        void TryAttack();
        void SetDamage(DamageInfor _damageInfor);
        void SetAttackSpeed(float _attackSpeed);
        void SetTarget(IDamageable _target);
    }
}