using ExampleProject;
using ExampleProject.Gameplay.Scenes;
using ExampleProject.Gameplay.Unit;
using ExampleProject.GameSystem;
using UnityEngine;

public class MergeMelee : MonoBehaviour
{
    [Header("Drag")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float dragRadius;

    [Header("Merge")]
    [SerializeField] private float mergeRadius = 0.5f;
    [SerializeField] UnitSetupData unitSetupData;
    [SerializeField] CreepUnit CreepUnit;
    [SerializeField] AudioClip mergeSound;
    [SerializeField] Transform psMerge;
    private bool isDragging;
    private Vector3 startPosition;
    private Vector3 offset;
    private Plane dragPlane;

    private MergeMelee targetMerge;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        startPosition = transform.position;

        // Tạo mặt phẳng ngang đi qua vị trí hiện tại
        dragPlane = new Plane(Vector3.up, startPosition);

        if (TryGetMousePoint(out Vector3 hitPoint))
        {
            offset = transform.position - hitPoint;
        }

        isDragging = true;
        targetMerge = null;
    }

    private void OnMouseDrag()
    {
        if (!isDragging)
            return;

        if (TryGetMousePoint(out Vector3 hitPoint))
        {
            Vector3 targetPos = hitPoint + offset;

            // Giới hạn theo bán kính quanh vị trí ban đầu
            Vector3 direction = targetPos - startPosition;

            if (direction.magnitude > dragRadius)
            {
                targetPos = startPosition + direction.normalized * dragRadius;
            }

            transform.position = targetPos;

            CheckMergeTarget();
        }
    }
    private void OnMouseUp()
    {
        isDragging = false;

        if (targetMerge != null)
        {
            Merge(targetMerge);
        }
        else
        {
            transform.position = startPosition;
        }
    }

    private bool TryGetMousePoint(out Vector3 point)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (dragPlane.Raycast(ray, out float enter))
        {
            point = ray.GetPoint(enter);
            return true;
        }

        point = Vector3.zero;
        return false;
    }

    private void CheckMergeTarget()
    {
        if (targetMerge != null)
            targetMerge.psMerge.gameObject.SetActive(false);
        targetMerge = null;

        Collider[] hits = Physics.OverlapSphere(transform.position + Vector3.up, mergeRadius);

        foreach (Collider hit in hits)
        {
            if (hit.gameObject == gameObject)
                continue;

            if (!hit.CompareTag(tag))
                continue;

            MergeMelee other = hit.GetComponent<MergeMelee>();

            if (other == null)
                continue;

            targetMerge = other;
            targetMerge.psMerge.gameObject.SetActive(true);
            return;
        }
    }

    private void Merge(MergeMelee other)
    {
        Debug.Log($"Merge {name} + {other.name}");

        // TODO:
        // Spawn item mới ở đây
        GameplayController.Instance.SpawnSingleGruntUnit2(unitSetupData, 12);
        GameplayController.Instance.HideTutorial();
        GameplayController.Instance.GameplayState = GameplayState.ArmyIntro;
        SoundSystem.Instance.PlaySoundOneShot(mergeSound);
        Destroy(other.gameObject);
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, mergeRadius);
    }
#endif
}