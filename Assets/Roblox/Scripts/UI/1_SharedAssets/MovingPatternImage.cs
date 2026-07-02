using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.UI.Shared
{
    public class MovingPatternImage : MonoBehaviour
    {
        #region Fields

        [SerializeField] float moveDuration;
        [SerializeField] Vector2 moveDirection;

        Image image;
        Material instanceMaterial;

        #endregion

        #region Properties

        Image ThisImage => image == null ? image = GetComponent<Image>() : image;

        #endregion

        #region LifeCycle   

        private void Start()
        {
            StartMove();
        }
        private void OnDisable()
        {
            StopMove();
        }

        #endregion

        #region Private Methods

        void CloneMaterial()
        {
            // Create an instance of the material to avoid modifying the original material
            instanceMaterial = Instantiate(ThisImage.material);
            ThisImage.material = instanceMaterial;
        }

        #endregion

        #region Public Methods

        public void StartMove()
        {
            CloneMaterial();

            instanceMaterial.mainTextureOffset = Vector2.zero;
            instanceMaterial.DOOffset(moveDirection, moveDuration).SetEase(Ease.Linear).SetLoops(-1);
        }

        public void StopMove()
        {
            instanceMaterial.DOKill();
        }

        #endregion
    }
}