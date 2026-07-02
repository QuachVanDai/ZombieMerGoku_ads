using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VTLTools.Effect
{
    public class Effect : MonoBehaviour
    {
        [SerializeField] MainParticleSystem mainParticleSystem;
        [SerializeField] bool isDestroyAfterStop = true;

        public bool IsPlaying
        {
            get
            {
                if (mainParticleSystem != null)
                    return mainParticleSystem.ThisParticleSystem.isPlaying;
                else
                    return false;
            }
        }

        public void OnParticleSystemStoppedListener()
        {
            if (isDestroyAfterStop)
                Destroy(this.gameObject);
        }

        public void Init(Vector3 _pos, Transform _parent = null)
        {
            this.transform.position = _pos;
            this.transform.SetParent(_parent);
        }

        public void DOMoveY(float _value, float _duration)
        {
            this.transform.DOMoveY(_value, _duration);
        }

        public void Play()
        {
            //if (IsPlaying)
            //    return;
            mainParticleSystem.ThisParticleSystem.Play();
        }

        public void Stop()
        {
            mainParticleSystem.ThisParticleSystem.Stop();
        }

        public void Pause()
        {
            mainParticleSystem.ThisParticleSystem.Pause();
        }

        public void SetRateOverTime(float _value)
        {
            var emission = mainParticleSystem.ThisParticleSystem.emission;
            emission.rateOverTime = _value;
        }

        public void DOMove(Vector3 _target, float _duration, float _delay, Action _onCompleteAction = null)
        {
            this.transform.DOMove(_target, _duration).SetDelay(_delay)
                .OnComplete(() => { _onCompleteAction?.Invoke(); });
        }
        public virtual void SetColor(Color _color)
        {
            if (mainParticleSystem == null || mainParticleSystem.ThisParticleSystem == null)
                return;

            var _main = mainParticleSystem.ThisParticleSystem.main;
            _main.startColor = _color;
        }

        // Fix burst, only for ui swarm effect
        public void SetBurstCount(short _count)
        {
            var _emission = mainParticleSystem.ThisParticleSystem.emission;
            _count = (short)Mathf.Clamp(_count, 0, 10);
            _emission.rateOverTime = _count;
        }

        public void SetActive(bool _active)
        {
            this.gameObject.SetActive(_active);
        }
    }
}
