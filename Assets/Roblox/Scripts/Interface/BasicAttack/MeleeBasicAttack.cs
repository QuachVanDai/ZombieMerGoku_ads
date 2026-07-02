using BreakInfinity;
using UnityEngine;

namespace ExampleProject.Interface
{
    public class MeleeBasicAttack : BaseBasicAttack
    {
        #region Fields

        ITargetDamageDealer damageDealer;

        #endregion

        #region Properties

       ITargetDamageDealer DamageDealer
       {
           get
           {
               if (damageDealer == null)
                   damageDealer = GetComponent<ITargetDamageDealer>();
               return damageDealer;
           }
       }

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public override void SetDamage(DamageInfor _damageInfor)
        {
            base.SetDamage(_damageInfor);
            DamageDealer.Init(_damageInfor);
        }
        public override void OnAttackPointReachedListener()
        {
            base.OnAttackPointReachedListener();
            DamageDealer.DealDamageTo(Target);
        }

        #endregion
    }
}
