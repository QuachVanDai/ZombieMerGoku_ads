using ExampleProject.UI.BaseUI.BasePopup;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ExampleProject.UI.Playable
{
    public class PlayableEndcardPopup : BasePopup, IPointerClickHandler, IPointerDownHandler
    {
        [SerializeField] bool installOnPopupClick = true;

        bool installRequested;

        protected override void OnShowStarted()
        {
            installRequested = false;
            base.OnShowStarted();
        }

        void InstallFullGame()
        {
            if (installRequested)
                return;

            installRequested = true;
            Luna.Unity.Playable.InstallFullGame();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            TryInstallFromClick();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            TryInstallFromClick();
        }

        void TryInstallFromClick()
        {
            if (!CanInstallFromClick())
                return;

            InstallFullGame();
        }

        bool CanInstallFromClick()
        {
            return installOnPopupClick && IsShow && !installRequested;
        }
    }
}
