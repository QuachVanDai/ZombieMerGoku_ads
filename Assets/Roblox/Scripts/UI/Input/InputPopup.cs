using ExampleProject.Manager;
using ExampleProject.UI.BaseUI.BasePopup;
using ExampleProject.UI.Shared;
using StarterAssets;
using UnityEngine;

namespace ExampleProject.UI.Input
{
    public class InputPopup : BasePopup
    {
        #region Fields

        [SerializeField] UICanvasControllerInput uiCanvasControllerInput;
        [SerializeField] Joystick moveJoystick;


        [SerializeField] GameObject uIButtonJump, uIButtonAtk;
        [SerializeField] CanvasGroup canvasGroupMoveJoystick;
        
      

        #endregion

        #region Properties

        public Vector2 MoveDirection
        {
            get
            {
                EnsureMoveJoystick();
                return moveJoystick != null ? moveJoystick.Direction : Vector2.zero;
            }
        }

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods

        protected override void AddListener()
        {
            base.AddListener();

        }
        protected override void RemoveListener()
        {
            base.RemoveListener();
          
        }

        void OnSetSafeAreaListener()
        {

        }

        protected override void Init()
        {
            base.Init();
            EnsureInputController();
            EnsureMoveJoystick();
        }


        #endregion

        #region Public Methods

        public override void Show()
        {
            SetActive(true);
            EnsureInputController();
            EnsureMoveJoystick();
            ForceVisible();
     //       Debug.Log($"[InputPopup] Show. activeSelf={gameObject.activeSelf}, activeInHierarchy={gameObject.activeInHierarchy}, joystick={moveJoystick != null}, joystickActive={(moveJoystick != null && moveJoystick.gameObject.activeInHierarchy)}, canvasGroup={GetCanvasGroupDebug()}, direction={MoveDirection}");
        }

        public override void Hide()
        {
            //    Debug.Log($"[InputPopup] Hide. activeSelf={gameObject.activeSelf}, activeInHierarchy={gameObject.activeInHierarchy}");
            SetActive(false);
        }

        public void SetStarterAssetsInputs(StarterAssetsInputs _starterAssetsInputs)
        {
            EnsureInputController();
            EnsureMoveJoystick();
            if (uiCanvasControllerInput == null) return;

            uiCanvasControllerInput.SetStarterAssetsInputs(_starterAssetsInputs);
            uiCanvasControllerInput.SetMoveJoystick(moveJoystick);
        }

        public void ShowOnlyMoveJoystick(bool _isShowOnly)
        {
            if (uIButtonAtk != null)
                uIButtonAtk.SetActive(!_isShowOnly);
            if (uIButtonJump != null)
                uIButtonJump.SetActive(!_isShowOnly);
        }
	        
        public void SetAlphaMoveJoystick(float _alpha)
        {
            if (canvasGroupMoveJoystick != null)
            {
                canvasGroupMoveJoystick.gameObject.SetActive(true);
                canvasGroupMoveJoystick.alpha = _alpha;
            }
        }

        public bool HasMoveInput(float threshold)
        {
            return MoveDirection.sqrMagnitude > threshold * threshold;
        }

        void EnsureMoveJoystick()
        {
            if (moveJoystick == null)
            {
                moveJoystick = GetComponentInChildren<Joystick>(true);
            }
        }

        void EnsureInputController()
        {
            if (uiCanvasControllerInput == null)
            {
                uiCanvasControllerInput = GetComponentInChildren<UICanvasControllerInput>(true);
            }
        }

        void ForceVisible()
        {
            if (moveJoystick != null)
                moveJoystick.gameObject.SetActive(true);

            if (canvasGroupMoveJoystick != null)
            {
                canvasGroupMoveJoystick.gameObject.SetActive(true);
                canvasGroupMoveJoystick.alpha = 1f;
                canvasGroupMoveJoystick.interactable = true;
                canvasGroupMoveJoystick.blocksRaycasts = true;
            }
        }

        #endregion
    }
}
