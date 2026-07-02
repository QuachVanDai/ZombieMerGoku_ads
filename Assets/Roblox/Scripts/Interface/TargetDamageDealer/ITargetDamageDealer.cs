using BreakInfinity;
using ExampleProject.Gameplay.Faction;
using System;

namespace ExampleProject.Interface
{
    public interface ITargetDamageDealer
    {
        DamageInfor DamageInfor { get; set; }
        event Action<IDamageable, DamageInfor> OnDealDamage;
        void Init(DamageInfor _damageInfor);
        void DealDamageTo(IDamageable _target);
    }
}