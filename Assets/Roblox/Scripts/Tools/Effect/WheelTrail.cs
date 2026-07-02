using UnityEngine;

namespace VTLTools.Effect
{
    public class WheelTrail : MonoBehaviour
    {
        [SerializeField] Transform _trail;

        void Start()
        {
            Invoke(nameof(ShowTrail), 0.5f);
        }

        public void ShowTrail()
        {
            _trail.gameObject.SetActive(true);
        }
    }
}