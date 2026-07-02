using BreakInfinity;
using ExampleProject.Gameplay.Faction;
using ExampleProject.Interface;
using UnityEngine;
namespace ExampleProject.Gameplay.Projectile
{
    public class TargetProjectile : MonoBehaviour
    {
        #region Fields
        ITargetDamageDealer damageDealer;
        ITargetMovable targetMovable;
        LookAtTarget lookAtTarget;
        IDamageable target;
        #endregion
        #region Properties
        ITargetDamageDealer IDamageDealer
        {
            get
            {
                if (damageDealer == null)
                    damageDealer = GetComponent<ITargetDamageDealer>();
                return damageDealer;
            }
        }
        ITargetMovable ITargetMovable
        {
            get
            {
                if (targetMovable == null)
                    targetMovable = GetComponent<ITargetMovable>();
                return targetMovable;
            }
        }
        LookAtTarget LookAtTarget
        {
            get
            {
                if (lookAtTarget == null)
                    lookAtTarget = GetComponent<LookAtTarget>();
                return lookAtTarget;
            }
        }
        #endregion
        #region LifeCycle
        void OnEnable()
        {
            ITargetMovable.OnTargetReached += OnTargetReachedListener;
        }
        void OnDisable()
        {
            ITargetMovable.OnTargetReached -= OnTargetReachedListener;
        }
        void Update()
        {
            if (IsTargetInvalid(target))
                Destroy(gameObject);
        }
        #endregion
        #region Private Methods
        void OnTargetReachedListener()
        {
            if (IsTargetInvalid(target))
            {
                Destroy(gameObject);
                return;
            }

            IDamageDealer.DealDamageTo(target);
            Destroy(this.gameObject);
        }
        bool IsTargetInvalid(IDamageable damageableTarget)
        {
            if (damageableTarget == null)
                return true;

            UnityEngine.Object unityObject = damageableTarget as UnityEngine.Object;
            return unityObject == null || damageableTarget.IsDead;
        }
        #endregion
        #region Public Methods
        public void Init(IDamageable _targetToMove, DamageInfor _damageInfor)
        {
            if (IsTargetInvalid(_targetToMove))
            {
                Destroy(gameObject);
                return;
            }

            target = _targetToMove;
            IDamageDealer.Init(_damageInfor);
            ITargetMovable.MoveTo(target.HitPosTransform);
            LookAtTarget.SetTarget(target.HitPosTransform);
        }
        #endregion
    }
}
