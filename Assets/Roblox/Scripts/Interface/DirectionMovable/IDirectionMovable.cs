using System;
using UnityEngine;

namespace ExampleProject.Interface
{
    public interface IDirectionMovable
    {
        event Action OnStartMove;
        event Action OnUpdate;
        event Action OnStopMove;
        Vector3 Direction { get; set; }
        bool IsMoving { get; }
        float MoveSpeed { get; set; }
        void Move(Vector3 _direction);
        void Stop();
    }
}