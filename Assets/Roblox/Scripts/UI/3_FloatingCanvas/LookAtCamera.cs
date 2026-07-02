using UnityEngine;
using ExampleProject.Gameplay.Scenes;

namespace ExampleProject.UI.FloatingCanvas
{
    public class LookAtCamera : MonoBehaviour
    {
        #region Fields

        [SerializeField] Camera _mainCamera;
        [SerializeField] private Vector3 rotationOffset = new Vector3(0f, 0f, 0f);

        #endregion

        #region Properties



        #endregion

        #region LifeCycle


        private void LateUpdate()
        {
            Camera targetCamera = GameplayController.Instance != null && GameplayController.Instance.GameplayCamera != null
                ? GameplayController.Instance.GameplayCamera
                : _mainCamera;

            if (targetCamera != null)
            {
                transform.LookAt(transform.position + targetCamera.transform.rotation * Vector3.forward);
                transform.Rotate(rotationOffset, Space.Self);
            }
        }

        #endregion

        #region Private Methods



        #endregion

        #region Public Methods



        #endregion
    }
}
