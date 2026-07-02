using BreakInfinity;
using ExampleProject.AI;
using ExampleProject.Gameplay.Buffs;
using ExampleProject.Gameplay.Characters;
using ExampleProject.Gameplay.Faction;
using ExampleProject.Gameplay.Scenes;
using ExampleProject.Gameplay.Weapon;
using ExampleProject.Interface;
using ExampleProject.UI.FloatingCanvas;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace ExampleProject.Gameplay.Unit
{
    public class BaseUnit : MonoBehaviour
    {
        #region Fields
        [SerializeField] protected UnitData unitData;
        [SerializeField] protected AnimationClip attackAnim;
        [SerializeField] protected AnimationClip deathAnim;
        [SerializeField] protected AnimationClip runAnim;
        [SerializeField] protected AnimationClip idleAnim;
        [SerializeField] float ratioHealth = 1;
        [SerializeField] float ratioDamage = 1;
        protected IDamageable target;
        [SerializeField] protected CharacterAnimator animator;
        IDamageable damageable;
        IBasicAttack basicAttack;
        ITargetMovable targetMovable;
        LookAtTarget lookAtTarget;
        SimpleUnitBrain simpleUnitBrain;
        Collider col;
        BuffManager buffManager;
        #endregion
        #region Properties
        public FactionId Faction { get; private set; }
        public IDamageable Damageable
        {
            get
            {
                if (damageable == null)
                    damageable = GetComponent<IDamageable>();
                return damageable;
            }
        }
        protected IBasicAttack BasicAttack
        {
            get
            {
                if (basicAttack == null)
                    basicAttack = GetComponent<IBasicAttack>();
                return basicAttack;
            }
        }
        protected ITargetMovable TargetMovable
        {
            get
            {
                if (targetMovable == null)
                    targetMovable = GetComponent<ITargetMovable>();
                return targetMovable;
            }
        }
        protected LookAtTarget LookAtTarget
        {
            get
            {
                if (lookAtTarget == null)
                    lookAtTarget = GetComponent<LookAtTarget>();
                return lookAtTarget;
            }
        }
        protected SimpleUnitBrain SimpleUnitBrain
        {
            get
            {
                if (simpleUnitBrain == null)
                    simpleUnitBrain = GetComponent<SimpleUnitBrain>();
                return simpleUnitBrain;
            }
        }
        protected CharacterAnimator UnitAnimator
        {
            get
            {
                if (animator == null)
                    animator = GetComponentInChildren<CharacterAnimator>(true);
                return animator;
            }
        }
        protected Collider Col
        {
            get
            {
                if (col == null)
                    col = GetComponent<Collider>();
                return col;
            }
        }
        protected BuffManager BuffManager
        {
            get
            {
                if (buffManager == null)
                {
                    buffManager = GetComponent<BuffManager>();
                    if (buffManager == null)
                        buffManager = gameObject.AddComponent<BuffManager>();
                }
                return buffManager;
            }
        }
        public event Action<BaseUnit> OnDie
        {
            add
            {
                if (Damageable != null)
                    Damageable.OnDie += () => value(this);
            }
            remove
            {
                if (Damageable != null)
                    Damageable.OnDie -= () => value(this);
            }
        }
        public bool IsHasTarget => target != null;
        public bool IsDead => Damageable != null && Damageable.IsDead;
        public float AttackRange { get; private set; }
        public BigDouble Damage => BasicAttack != null ? BasicAttack.DamageInfor.damage : 0;
        public BigDouble MaxHealth => Damageable != null ? Damageable.MaxHealth : 0;
        public UnitId UnitId => unitData != null ? unitData.unitId : UnitId.None;
        BigDouble BaseHealth => (BigDouble)unitData.health * ratioHealth;
        BigDouble BaseDamage => (BigDouble)unitData.damage * ratioDamage;
        float BaseScale => unitData.scale;
        #endregion
        #region LifeCycle   
        protected virtual void OnEnable()
        {
            Damageable.OnDie += OnDieListener;
            BasicAttack.OnStartAttack += OnStartAttackListner;
            TargetMovable.OnStartMove += OnStartMoveListener;
            TargetMovable.OnUpdateMove += OnMoveUpdateListener;
            TargetMovable.OnStopMove += OnStopMoveListener;
        }
        protected virtual void OnDisable()
        {
            Damageable.OnDie -= OnDieListener;
            BasicAttack.OnStartAttack -= OnStartAttackListner;
            TargetMovable.OnStartMove -= OnStartMoveListener;
            TargetMovable.OnUpdateMove -= OnMoveUpdateListener;
            TargetMovable.OnStopMove -= OnStopMoveListener;
        }
        protected virtual void OnDrawGizmos()
        {
            //Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(transform.position, unitData.attackRange);
        }
        protected virtual void Update()
        {
            if (target == null || IsDead)
                return;

            TryAttack();
        }
        #endregion
        #region Private Methods
        protected virtual void OnDieListener()
        {
            if (TargetMovable.IsMoving)
                TargetMovable.StopMove();
            SetAnim(deathAnim.name, 1);
            ClearTarget();
            Col.enabled = false;
        }
        protected virtual void OnStartAttackListner()
        {
            var _speed = animator.GetAnimationLength(attackAnim.name) / BasicAttack.Interval;
            SetAnim(attackAnim.name, _speed);
        }
        void OnStartMoveListener()
        {
            SetAnim(runAnim.name, 1);
        }
        void OnMoveUpdateListener()
        {
            //if (target == null)
            //    return;
            //if (IsInAttackRange())
            //    TargetMovable.StopMove();
        }
        void OnStopMoveListener()
        {
            SetAnim(idleAnim.name, 1);
        }
        bool IsInAttackRange()
        {
            if (target == null)
                return false;
            float _distance = Vector3.Distance(Damageable.GroundTransform.position, target.GroundTransform.position);
            // Add DamageableDistance to allow attacking when the target is too big and its center is outside of the attack range but its edge is still within the range
            return _distance <= AttackRange + target.DamageableDistance;
        }
        IEnumerable<AnimationClip> GetAllAnimationClips()
        {
            return animator.GetAnimationClip();
        }
        protected void SetAnim(string _animName, float _speed)
        {
            if (UnitAnimator == null)
                return;

            UnitAnimator.SetSpeed(_speed);
            UnitAnimator.PlaySafe(_animName);
        }
        #endregion
        #region Public Methods
        public void ApplyMultiplier(float _ratioHealth, float _ratioDamage)
        {
            ratioHealth = _ratioHealth;
            ratioDamage = _ratioDamage;
        }
        public void SetFaction(FactionId _id)
        {
            Faction = _id;
        }
        public void SetPosition(Vector3 _position)
        {
            transform.position = _position;
        }
        public void SetScale(float _newScale)
        {
            transform.localScale = _newScale * BaseScale * Vector3.one;
        }
        public virtual void SetData(object _data)
        {
        }
        public void SetAnimatorOverrideController()
        {
            if (unitData == null)
            {
                Debug.LogError($"{name}: SetAnimatorOverrideController failed because unitData is null.");
                return;
            }

            if (UnitAnimator == null)
            {
                Debug.LogError($"{name}: SetAnimatorOverrideController failed because CharacterAnimator is missing.");
                return;
            }

            if (unitData.animatorOverride == null)
            {
                Debug.LogError($"{name}: SetAnimatorOverrideController failed because animatorOverride is missing. unitId={unitData.unitId}");
                return;
            }

            UnitAnimator.SetAnimatorOverrideController(unitData.animatorOverride);
        }
        public void PlayIdleAnim()
        {
            if (idleAnim == null)
            {
                Debug.LogError($"{name}: PlayIdleAnim failed because idleAnim is missing.");
                return;
            }

            if (UnitAnimator == null)
            {
                Debug.LogError($"{name}: PlayIdleAnim failed because CharacterAnimator is missing.");
                return;
            }

            UnitAnimator.SetSpeed(1);
            UnitAnimator.PlayAnimationAtTime(idleAnim.name, 0f);
        }

        public void SetAnimatorEnabled(bool isEnabled)
        {
            CharacterAnimator characterAnimator = UnitAnimator;
            if (characterAnimator == null)
                return;

            Animator unityAnimator = characterAnimator.GetComponent<Animator>();
            if (unityAnimator != null)
                unityAnimator.enabled = isEnabled;
        }

        public virtual void Init()
        {
            // Initialize Stats
            Damageable.SetHealth(BaseHealth, Faction);
            DamageInfor _damageInfor = new DamageInfor(BaseDamage, Faction, this.gameObject);            BasicAttack.SetDamage(_damageInfor);
            BasicAttack.SetAttackSpeed(unitData.attackSpeed);
            TargetMovable.MoveSpeed = unitData.moveSpeed;
            AttackRange = unitData.attackRange;
            SetScale(BaseScale);
            // Set Animator Override
            SetAnimatorOverrideController();
        }
        public virtual void SetTarget(IDamageable _target)
        {
            if (Damageable.IsDead)
                return;

            if (IsDamageableInvalid(_target))
            {
                ClearTarget();
                return;
            }

            if (!IsDamageableInvalid(target))
                target.OnDie -= ClearTarget;

            // Clear previous target event
            target = _target;
            if (SimpleUnitBrain != null)
                SimpleUnitBrain.enabled = false;
            BasicAttack.SetTarget(target);
            LookAtTarget.SetTarget(_target.GroundTransform);
            // Subscribe OnDie Event
            target.OnDie += ClearTarget;
        }
        public virtual void ClearTarget()
        {
            if (this == null)
                return;

            IDamageable previousTarget = target;
            target = null;
            if (!IsDamageableInvalid(previousTarget))
                previousTarget.OnDie -= ClearTarget;
            if (BasicAttack != null)
                BasicAttack.SetTarget(null);
//            Debug.Log("Clear Target");
            if (LookAtTarget != null)
                LookAtTarget.SetTarget(null);

            if (!IsDead &&
                GameplayController.Instance != null &&
                GameplayController.Instance.GameplayState == GameplayState.Fight &&
                SimpleUnitBrain != null)
            {
                SimpleUnitBrain.enabled = true;
            }
        }
        public virtual void TryAttack()
        {
            if (IsDamageableInvalid(target))
            {
                ClearTarget();
                return;
            }
            if (IsDead)
                return;
            if (IsInAttackRange())
            {
                if (TargetMovable.IsMoving)
                    TargetMovable.StopMove();
                BasicAttack.TryAttack();
            }
            else
            {
                TargetMovable.MoveTo(target.GroundTransform);
            }
        }

        protected bool IsDamageableInvalid(IDamageable damageableTarget)
        {
            if (damageableTarget == null)
                return true;

            UnityEngine.Object unityObject = damageableTarget as UnityEngine.Object;
            return unityObject == null || damageableTarget.IsDead;
        }
        void CalculateBuffs()
        {
            if (unitData == null)
                return;

            var _buffs = BuffManager.GetActiveBuffs();
            BigDouble modifiedHealth = BaseHealth;
            BigDouble modifiedDamage = BaseDamage;
            float modifiedCriticalChance = unitData.criticalChance;
            float modifiedCriticalMultiplier = unitData.criticalMultiplier;
            float modifiedAttackRange = unitData.attackRange;
            float modifiedMoveSpeed = unitData.moveSpeed;

            foreach (var _buff in _buffs)
            {
                switch (_buff.buffType)
                {
                    case BuffType.Damage:
                        modifiedDamage = _buff.CalculateModifier(modifiedDamage);
                        break;
                    case BuffType.Health:
                        modifiedHealth = _buff.CalculateModifier(modifiedHealth);
                        break;
                    case BuffType.CriticalChance:
                        modifiedCriticalChance = _buff.CalculateModifier(modifiedCriticalChance);
                        break;
                    case BuffType.CriticalDamage:
                        modifiedCriticalMultiplier = _buff.CalculateModifier(modifiedCriticalMultiplier);
                        break;
                    case BuffType.AttackRange:
                        modifiedAttackRange = _buff.CalculateModifier(modifiedAttackRange);
                        break;
                    case BuffType.MoveSpeed:
                        modifiedMoveSpeed = _buff.CalculateModifier(modifiedMoveSpeed);
                        break;
                }
            }

            if (Damageable != null)
                Damageable.SetHealth(modifiedHealth, Faction);

            if (BasicAttack != null)
            {
                BasicAttack.SetDamage(new DamageInfor(modifiedDamage, Faction, this.gameObject));
                BasicAttack.DamageInfor.criticalChance = modifiedCriticalChance;
                BasicAttack.DamageInfor.criticalDamage = modifiedCriticalMultiplier;
            }

            AttackRange = modifiedAttackRange;

            if (TargetMovable != null)
                TargetMovable.MoveSpeed = modifiedMoveSpeed;
        }
        public virtual void AddBuff(Buff _buff)
        {
            if (_buff == null)
                return;

            BuffManager.AddBuff(_buff);
            CalculateBuffs();
        }
        public virtual void AddBuffs(List<Buff> _buffs)
        {
            if (_buffs == null || _buffs.Count == 0)
                return;

            BuffManager.AddBuffs(_buffs);
            CalculateBuffs();
        }
        public virtual void RemoveBuff(Buff _buff)
        {
            if (_buff == null)
                return;

            BuffManager.RemoveBuff(_buff);
            CalculateBuffs();
        }
        public virtual void RemoveBuffs(List<Buff> _buffs)
        {
            if (_buffs == null || _buffs.Count == 0)
                return;

            BuffManager.RemoveBuffs(_buffs);
            CalculateBuffs();
        }
        public virtual void ClearAllBuffs()
        {
            BuffManager.ClearAllBuffs();
            CalculateBuffs();
        }
        public virtual List<Buff> GetActiveBuffs()
        {
            return BuffManager.GetActiveBuffs();
        }
        #endregion
    }
}
