using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VTLTools.Effect
{
    public class MainParticleSystem : MonoBehaviour
    {
        [SerializeField] Effect effect;
        ParticleSystem ps;
        public ParticleSystem ThisParticleSystem
        {
            get
            {
                if (ps == null)
                    ps = GetComponent<ParticleSystem>();
                return ps;
            }
        }

        /*void Awake()
        {
            SetupStopCallback();
        }

        void OnParticleSystemStopped()
        {
            if (effect != null)
                effect.OnParticleSystemStoppedListener();
        }

        void SetupStopCallback()
        {
            ParticleSystem particleSystem = ThisParticleSystem;
            if (particleSystem == null)
                return;

            ParticleSystem.MainModule main = particleSystem.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }*/
    }
}
