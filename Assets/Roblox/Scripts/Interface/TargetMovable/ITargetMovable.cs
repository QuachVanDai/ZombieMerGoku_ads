using System;
using UnityEngine;

namespace ExampleProject.Interface
{
    public interface ITargetMovable
    {
        Transform Target { get; set; }
        bool IsMoving { get; }
        float MoveSpeed { get; set; }

        event Action OnStartMove;
        event Action OnStopMove;
        event Action OnTargetReached;
        event Action OnUpdateMove;

        void MoveTo(Transform _target);
        void MoveTo(Vector3 _targetPosition);
        void StopMove();
        void SetSpeed(float _speed);
    }
}