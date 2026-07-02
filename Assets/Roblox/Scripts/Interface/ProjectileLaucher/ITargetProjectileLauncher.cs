using BreakInfinity;
using ExampleProject.Gameplay.Projectile;
using UnityEngine;

namespace ExampleProject.Interface
{
    public interface ITargetProjectileLauncher
    {
        Transform FirePos { get; }
        TargetProjectile ProjectilePrefab { get; }
        IDamageable Target { get; }
        DamageInfor DamageInfor { get; }
        void SetTarget(IDamageable _target);
        void Init(DamageInfor _damageInfor);
        void Attack();
    }
}