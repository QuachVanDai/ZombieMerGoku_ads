using BreakInfinity;
using ExampleProject.Gameplay.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ExampleProject.Interface
{
    public class BaseBasicAttack : MonoBehaviour, IBasicAttack
    {
        #region Fields
        [SerializeField] CharacterAnimator animator;
        public event Action OnStartAttack;
        #endregion
        #region Properties
        public DamageInfor DamageInfor { get; private set; }
        public float AttackSpeed { get; private set; }
        public float Interval => 1f / AttackSpeed;
        public float LastAttackTime { get; private set; }
        public IDamageable Target { get; private set; }
        #endregion
        #region LifeCycle
        protected virtual void OnEnable()
        {
            // Initialize last attack time to allow immediate attack on enable
            LastAttackTime = Time.time - Interval;
            if (animator != null)
                animator.onAttackPointReached += OnAttackPointReachedListener;
        }
        protected virtual void OnDisable()
        {
            if (animator != null)
                animator.onAttackPointReached -= OnAttackPointReachedListener;
        }
        #endregion
        #region Public Methods
        public virtual void SetDamage(DamageInfor _damageInfor)
        {
            DamageInfor = _damageInfor;
        }
        public virtual void SetAttackSpeed(float _attackSpeed)
        {
            AttackSpeed = _attackSpeed;
        }
        public virtual void SetTarget(IDamageable _target)
        {
            Target = _target;
        }
        public virtual void TryAttack()
        {
            if (Time.time < LastAttackTime + Interval)
                return;
            LastAttackTime = Time.time;
            OnStartAttack?.Invoke();
        }
        // Put this to animation clip event
        public virtual void OnAttackPointReachedListener()
        {
        }
        #endregion
    }
}