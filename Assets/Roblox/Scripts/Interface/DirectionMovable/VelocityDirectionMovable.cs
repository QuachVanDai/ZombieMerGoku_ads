using System;
using UnityEngine;
namespace ExampleProject.Interface
{
    [RequireComponent(typeof(Rigidbody))]
    public class VelocityDirectionMovable : MonoBehaviour, IDirectionMovable
    {
        #region Fields
        public event Action OnStartMove;
        public event Action OnUpdate;
        public event Action OnStopMove;
        readonly Rigidbody rb;
        #endregion
        #region Properties
        private Rigidbody Rigidbody => rb == null ? GetComponent<Rigidbody>() : rb;
        public Vector3 Direction { get; set; }
        public bool IsMoving { get; private set; }
        public float MoveSpeed { get; set; }
        #endregion
        #region LifeCycle       
        private void FixedUpdate()
        {
            if (!IsMoving || MoveSpeed == 0)
                return;
            Rigidbody.velocity = Direction.normalized * MoveSpeed;
            OnUpdate?.Invoke();
        }
        #endregion
        #region Private Methods
        #endregion
        #region Public Methods
        public void SetMoveSpeed(float _speed)
        {
            MoveSpeed = _speed;
        }
        public void Move(Vector3 _direction)
        {
            if (_direction == Vector3.zero)
            {
                Stop();
                return;
            }
            Direction = _direction;
            IsMoving = true;
            OnStartMove?.Invoke();
        }
        public void Stop()
        {
            Direction = Vector3.zero;
            IsMoving = false;
            Rigidbody.velocity = Vector3.zero;
            OnStopMove?.Invoke();
        }
        #endregion
    }
}