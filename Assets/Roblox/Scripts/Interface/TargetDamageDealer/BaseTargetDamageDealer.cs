using BreakInfinity;
using ExampleProject.Gameplay.Faction;
using ExampleProject.Interface;
using System;
using UnityEngine;

namespace ExampleProject.Interface
{
    public class BaseTargetDamageDealer : MonoBehaviour, ITargetDamageDealer
    {
        #region Fields

        public event Action<IDamageable, DamageInfor> OnDealDamage;

        #endregion

        #region Properties

        public DamageInfor DamageInfor { get; set; }

        #endregion

        #region LifeCycle   


        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public void Init(DamageInfor _damageInfor)
        {
            DamageInfor = _damageInfor;
        }
        public virtual void DealDamageTo(IDamageable _target)
        {
            if (IsTargetInvalid(_target))
                return;

            _target.TakeDamage(DamageInfor);
            OnDealDamage?.Invoke(_target, DamageInfor);
        }

        protected bool IsTargetInvalid(IDamageable _target)
        {
            if (_target == null)
                return true;

            UnityEngine.Object unityObject = _target as UnityEngine.Object;
            return unityObject == null || _target.IsDead;
        }

        #endregion
    }
}
