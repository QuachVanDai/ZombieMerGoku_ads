using BreakInfinity;
using ExampleProject.Gameplay.Faction;
using ExampleProject.Gameplay.Unit;
using ExampleProject.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using ExampleProject.Gameplay.Buffs;
using ExampleProject.Gameplay.Scenes;
using UnityEngine;
using Random = UnityEngine.Random;
using Units = ExampleProject.Gameplay.Unit.Units;

namespace ExampleProject
{
    public class Tower : BaseUnit
    {
        #region Fields

        [SerializeField] GameObject healthBar;
        [SerializeField] GameObject meshPieceParent;
        [SerializeField] private GameObject model;
        [SerializeField] bool isMainTower = false;
        [SerializeField] float pieceFallDuration = 0.7f;
        [SerializeField] float pieceFallDistance = 2.5f;
        [SerializeField] float pieceScatterDistance = 0.6f;
        [SerializeField] float pieceSpinAngle = 45f;
        #endregion

        #region LifeCycle

        protected override void OnEnable()
        {
            if (Damageable != null)
                Damageable.OnDie += OnDieAnimationListener;
        }

        protected override void OnDisable()
        {
            if (Damageable != null)
                Damageable.OnDie -= OnDieAnimationListener;
        }

        #endregion

        #region Die Logic

        private void OnDieAnimationListener()
        {
            OnDieAnimation(this);
        }
	
        private void OnDieAnimation(BaseUnit _unit)
        {
            if (model != null)
                model.SetActive(false);
            if (Col != null)
                Col.enabled = false;
            if (meshPieceParent == null)
                return;

            meshPieceParent.SetActive(true);
            StartCoroutine(AnimateTowerPieces());

            if(!isMainTower)
                StartCoroutine(FadeOutAndDestroyPieces());
        }

        private IEnumerator AnimateTowerPieces()
        {
            MeshRenderer[] renderers = meshPieceParent.GetComponentsInChildren<MeshRenderer>();
            List<Transform> pieces = new List<Transform>();
            foreach (MeshRenderer rend in renderers)
            {
                if (rend == null || rend.transform == meshPieceParent.transform)
                    continue;
                if (!pieces.Contains(rend.transform))
                    pieces.Add(rend.transform);
            }

            if (pieces.Count == 0)
                yield break;

            Vector3[] startPositions = new Vector3[pieces.Count];
            Vector3[] targetPositions = new Vector3[pieces.Count];
            Quaternion[] startRotations = new Quaternion[pieces.Count];
            Quaternion[] targetRotations = new Quaternion[pieces.Count];

            for (int i = 0; i < pieces.Count; i++)
            {
                Transform piece = pieces[i];
                startPositions[i] = piece.localPosition;
                startRotations[i] = piece.localRotation;

                Vector3 scatter = new Vector3(
                    Random.Range(-pieceScatterDistance, pieceScatterDistance),
                    -pieceFallDistance * Random.Range(0.75f, 1.15f),
                    Random.Range(-pieceScatterDistance, pieceScatterDistance));

                targetPositions[i] = startPositions[i] + scatter;
                targetRotations[i] = startRotations[i] * Quaternion.Euler(
                    Random.Range(-pieceSpinAngle, pieceSpinAngle),
                    Random.Range(-pieceSpinAngle, pieceSpinAngle),
                    Random.Range(-pieceSpinAngle, pieceSpinAngle));
            }

            float elapsed = 0f;
            float duration = Mathf.Max(0.01f, pieceFallDuration);
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                t = t * t * (3f - 2f * t);

                for (int i = 0; i < pieces.Count; i++)
                {
                    if (pieces[i] == null)
                        continue;

                    pieces[i].localPosition = Vector3.Lerp(startPositions[i], targetPositions[i], t);
                    pieces[i].localRotation = Quaternion.Lerp(startRotations[i], targetRotations[i], t);
                }

                yield return null;
            }
        }

        #endregion

        #region Fade Logic

        private IEnumerator FadeOutAndDestroyPieces()
        {
            float duration = 1f;
            float elapsed = 0f;

            MeshRenderer[] renderers = meshPieceParent.GetComponentsInChildren<MeshRenderer>();
            List<Material> materials = new List<Material>();

            // ✅ Tạo material instance 1 lần duy nhất
            foreach (var rend in renderers)
            {
                Material mat = new Material(rend.material);

                // 🔥 Chuyển sang Transparent (support cả Built-in + URP)
                SetupMaterialForTransparency(mat);

                rend.material = mat;
                materials.Add(mat);
            }

            // 🎬 Fade
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                // ease out cho đẹp
                float alpha = Mathf.Lerp(1f, 0f, t * t);

                foreach (var mat in materials)
                {
                    if (mat.HasProperty("_Color"))
                    {
                        Color c = mat.color;
                        c.a = alpha;
                        mat.color = c;
                    }
                    else if (mat.HasProperty("_BaseColor"))
                    {
                        Color c = mat.GetColor("_BaseColor");
                        c.a = alpha;
                        mat.SetColor("_BaseColor", c);
                    }
                }

                yield return null;
            }

            Destroy(meshPieceParent);
        }

        // 🔧 Setup material transparency
        private void SetupMaterialForTransparency(Material mat)
        {
            // Built-in Standard Shader
            if (mat.HasProperty("_Mode"))
            {
                mat.SetFloat("_Mode", 2); // Fade

                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);

                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");

                mat.renderQueue = 3000;
            }

            // URP Shader
            if (mat.HasProperty("_Surface"))
            {
                mat.SetFloat("_Surface", 1); // Transparent
                mat.SetFloat("_Blend", 0);

                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);

                mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                mat.renderQueue = 3000;
            }
        }

        #endregion

        #region Public Methods

        public override void SetData(object _id)
        {
            base.SetData(_id);
            unitData = Units.GetTowerResourceData((UnitId)_id);
        }

        public override void Init()
        {
            if (unitData == null)
                return;

            if (Damageable != null)
                Damageable.SetHealth((BigDouble)unitData.health, Faction);

            IBasicAttack basicAttack = BasicAttack;
            if (basicAttack != null)
            {
                DamageInfor damageInfor = new DamageInfor((BigDouble)unitData.damage, Faction, gameObject);
                basicAttack.SetDamage(damageInfor);
                basicAttack.SetAttackSpeed(unitData.attackSpeed);
            }

            ITargetMovable targetMovable = TargetMovable;
            if (targetMovable != null)
                targetMovable.MoveSpeed = unitData.moveSpeed;

            SetScale(unitData.scale);
        }

        public void SetHealth(BigDouble _towerHealth)
        {
            if (Damageable != null)
                Damageable.SetHealth(_towerHealth, Faction);
        }

        public override void AddBuffs(List<Buff> _buffs)
        {
            if (Damageable == null || _buffs == null || _buffs.Count == 0)
                return;

            base.AddBuffs(_buffs);
        }

        public void CanDamage(bool _canDamage)
        {
            if (_canDamage)
            {
                if (healthBar != null)
                    healthBar.SetActive(true);
                if (Col != null)
                    Col.enabled = true;
            }
            else
            {
                if (healthBar != null)
                    healthBar.SetActive(false);
                if (Col != null)
                    Col.enabled = false;
            }
        }

        #endregion
    }
}
