using UnityEngine;

namespace StarterAssets
{
    public class TactixMovePlayer : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Tham chiếu đến Player (ThirdPersonController)")]
        public ThirdPersonController playerController;

        [Tooltip("Tốc độ di chuyển khi dùng MoveTo (m/s)")]
        public float moveToSpeed = 5.0f;

        private bool _isMovingToTarget = false;
        private Vector3 _targetPosition;
        private Transform _targetTransform;

        // Để điều khiển animation
        private float _currentSpeed = 0f;
        private Animator animator;
        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int MotionSpeedHash = Animator.StringToHash("MotionSpeed");

 

        public void Init()
        {
            if (playerController == null)
            {
                playerController = GetComponent<ThirdPersonController>();
            }

            CacheAnimator();
            enabled = _isMovingToTarget;
        }

        private void Awake()
        {
            CacheAnimator();
            enabled = false;
        }

        private void Update()
        {
            if (_isMovingToTarget && playerController != null)
            {
                MoveTowardsTarget();
            }
        }

        public void MoveTo(Transform targetTransform)
        {
            if (targetTransform == null) return;

            _targetTransform = targetTransform;
            _targetPosition = targetTransform.position;
            _isMovingToTarget = true;
            enabled = true;

          
        }

        public void MoveTo(Vector3 targetPosition)
        {
            _targetTransform = null;
            _targetPosition = targetPosition;
            _isMovingToTarget = true;
            enabled = true;

        }

        private void MoveTowardsTarget()
        {
            if (playerController == null) return;

            Vector3 currentPos = playerController.transform.position;
            Vector3 direction = (_targetPosition - currentPos).normalized;
            float distance = Vector3.Distance(currentPos, _targetPosition);

            // Đến nơi thì dừng
            if (distance < 0.3f)
            {
                StopMoving();
                return;
            }

            // === DI CHUYỂN ===
            Vector3 moveVector = direction * moveToSpeed * Time.deltaTime;
            playerController.transform.position += moveVector;

            // === XỬ LÝ ANIMATION ===
            _currentSpeed = moveToSpeed;                    // Dùng tốc độ đầy đủ
            UpdateAnimation(_currentSpeed, 1f);             // inputMagnitude = 1

            // === XOAY NHÂN VẬT ===
            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                playerController.transform.rotation = Quaternion.Slerp(
                    playerController.transform.rotation, 
                    targetRotation, 
                    12f * Time.deltaTime);
            }
        }

        private void UpdateAnimation(float speed, float inputMagnitude)
        {
            if (!playerController) return;

            CacheAnimator();
            if (animator != null)
            {
                animator.SetFloat(SpeedHash, speed);
                animator.SetFloat(MotionSpeedHash, inputMagnitude);
            }
        }

        private void CacheAnimator()
        {
            if (animator != null)
                return;

            if (playerController != null)
                animator = playerController.GetComponent<Animator>();
        }

        public void StopMoving()
        {
            _isMovingToTarget = false;
            _currentSpeed = 0f;
            enabled = false;

            UpdateAnimation(0f, 0f);        // Dừng animation

          
        }

       
    }
}
