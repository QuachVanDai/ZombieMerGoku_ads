using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ExampleProject.Gameplay.Characters
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimator : MonoBehaviour
    {
        #region Fields

        readonly Animator animator;
        public Action onAttackPointReached;
        public Action<AnimationEvent> onAttacStart;
        public Action<AnimationEvent> onJump;
        public Action<AnimationEvent> onLand;
        public Action<AnimationEvent> onFootStep;

        #endregion

        #region Properties

        Animator Animator => animator != null ? animator : GetComponent<Animator>();

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public void Play(string _animationName, Action _onComplete = null)
        {
            Animator.Play(_animationName);
            if (_onComplete != null)
            {
                StartCoroutine(WaitForAnimation(_animationName, _onComplete));
            }
        }
        IEnumerator WaitForAnimation(string _animationName, Action _onComplete)
        {
            // Wait until the animation state is playing
            AnimatorStateInfo _stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
            while (!_stateInfo.IsName(_animationName))
            {
                yield return null;
                _stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
            }

            // Wait until the animation finishes
            while (_stateInfo.normalizedTime < 1.0f)
            {
                yield return null;
                _stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
            }

            _onComplete?.Invoke();
        }
        public void ResetTrigger(string animationName)
        {
            Animator.ResetTrigger(animationName);
        }
        public void CrossFade(string _animationName, float _fadeLength = 0.1f)
        {
            PlaySafe(_animationName, 0f);
        }
        public void PlaySafe(string _animationName, float _normalizedTime = 0f)
        {
            if (Animator == null || string.IsNullOrEmpty(_animationName))
                return;

            if (!Animator.gameObject.activeInHierarchy)
                return;

            Animator.Play(_animationName, 0, Mathf.Clamp01(_normalizedTime));
            Animator.Update(0f);
        }
        public void SetSpeed(float _speed)
        {
            //Animator.speed = _speed;
            Animator.SetFloat("AttackSpeed", _speed);
        }
        public void SetWeaponRunAnim(bool _isHaveWeapon)
        {
            float _value = _isHaveWeapon ? 1f : 0f;
            Animator.SetFloat("IsHaveWeapon", _value);
        }
        public AnimationClip GetRandomAnimationClip()
        {
            var _clips = GetAnimationClip().ToList();
            if (_clips == null || _clips.Count == 0)
                return null;

            int index = Random.Range(0, _clips.Count);
            return _clips[index];
        }
        public float GetAnimatorProgress()
        {
            return Animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
        }
        public void PlayAnimationAtTime(string _animationName, float _normalizedTime)
        {
            if (Animator == null)
                return;

            if (!Animator.gameObject.activeInHierarchy)
                return;

            float _t = Mathf.Clamp01(_normalizedTime);

            Animator.Play(_animationName, 0, _t);
            Animator.Update(0f);
        }
        public IEnumerable<AnimationClip> GetAnimationClip()
        {
            if (Animator == null || Animator.runtimeAnimatorController == null)
                return new List<AnimationClip>();

            return Animator.runtimeAnimatorController.animationClips
                .Select(clip => clip)
                .Distinct()
                .ToList();
        }

        void OnAttackStart(AnimationEvent animationEvent)
        {
            onAttacStart?.Invoke(animationEvent);
        }
        public void OnAttackPointReached()
        {
            onAttackPointReached?.Invoke();
        }
        public void OnFootStepReached(AnimationEvent animationEvent)
        {
            onFootStep?.Invoke(animationEvent);
        }
        public void OnJump(AnimationEvent animationEvent)
        {
            onJump?.Invoke(animationEvent);
        }
        public void OnLand(AnimationEvent animationEvent)
        {
            onLand?.Invoke(animationEvent);
        }

        internal void SetAnimatorOverrideController(AnimatorOverrideController _animatorOverride)
        {
            if (_animatorOverride == null)
                return;

            // Assign the new override controller back to the Animator component
            Animator.runtimeAnimatorController = _animatorOverride;
            Animator.Rebind();
            Animator.Update(0f);
        }
        public float GetAnimationLength(string _originalAnimName)
        {
            AnimatorOverrideController _aoc = Animator.runtimeAnimatorController as AnimatorOverrideController;
            if (_aoc == null)
                return 0f;

            var _overrideAnim = _aoc[_originalAnimName];
            return _overrideAnim != null ? _overrideAnim.length : 0f;
        }    

        #endregion
    }
}
