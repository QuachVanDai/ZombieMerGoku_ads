using ExampleProject.Gameplay.Characters;
using ExampleProject.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace ExampleProject.Gameplay.Unit
{
    public class CreepUnit : BaseUnit
    {
        #region Fields

        [SerializeField] SkinnedMeshRenderer meshRenderer;
        [SerializeField] GameObject zombieTargetMarker;
        public int slot;
        public Transform halo;
        public MergeMelee MergeMelee;

        #endregion

        #region Properties   


        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods

        public void SetSkin()
        {
            if (meshRenderer == null)
                meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>(true);

            if (meshRenderer == null)
            {
                Debug.LogError($"{name}: SetSkin failed because SkinnedMeshRenderer is missing.");
                return;
            }

            if (unitData == null)
            {
                Debug.LogError($"{name}: SetSkin failed because unitData is null.");
                return;
            }

            if (IsGruntUnit(unitData.unitType))
                return;

            if (unitData.mesh != null)
                meshRenderer.sharedMesh = unitData.mesh;

            if (unitData.material != null)
                meshRenderer.sharedMaterial = unitData.material;
        }

        bool IsGruntUnit(UnitType unitType)
        {
            return unitType == UnitType.MeleeGrunt || unitType == UnitType.RangedGrunt;
        }

        #endregion

        #region Public Methods

        public override void SetData(object _id)
        {
            base.SetData(_id);
            unitData = Units.GetUnitData((UnitId)_id);
            if (unitData == null)
                Debug.LogError($"{name}: UnitData not found. unitId={(UnitId)_id}");
        }
        public override void Init()
        {
            base.Init();
            SetSkin();
            SetAnim(idleAnim.name, 1);
            SetZombieTargetMarkerActive(false);
        }

        public void SetZombieTargetMarkerActive(bool isActive)
        {
            if (zombieTargetMarker != null)
                zombieTargetMarker.SetActive(isActive);
        }
        public void SetHaloActive(bool isActive)
        {
            halo.gameObject.SetActive(isActive);
            Destroy(MergeMelee);
        }
        #endregion
    }
}
