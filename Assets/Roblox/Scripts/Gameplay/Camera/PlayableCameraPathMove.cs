using System.Collections;
using UnityEngine;

namespace ExampleProject.Gameplay.GameplayCamera
{
    public class PlayableCameraPathMove : MonoBehaviour
    {
        [SerializeField] Transform orbitTarget;
        [SerializeField] Vector3 orbitTargetOffset = new Vector3(0f, 1.2f, 0f);
        [SerializeField] float moveDuration = 2f;
        [SerializeField] float orbitAngle = 180f;
        [SerializeField] bool clockwise = true;
        [SerializeField] bool deriveOrbitFromCurrentTransformOnPlay = true;
        [SerializeField] float orbitRadius = 10f;
        [SerializeField] float orbitHeight = 4f;
        [SerializeField] float startAngle = -45f;
        [SerializeField] PlayerCameraFollow playerCameraFollow;
        [SerializeField] bool pausePlayerFollowDuringMove = true;
        [SerializeField] bool resumePlayerFollowOnComplete = true;

        bool isPlaying;

        public IEnumerator Play()
        {
            if (isPlaying)
                yield break;

            Transform target = ResolveTarget();
            if (target == null)
                yield break;

            isPlaying = true;

            if (pausePlayerFollowDuringMove && playerCameraFollow != null)
                playerCameraFollow.SetFollowActive(false, false);

            if (deriveOrbitFromCurrentTransformOnPlay)
                DeriveOrbitFromCurrentTransform(target);

            float duration = Mathf.Max(0.01f, moveDuration);
            float elapsed = 0f;
            float fromAngle = startAngle;
            float toAngle = startAngle + (clockwise ? -orbitAngle : orbitAngle);
            Quaternion fromRotation = transform.rotation;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float easedT = Mathf.SmoothStep(0f, 1f, t);
                float angle = Mathf.Lerp(fromAngle, toAngle, easedT);
                ApplyOrbit(target, angle, fromRotation, easedT);
                yield return null;
            }

            ApplyOrbit(target, toAngle, fromRotation, 1f);

            if (resumePlayerFollowOnComplete && playerCameraFollow != null)
                playerCameraFollow.SetFollowActive(true, true);

            isPlaying = false;
        }

        public void Stop()
        {
            isPlaying = false;
            if (resumePlayerFollowOnComplete && playerCameraFollow != null)
                playerCameraFollow.SetFollowActive(true, true);
        }

        Transform ResolveTarget()
        {
            if (orbitTarget != null)
                return orbitTarget;

            return transform.parent;
        }

        void ApplyOrbit(Transform target, float angle, Quaternion fromRotation, float rotationT)
        {
            Vector3 center = target.position + orbitTargetOffset;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Sin(rad) * orbitRadius, orbitHeight, Mathf.Cos(rad) * orbitRadius);
            transform.position = center + offset;

            Vector3 lookDirection = center - transform.position;
            if (lookDirection.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(fromRotation, targetRotation, rotationT);
            }
        }

        void DeriveOrbitFromCurrentTransform(Transform target)
        {
            Vector3 center = target.position + orbitTargetOffset;
            Vector3 offset = transform.position - center;
            Vector2 flatOffset = new Vector2(offset.x, offset.z);

            if (flatOffset.sqrMagnitude > 0.0001f)
                orbitRadius = flatOffset.magnitude;

            orbitHeight = offset.y;
            startAngle = Mathf.Atan2(offset.x, offset.z) * Mathf.Rad2Deg;
        }
    }
}
