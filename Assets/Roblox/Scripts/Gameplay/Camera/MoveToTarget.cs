using System.Collections;
using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float duration = 1f;
    [SerializeField] private AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine moveCoroutine;

    public void Move()
    {
        if (target == null)
            return;

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        Vector3 _startPosition = transform.position;
        Quaternion _startRotation = transform.rotation;

        float _time = 0f;

        while (_time < duration)
        {
            _time += Time.deltaTime;

            float _progress = Mathf.Clamp01(_time / duration);
            float _curveValue = moveCurve.Evaluate(_progress);

            transform.position = Vector3.Lerp(_startPosition, target.position, _curveValue);
            transform.rotation = Quaternion.Slerp(_startRotation, target.rotation, _curveValue);

            yield return null;
        }

        transform.position = target.position;
        transform.rotation = target.rotation;

        moveCoroutine = null;
    }
}