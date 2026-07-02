using System;
using UnityEngine;

namespace ExampleProject.Interface
{
    public class TranslateTargetMovable : MonoBehaviour, ITargetMovable
    {
        #region Fields

        [SerializeField] float moveSpeed = 5f;
        [SerializeField] float stopDistance = 0.1f;

        Transform target;
        Vector3 targetPosition;
        bool isMoving;
        bool hasTarget;

        #endregion

        #region Properties

        public Transform Target
        {
            get => target;
            set => target = value;
        }

        public bool IsMoving => isMoving;

        public float MoveSpeed
        {
            get => moveSpeed;
            set => moveSpeed = value;
        }

        #endregion

        #region Events

        public event Action OnStartMove;
        public event Action OnStopMove;
        public event Action OnUpdateMove;
        public event Action OnTargetReached;

        #endregion

        #region LifeCycle   

        private void Awake()
        {
            enabled = false;
        }

        private void Update()
        {
            if (!isMoving || !hasTarget)
            {
                enabled = false;
                return;
            }

            Vector3 _currentTarget = target != null ? target.position : targetPosition;
            Vector3 _direction = (_currentTarget - transform.position).normalized;
            float _distance = Vector3.Distance(transform.position, _currentTarget);

            if (_distance <= stopDistance)
            {
                ReachTarget();
                return;
            }

            transform.Translate(moveSpeed * Time.deltaTime * _direction, Space.World);
            OnUpdateMove?.Invoke();
        }

        #endregion

        #region Private Methods

        private void StartMoving()
        {
            if (!isMoving)
            {
                isMoving = true;
                hasTarget = true;
                enabled = true;
                OnStartMove?.Invoke();
            }
        }

        private void ReachTarget()
        {
            isMoving = false;
            hasTarget = false;
            enabled = false;
            OnTargetReached?.Invoke();
        }

        #endregion

        #region Public Methods

        public void MoveTo(Transform _target)
        {
            if (_target == null)
            {
                Debug.LogWarning("Target transform is null!");
                StopMove();
                return;
            }

            target = _target;
            StartMoving();
        }
        public void MoveTo(Vector3 _targetPosition)
        {
            target = null;
            targetPosition = _targetPosition;
            StartMoving();
        }
        public void StopMove()
        {
            if (isMoving)
            {
                isMoving = false;
                hasTarget = false;
                enabled = false;
                OnStopMove?.Invoke();
            }
        }
        public void SetSpeed(float _speed)
        {
            moveSpeed = _speed;
        }
        public void SetStopDistance(float _dis)
        {
            stopDistance = _dis;
        }

        #endregion
    }
}
