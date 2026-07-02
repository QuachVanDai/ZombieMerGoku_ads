using ExampleProject.Gameplay.Characters;
using ExampleProject.Gameplay.Characters.Skin;
using UnityEngine;

namespace ExampleProject.UI.Shared
{
    public class UI3DSkinDisplay : MonoBehaviour
    {
        #region Fields

        [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
        [SerializeField] CharacterAnimator characterAnimator;

        #endregion

        #region Properties



        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public void Init(SkinId _id)
        {
            var _data = Skins.GetResourceData(_id);
            if (_data == null || skinnedMeshRenderer == null)
                return;

            if (_data.mesh != null)
                skinnedMeshRenderer.sharedMesh = _data.mesh;

            if (_data.material != null)
                skinnedMeshRenderer.sharedMaterial = _data.material;

            if (characterAnimator != null && _data.displayAnimatorOverrideController != null)
                characterAnimator.SetAnimatorOverrideController(_data.displayAnimatorOverrideController);
        }

        #endregion
    }
}
