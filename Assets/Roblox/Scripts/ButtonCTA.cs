using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.UI.Playable
{
    public class ButtonCTA : MonoBehaviour
    {
        
        [SerializeField] Button buttonCta;

        private void OnEnable()
        {
        

       
            buttonCta.onClick.AddListener(OnCTA);
        }

        private void OnDisable()
        {
      
            buttonCta.onClick.RemoveListener(OnCTA);
        }

        public void _OnCTA()
        {
            OnCTA();
        }

        public void OnCTA()
        {
            Luna.Unity.Playable.InstallFullGame();
        }

    
    }
}
