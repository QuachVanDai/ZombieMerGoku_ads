using ExampleProject.Gameplay.Characters;
using ExampleProject.Gameplay.Scenes;
using UnityEngine;

namespace ExampleProject.Gameplay.GameplayCamera
{
    public class PlayerCameraFollow : MonoBehaviour
    {
        [SerializeField] PlayablePlayer player;
        [SerializeField] Transform target;
        [SerializeField] Vector3 offset;
        [SerializeField] float followSmoothTime = 0.08f;
        [SerializeField] bool followActive = true;

        Vector3 velocity;
        bool hasOffset;
        

        void LateUpdate()
        {
            if (!followActive)
                return;

            if (target == null && !ResolveTarget())
                return;

            Vector3 targetPosition = target.position + offset;
            transform.position = followSmoothTime <= 0f
                ? targetPosition
                : Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followSmoothTime);
        }

        public void SetPlayer(PlayablePlayer newPlayer)
        {
            player = newPlayer;
            target = player != null && player.PlayerCameraRoot != null
                ? player.PlayerCameraRoot
                : player != null
                    ? player.transform
                    : null;
            RecaptureOffset();
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            RecaptureOffset();
        }

        public void RestartFollow()
        {
            SetFollowActive(true, true);
        }

        public void SetFollowActive(bool active)
        {
            SetFollowActive(active, active);
        }

        public void SetFollowActive(bool active, bool recaptureOffset)
        {
            followActive = active;
            velocity = Vector3.zero;
            if (followActive && recaptureOffset)
                RecaptureOffset();
        }

         public void CaptureOffset()
        {
            if (hasOffset)
                return;

            if (target == null && !ResolveTarget())
                return;

            offset = transform.position - target.position;
            hasOffset = true;
        }

        void RecaptureOffset()
        {
            hasOffset = false;
            velocity = Vector3.zero;
            CaptureOffset();
        }

        bool ResolveTarget()
        {
            if (target != null)
                return true;

            if (player == null && GameplayController.Instance != null)
                player = GameplayController.Instance.Player;

            if (player == null)
                player = FindObjectOfType<PlayablePlayer>();

            if (player == null)
                return false;

            target = player.PlayerCameraRoot != null ? player.PlayerCameraRoot : player.transform;
            return target != null;
        }
    }
}
