using System;
using UnityEngine;

namespace ExampleProject.Interface
{
    public class TranslateFollowTarget : MonoBehaviour, IFollowTarget
    {
        #region Fields

        [SerializeField] float followSpeed = 5f;
        [SerializeField] float followDistance = 2f;

        Transform target;
        bool isFollowing;

        #endregion

        #region Properties

        public Transform Target
        {
            get => target;
            set => target = value;
        }

        public bool IsFollowing => isFollowing;

        public float FollowSpeed
        {
            get => followSpeed;
            set => followSpeed = value;
        }

        public float FollowDistance
        {
            get => followDistance;
            set => followDistance = value;
        }

        #endregion

        #region Events

        public event Action OnStartFollow;
        public event Action OnStopFollow;
        public event Action OnFollowingUpdate;
        public event Action OnTargetLost;

        #endregion

        #region LifeCycle

        private void Update()
        {
            if (!isFollowing || target == null)
            {
                if (isFollowing && target == null)
                {
                    TargetLost();
                }
                return;
            }

            Vector3 _direction = (target.position - transform.position).normalized;
            float _distance = Vector3.Distance(transform.position, target.position);

            if (_distance > followDistance)
            {
                transform.Translate(followSpeed * Time.deltaTime * _direction, Space.World);
                OnFollowingUpdate?.Invoke();
            }
        }

        #endregion

        #region Private Methods

        private void StartFollowingInternal()
        {
            if (!isFollowing)
            {
                isFollowing = true;
                OnStartFollow?.Invoke();
            }
        }

        private void TargetLost()
        {
            isFollowing = false;
            OnTargetLost?.Invoke();
        }

        #endregion

        #region Public Methods

        public void StartFollowing(Transform _target)
        {
            if (_target == null)
            {
                Debug.LogWarning("Target transform is null!");
                StopFollowing();
                return;
            }

            target = _target;
            StartFollowingInternal();
        }

        public void StopFollowing()
        {
            if (isFollowing)
            {
                isFollowing = false;
                OnStopFollow?.Invoke();
            }
        }

        public void SetFollowSpeed(float _speed)
        {
            followSpeed = _speed;
        }

        public void SetFollowDistance(float _distance)
        {
            followDistance = _distance;
        }

        #endregion
    }
}
