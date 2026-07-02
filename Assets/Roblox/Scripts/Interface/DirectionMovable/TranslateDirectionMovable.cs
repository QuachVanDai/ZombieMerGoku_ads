using System;
using UnityEngine;
namespace ExampleProject.Interface
{
    public class TranslateDirectionMovable : MonoBehaviour, IDirectionMovable
    {
        #region Fields
        public event Action OnStartMove;
        public event Action OnUpdate;
        public event Action OnStopMove;
        #endregion
        #region Properties
        public Vector3 Direction { get; set; }
        public bool IsMoving { get; private set; }
        public float MoveSpeed { get; set; }
        #endregion
        #region LifeCycle   
        private void Update()
        {
            if (!IsMoving || MoveSpeed == 0)
                return;
            transform.Translate(MoveSpeed * Time.deltaTime * Direction.normalized, Space.World);
            OnUpdate?.Invoke();
        }
        #endregion
        #region Private Methods
        #endregion
        #region Public Methods
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
            OnStopMove?.Invoke();
        }
        #endregion
    }
}