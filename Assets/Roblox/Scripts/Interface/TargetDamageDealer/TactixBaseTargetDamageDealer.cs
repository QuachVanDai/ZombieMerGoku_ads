using System.Collections;
using System.Collections.Generic;
using ExampleProject.Interface;
using UnityEngine;

namespace ExampleProject
{
    public class TactixBaseTargetDamageDealer : BaseTargetDamageDealer
    {
        #region Fields

        [SerializeField] private Transform effectHitParent;
        [SerializeField] private GameObject hitEffectPrefab;
        [SerializeField] private bool spawnHitEffect;

        #endregion

        #region Properties

       

        #endregion

        #region LifeCycle   


        #endregion

        #region Private Methods

        private void OnSpawnHitEffect()
        {
            if (!spawnHitEffect || hitEffectPrefab == null || effectHitParent == null)
                return;
            GameObject hitEffect = Instantiate(hitEffectPrefab, effectHitParent.position, Quaternion.identity);
            hitEffect.transform.SetParent(transform.parent);
            hitEffect.transform.localScale*= transform.localScale.x;
            Destroy(hitEffect, 1f);
        }

        #endregion

        #region Public Methods

        public override void DealDamageTo(IDamageable _target)
        {
            if (IsTargetInvalid(_target))
                return;

            base.DealDamageTo(_target);
            OnSpawnHitEffect();
        }

        #endregion
    }
}
