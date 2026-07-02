using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public float Horizontal
    {
        get { return (snapX) ? SnapFloat(input.x, AxisOptions.Horizontal) : input.x; }
    }

    public float Vertical
    {
        get { return (snapY) ? SnapFloat(input.y, AxisOptions.Vertical) : input.y; }
    }

    public Vector2 Direction
    {
        get { return new Vector2(Horizontal, Vertical); }
    }

    public float HandleRange
    {
        get { return handleRange; }
        set { handleRange = Mathf.Abs(value); }
    }

    public float DeadZone
    {
        get { return deadZone; }
        set { deadZone = Mathf.Abs(value); }
    }

    public AxisOptions AxisOptions
    {
        get { return axisOptions; }
        set { axisOptions = value; }
    }

    public bool SnapX
    {
        get { return snapX; }
        set { snapX = value; }
    }

    public bool SnapY
    {
        get { return snapY; }
        set { snapY = value; }
    }

    [SerializeField] private float handleRange = 1;
    [SerializeField] private float deadZone = 0;
    [SerializeField] private AxisOptions axisOptions = AxisOptions.Both;
    [SerializeField] private bool snapX = false;
    [SerializeField] private bool snapY = false;

    [SerializeField] protected RectTransform background = null;
    [FormerlySerializedAs("handle")]
    [SerializeField] private RectTransform handlee;
    [SerializeField] private RectTransform baseRect = null;

    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera cam;

    private Vector2 input = Vector2.zero;

    protected virtual void Start()
    {
        HandleRange = handleRange;
        DeadZone = deadZone;
        ResolveReferences();

        if (background == null || handlee == null)
        {
            Debug.LogError($"{name}: Joystick is missing background or handle RectTransform.", this);
            enabled = false;
            return;
        }

        baseRect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("The Joystick is not placed inside a canvas");
            enabled = false;
            return;
        }

        Vector2 center = new Vector2(0.5f, 0.5f);
        background.pivot = center;
        handlee.anchorMin = center;
        handlee.anchorMax = center;
        handlee.pivot = center;
        handlee.anchoredPosition = Vector2.zero;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (background == null || handlee == null || canvas == null) return;

        cam = null;
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            cam = canvas.worldCamera;

        Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
        Vector2 radius = background.sizeDelta / 2;
        input = (eventData.position - position) / (radius * canvas.scaleFactor);
        FormatInput();
        HandleInput(input.magnitude, input.normalized, radius, cam);
        handlee.anchoredPosition = input * radius * handleRange;
    }

    protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > deadZone)
        {
            if (magnitude > 1)
                input = normalised;
        }
        else
            input = Vector2.zero;
    }

    private void FormatInput()
    {
        if (axisOptions == AxisOptions.Horizontal)
            input = new Vector2(input.x, 0f);
        else if (axisOptions == AxisOptions.Vertical)
            input = new Vector2(0f, input.y);
    }

    private float SnapFloat(float value, AxisOptions snapAxis)
    {
        if (value == 0)
            return value;

        if (axisOptions == AxisOptions.Both)
        {
            float angle = Vector2.Angle(input, Vector2.up);
            if (snapAxis == AxisOptions.Horizontal)
            {
                if (angle < 22.5f || angle > 157.5f)
                    return 0;
                else
                    return (value > 0) ? 1 : -1;
            }
            else if (snapAxis == AxisOptions.Vertical)
            {
                if (angle > 67.5f && angle < 112.5f)
                    return 0;
                else
                    return (value > 0) ? 1 : -1;
            }

            return value;
        }
        else
        {
            if (value > 0)
                return 1;
            if (value < 0)
                return -1;
        }

        return 0;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        if (handlee != null)
        {
            handlee.anchoredPosition = Vector2.zero;
        }
    }

    protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        Vector2 localPoint = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint))
        {
            Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
            return localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
        }

        return Vector2.zero;
    }

    private void ResolveReferences()
    {
        if (background == null)
        {
            Transform backgroundTransform = transform.Find("Background");
            if (backgroundTransform == null)
            {
                backgroundTransform = transform.Find("JoystickRBackground");
            }

            if (backgroundTransform != null)
            {
                background = backgroundTransform.GetComponent<RectTransform>();
            }
        }

        if (handlee == null)
        {
            Transform handleTransform = transform.Find("Background/Handle");
            if (handleTransform == null && background != null)
            {
                handleTransform = background.Find("Handle");
            }
            if (handleTransform == null)
            {
                handleTransform = transform.Find("Handle");
            }
            if (handleTransform == null)
            {
                handleTransform = transform.Find("JoystickRKnob");
            }

            if (handleTransform != null)
            {
                handlee = handleTransform.GetComponent<RectTransform>();
            }
        }
    }
}

public enum AxisOptions
{
    Both,
    Horizontal,
    Vertical
}
