using BreakInfinity;
using ExampleProject.Gameplay.Projectile;
using UnityEngine;

namespace ExampleProject.Interface
{
    public class BaseTargetProjectileLauncher : MonoBehaviour, ITargetProjectileLauncher
    {
        #region Fields

        [SerializeField] TargetProjectile projectilePrefab;
        [SerializeField] Transform firePos;

        #endregion

        #region Properties

        public TargetProjectile ProjectilePrefab => projectilePrefab;
        public Transform FirePos => firePos != null ? firePos : transform;
        public IDamageable Target { get; set; }
        public DamageInfor DamageInfor { get; private set; }

        #endregion

        #region LifeCycle



        #endregion

        #region Private Methods


        #endregion

        #region Public Methods

        public void SetTarget(IDamageable _target)
        {
            Target = _target;
        }
        public void Init(DamageInfor _damageInfor)
        {
            DamageInfor = _damageInfor;
        }
        public void Attack()
        {
            if (projectilePrefab == null || IsTargetInvalid(Target))
                return;

            TargetProjectile _projectile = Instantiate(projectilePrefab, FirePos.position, FirePos.rotation);
            _projectile.Init(Target, DamageInfor);
        }

        bool IsTargetInvalid(IDamageable target)
        {
            if (target == null)
                return true;

            UnityEngine.Object unityObject = target as UnityEngine.Object;
            return unityObject == null || target.IsDead;
        }

        #endregion
    }
}
