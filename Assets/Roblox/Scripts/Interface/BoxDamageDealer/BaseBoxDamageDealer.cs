using BreakInfinity;
using ExampleProject.Gameplay.Faction;
using ExampleProject.GameSystem;
using ExampleProject.Interface;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExampleProject.Interface
{
    public class BaseBoxDamageDealer : MonoBehaviour, IBoxDamageDealer
    {
        #region Fields

        [SerializeField] protected Vector3 boxSize = Vector3.one;
        [SerializeField] protected Vector3 offset = Vector3.zero;
        public event Action<IEnumerable<IDamageable>, DamageInfor> OnDealDamage;

        [SerializeField] protected Color gizmoColor = Color.red;
        [SerializeField] protected bool showGizmos = true;

        [SerializeField] List<AudioClip> hitAudioClip;
        [SerializeField] AudioClip criticalAudioClip;

        #endregion

        #region Properties


        public DamageInfor DamageInfor { get; set; }
        public Vector3 BoxSize => boxSize;
        public Vector3 Offset => offset;

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods

        private void OnDrawGizmos()
        {
            if (!showGizmos)
                return;

            Gizmos.color = gizmoColor;
            Vector3 _boxCenter = this.transform.position + this.transform.TransformDirection(offset);

            // Save current matrix, apply rotation, draw, and restore
            Matrix4x4 _originalMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(_boxCenter, this.transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, boxSize);
            Gizmos.matrix = _originalMatrix;
        }

        #endregion

        #region Public Methods

        public void SetDamage(DamageInfor _damageInfor)
        {
            DamageInfor = _damageInfor;
        }

        public void SetBoxSize(Vector3 _boxSize)
        {
            boxSize = _boxSize;
        }

        public void SetOffset(Vector3 _offset)
        {
            offset = _offset;
        }

        public virtual void DealDamageInBox()
        {
            // Calculate the center of the box in world space
            Vector3 _boxCenter = this.transform.position + this.transform.TransformDirection(offset);
            Collider[] _hitColliders = Physics.OverlapBox(_boxCenter, boxSize / 2f, this.transform.rotation);

            // If no colliders are hit, exit early
            if (_hitColliders.Length == 0)
                return;

            // Process each hit collider and apply damage if it has an IDamageable component
            bool _canPlayHitSound = false;
            bool _isHaveCriticalHit = false;
            List<IDamageable> _damagedTargets = new List<IDamageable>();
            foreach (Collider _collider in _hitColliders)
            {
                IDamageable _target = _collider.GetComponent<IDamageable>();

                if (_target != null && !_target.IsDead)
                {
                    _target.TakeDamage(DamageInfor);
                    _damagedTargets.Add(_target);

                    _canPlayHitSound = true;
                    if (DamageInfor.IsCritical)
                        _isHaveCriticalHit = true;
                }
            }
            if(_canPlayHitSound)
            {
                if (_isHaveCriticalHit)
                {
                    SoundSystem.Instance.PlaySoundOneShot(criticalAudioClip);
                }
                else
                {
                    int _randomIndex = UnityEngine.Random.Range(0, hitAudioClip.Count);
                    SoundSystem.Instance.PlaySoundOneShot(hitAudioClip[_randomIndex]);
                }
            }

            // Invoke the OnDealDamage event if there are any damaged targets
            if (_damagedTargets.Count > 0)
            {
                OnDealDamage?.Invoke(_damagedTargets, DamageInfor);
            }
        }

        #endregion
    }
}