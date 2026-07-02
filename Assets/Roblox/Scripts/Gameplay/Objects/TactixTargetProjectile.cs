using System.Collections;
using System.Collections.Generic;
using ExampleProject.Gameplay.Scenes;
using UnityEngine;

namespace ExampleProject.Gameplay.Projectile
{
    public class TactixTargetProjectile : MonoBehaviour
    {
        #region Fields
        [SerializeField] GameObject impactPrefab;
        [SerializeField] bool spawnImpactVfx;
        

        #endregion

        #region Properties

        

        #endregion

        #region LifeCycle

        void OnEnable()
        {
            EventDispatcher.Instance.AddListener(EventName.OnAfterGameStateChange,OnListenChangeState);
        }

        void OnDisable()
        {
           EventDispatcher.Instance.RemoveListener(EventName.OnAfterGameStateChange,OnListenChangeState);
           if(spawnImpactVfx && impactPrefab != null&& GameplayController.Instance != null && GameplayController.Instance.GameplayState == GameplayState.Fight)
           {
               GameObject _impactVfx = Instantiate(impactPrefab, transform.position, Quaternion.identity,transform.parent);
               Destroy(_impactVfx, 1.5f);
           }
        }

        #endregion

        #region Private Methods
        

        #endregion

        #region Public Methods

        public void OnListenChangeState(EventName _name = EventName.NONE, object _obj = null)
        {
            if (GameplayController.Instance.GameplayState != GameplayState.Fight)
            {
                Destroy(gameObject);
            }
        }
        #endregion
    }
}
