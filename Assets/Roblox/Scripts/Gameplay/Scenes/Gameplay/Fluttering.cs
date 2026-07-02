using System.Collections;
using UnityEngine;

public class Fluttering : MonoBehaviour
{
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.2f;
    [SerializeField] private float flutterSpeed = 2f;

    private void Start()
    {
        StartCoroutine(Flutter());
    }

    private IEnumerator Flutter()
    {
        Vector3 baseScale = transform.localScale;

        while (true)
        {
            float t = Mathf.PingPong(Time.time * flutterSpeed, 1f);

            float scaleValue = Mathf.Lerp(minScale, maxScale, t);

            transform.localScale = baseScale * scaleValue;

            yield return null;
        }
    }
}