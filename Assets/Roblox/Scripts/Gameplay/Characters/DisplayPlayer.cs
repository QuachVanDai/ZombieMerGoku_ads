using ExampleProject.Gameplay.Characters.Skin;
using ExampleProject.Gameplay.Weapon;
using ExampleProject.Gameplay.Wing;
using ExampleProject.Tools;
using UnityEngine;

namespace ExampleProject.Gameplay.Characters
{
    public class DisplayPlayer : BasePlayer
    {
        #region Fields



        #endregion

        #region Properties



        #endregion

        #region LifeCycle   

        protected override void OnEnable()
        {
            base.OnEnable();
            Init();
        }

        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public override void Init()
        {
            base.Init();
            Helpers.SetLayerAllChildren(transform, StringsSafeAccess.LAYER_UI_3D);
        }

        public override void SetWeapon(WeaponId _id)
        {
            base.SetWeapon(_id);
            Helpers.SetLayerAllChildren(transform, StringsSafeAccess.LAYER_UI_3D);
        }
        public override void SetWing(WingId _id)
        {
            base.SetWing(_id);
            Helpers.SetLayerAllChildren(transform, StringsSafeAccess.LAYER_UI_3D);
        }
        override public void SetSkin(SkinId _id)
        {
            base.SetSkin(_id);
            Helpers.SetLayerAllChildren(transform, StringsSafeAccess.LAYER_UI_3D);

            characterAnimator.SetAnimatorOverrideController(Skins.GetResourceData(_id).displayAnimatorOverrideController);
        }
        #endregion
    }
}
