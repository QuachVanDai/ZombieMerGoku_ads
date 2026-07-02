using BreakInfinity;
using UnityEngine;

namespace ExampleProject.Interface
{
    public class RangedBasicAttack : BaseBasicAttack
    {
        #region Fields

        ITargetProjectileLauncher launcher;

        #endregion

        #region Properties

        ITargetProjectileLauncher Laucher
        {
            get
            {
                if (launcher == null)
                    launcher = GetComponent<ITargetProjectileLauncher>();
                return launcher;
            }
        }

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods

        public override void SetDamage(DamageInfor _damageInfor)
        {
            base.SetDamage(_damageInfor);
            Laucher.Init(_damageInfor);
        }
        public override void SetTarget(IDamageable _target)
        {
            base.SetTarget(_target);
            Laucher.SetTarget(_target);
        }

        #endregion

        #region Public Methods

        public override void OnAttackPointReachedListener()
        {
            base.OnAttackPointReachedListener();
            Laucher.Attack();
        }

        #endregion
    }
}
