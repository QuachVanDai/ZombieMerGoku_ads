using BreakInfinity;
using ExampleProject.Gameplay.Faction;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExampleProject.Interface
{
    public interface IBoxDamageDealer
    {
        DamageInfor DamageInfor { get; }
        Vector3 BoxSize { get; }
        Vector3 Offset { get; }
        event Action<IEnumerable<IDamageable>, DamageInfor> OnDealDamage;

        void SetDamage(DamageInfor _damageInfor);
        void SetBoxSize(Vector3 _boxSize);
        void SetOffset(Vector3 _offset);
        void DealDamageInBox();
    }
}