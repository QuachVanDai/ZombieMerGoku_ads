using System;
using UnityEngine;

namespace ExampleProject.Interface
{
    public interface IFollowTarget
    {
        Transform Target { get; set; }
        bool IsFollowing { get; }
        float FollowSpeed { get; set; }
        float FollowDistance { get; set; }

        event Action OnStartFollow;
        event Action OnStopFollow;
        event Action OnFollowingUpdate;
        event Action OnTargetLost;

        void StartFollowing(Transform _target);
        void StopFollowing();
        void SetFollowSpeed(float _speed);
        void SetFollowDistance(float _distance);
    }
}
