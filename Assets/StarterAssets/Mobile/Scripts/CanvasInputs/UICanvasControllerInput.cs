using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {

        [Header("Output")]
        [SerializeField] StarterAssetsInputs starterAssetsInputs;
        [SerializeField] Joystick moveJoystick;

        public void SetStarterAssetsInputs(StarterAssetsInputs _starterAssetsInputs)
        {
            starterAssetsInputs = _starterAssetsInputs;
        }

        public void SetMoveJoystick(Joystick _moveJoystick)
        {
            moveJoystick = _moveJoystick;
        }

        private void Update()
        {
            if (starterAssetsInputs == null || moveJoystick == null) return;

            starterAssetsInputs.MoveInput(moveJoystick.Direction);
        }

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            if (starterAssetsInputs == null) return;

            starterAssetsInputs.MoveInput(virtualMoveDirection);
        }
        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            if (starterAssetsInputs == null) return;

            starterAssetsInputs.LookInput(virtualLookDirection);
        }
        public void VirtualJumpInput(bool virtualJumpState)
        {
            if (starterAssetsInputs == null) return;

            starterAssetsInputs.JumpInput(virtualJumpState);
        }
        public void VirtualSprintInput(bool virtualSprintState)
        {
            if (starterAssetsInputs == null) return;

            starterAssetsInputs.SprintInput(virtualSprintState);
        }
        public void VirtualAttackInput(bool virtualAttackState)
        {
            if (starterAssetsInputs == null) return;

            starterAssetsInputs.AttackInput(virtualAttackState);
        }

    }

}
