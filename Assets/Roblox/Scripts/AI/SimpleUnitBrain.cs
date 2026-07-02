using ExampleProject.Gameplay.Faction;
using ExampleProject.Gameplay.Scenes;
using ExampleProject.Gameplay.Unit;
using ExampleProject.Interface;
using UnityEngine;

namespace ExampleProject.AI
{
    public class SimpleUnitBrain : MonoBehaviour
    {
        public enum TargetSearchMode
        {
            ClosestGlobal,
            ClosestInAttackRange
        }

        [SerializeField] BaseUnit unit;
        [SerializeField] TargetSearchMode targetSearchMode = TargetSearchMode.ClosestGlobal;
        [SerializeField] float updateInterval = 0.2f;

        float nextUpdateTime;

        void Awake()
        {
            if (unit == null)
            {
                unit = GetComponent<BaseUnit>();
            }
        }

        void Update()
        {
            if (unit == null || unit.IsDead)
                return;

            if (unit.IsHasTarget)
            {
                enabled = false;
                return;
            }

            if (GameplayController.Instance == null || GameplayController.Instance.GameplayState != GameplayState.Fight)
                return;

            if (Time.time < nextUpdateTime)
                return;

            nextUpdateTime = Time.time + updateInterval;

            IDamageable target = FindTarget();
            if (target != null)
            {
                unit.SetTarget(target);
            }
        }

        IDamageable FindTarget()
        {
            if (targetSearchMode == TargetSearchMode.ClosestInAttackRange)
            {
                return FindClosestTargetInAttackRange();
            }

            BaseUnit targetUnit = GameplayController.Instance.FindClosestTarget(unit);
            return targetUnit != null ? targetUnit.Damageable : null;
        }

        IDamageable FindClosestTargetInAttackRange()
        {
            Collider[] hitColliders = Physics.OverlapSphere(unit.transform.position, unit.AttackRange);

            IDamageable closest = null;
            float minSqrDist = float.MaxValue;
            Vector3 unitPosition = unit.transform.position;

            foreach (Collider hitCollider in hitColliders)
            {
                IDamageable target = hitCollider.GetComponent<IDamageable>();
                if (!IsValidTarget(target))
                    continue;

                float sqrDist = (target.GroundTransform.position - unitPosition).sqrMagnitude;
                if (sqrDist < minSqrDist)
                {
                    minSqrDist = sqrDist;
                    closest = target;
                }
            }

            return closest;
        }

        bool IsValidTarget(IDamageable target)
        {
            if (target == null)
                return false;

            if (target == unit.Damageable)
                return false;

            if (target.IsDead)
                return false;

            if (target.Faction == unit.Faction)
                return false;

            return true;
        }
    }
}
