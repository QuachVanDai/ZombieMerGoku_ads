using UnityEngine;

namespace ExampleProject.Interface
{
    public class LookAtTarget : MonoBehaviour
    {
        #region Fields

        [SerializeField] float rotationSpeed = 5f;
        [SerializeField] Transform target;

        #endregion

        #region Properties



        #endregion

        #region LifeCycle   

        private void Awake()
        {
            enabled = target != null;
        }

        private void Update()
        {
            if (target == null)
            {
                enabled = false;
                return;
            }

            Vector3 _direction = target.position - transform.position;
            Quaternion _targetRotation = Quaternion.LookRotation(_direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * rotationSpeed);
        }

        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public void SetTarget(Transform _newTarget)
        {
            target = _newTarget;
            enabled = target != null;
        }

        #endregion
    }
}
