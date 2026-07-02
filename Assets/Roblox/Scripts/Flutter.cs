using UnityEngine;

public class Flutter : MonoBehaviour
{
    [SerializeField] private RectTransform targetButton;
    [SerializeField] private float pulseSpeed = 0.1f;
    [SerializeField] private float pulseAmount = 0.1f;

    [SerializeField] private Vector3 originalScale;

    private void Start()
    {
        if (targetButton == null)
            targetButton = GetComponent<RectTransform>();

        originalScale = targetButton.localScale;
    }

    private void Update()
    {
        var scaleOffset = Mathf.PingPong(Time.time * pulseSpeed, pulseAmount);
        var currentScale = 1 + scaleOffset;

        targetButton.localScale = originalScale * currentScale;
    }
}