using BreakInfinity;
using ExampleProject.Gameplay.Buffs;
using ExampleProject.Gameplay.Characters.Skin;
using ExampleProject.Gameplay.Faction;
using ExampleProject.Gameplay.Unit;
using ExampleProject.Gameplay.Weapon;
using ExampleProject.Gameplay.Wing;
using ExampleProject.GameSystem;
using ExampleProject.Interface;
using ExampleProject.Manager;
using ExampleProject.Tools;
using StarterAssets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace ExampleProject.Gameplay.Characters
{
    public class PlayablePlayer : BasePlayer
    {
        #region Fields

        [SerializeField] Transform playerCameraRoot;
        [FormerlySerializedAs("baseDamage")]
        [SerializeField] float playerDamage = 200f;
        [SerializeField] float baseCriticalChance = 0f;
        [SerializeField] float baseCriticalMultiplier = 1.5f;
        [SerializeField] float baseMoveSpeed = 12f;
        [SerializeField] List<AudioClip> footStepSound;
        [SerializeField] AudioClip landSound;
        [SerializeField] AudioClip jumpSound;
        [SerializeField] AudioClip attackSound;
        [SerializeField] bool autoAttackWhenMoving = true;
        [SerializeField] float autoAttackMoveThreshold = 0.05f;
        IBoxDamageDealer boxDamageDealer;
        readonly ThirdPersonController thirdPersonController;
        readonly StarterAssetsInputs starterAssetsInputs;
        readonly BuffManager buffManager;
        bool gameplayActive = true;

        #endregion

        #region Properties

        public Vector3 Position => transform.position;
        public Transform PlayerCameraRoot => playerCameraRoot;
        public ThirdPersonController ThirdPersonController => thirdPersonController != null ? thirdPersonController : GetComponent<ThirdPersonController>();
        public StarterAssetsInputs StarterAssetsInputs => starterAssetsInputs != null ? starterAssetsInputs : GetComponent<StarterAssetsInputs>();
        public IBoxDamageDealer BoxDamageDealer
        {
            get
            {
                if (boxDamageDealer == null)
                    boxDamageDealer = GetComponent<IBoxDamageDealer>();
                return boxDamageDealer;
            }
        }
        BuffManager BuffManager => buffManager != null ? buffManager : GetComponent<BuffManager>();
        public BigDouble Damage => BoxDamageDealer.DamageInfor.damage;
        public float PlayerDamage => playerDamage;
        public float CriticalChance => BoxDamageDealer.DamageInfor.criticalChance;
        public float CriticalDamage => BoxDamageDealer.DamageInfor.criticalDamage;

        #endregion

        #region LifeCycle

        protected override void OnEnable()
        {
            base.OnEnable();
            characterAnimator.onAttackPointReached += OnAttackPointReachedListener;
            characterAnimator.onFootStep += OnFootStepListener;
            characterAnimator.onJump += OnJumpListener;
            characterAnimator.onLand += OnLandListener;
            characterAnimator.onAttacStart += OnAttackStartListener;

            DamageInfor _damageInfor = new DamageInfor(playerDamage, FactionId.Grunt, this.gameObject);
            BoxDamageDealer.SetDamage(_damageInfor);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            characterAnimator.onAttackPointReached -= OnAttackPointReachedListener;
            characterAnimator.onFootStep -= OnFootStepListener;
            characterAnimator.onJump -= OnJumpListener;
            characterAnimator.onLand -= OnLandListener;
            characterAnimator.onAttacStart -= OnAttackStartListener;
        }

        void Update()
        {
            UpdateAutoAttackWhenMoving();
        }

        #endregion

        #region Private Methods

        void UpdateAutoAttackWhenMoving()
        {
            if (!gameplayActive || !autoAttackWhenMoving || StarterAssetsInputs == null)
                return;

            bool hasMoveInput = StarterAssetsInputs.move.sqrMagnitude >= autoAttackMoveThreshold * autoAttackMoveThreshold;
            StarterAssetsInputs.AttackInput(hasMoveInput);
        }

        void OnAttackPointReachedListener()
        {
            if (!gameplayActive)
                return;

            BoxDamageDealer.DealDamageInBox();
        }
        void OnFootStepListener(AnimationEvent animationEvent)
        {
            if (footStepSound == null || footStepSound.Count == 0)
                return;

            AudioClip _footStepAudioClip = null;
            for (int i = 0; i < footStepSound.Count; i++)
            {
                AudioClip _audioClip = footStepSound[UnityEngine.Random.Range(0, footStepSound.Count)];
                if (_audioClip == null)
                    continue;

                _footStepAudioClip = _audioClip;
                break;
            }

            if (_footStepAudioClip != null)
                SoundSystem.Instance.PlaySoundOneShot(_footStepAudioClip);
        }
        void OnJumpListener(AnimationEvent animationEvent)
        {
            SoundSystem.Instance.PlaySoundOneShot(jumpSound);
        }
        void OnLandListener(AnimationEvent animationEvent)
        {
            SoundSystem.Instance.PlaySoundOneShot(landSound);
        }
        void OnAttackStartListener(AnimationEvent animationEvent)
        {
           SoundSystem.Instance.PlaySoundOneShot(attackSound);
        }

        #endregion

        #region Public Methods

        public override void SetSkin(SkinId _id)
        {
            // Remove the buffs of the old skin if it's not None
            if (skin != SkinId.None)
            {
                var _oldSkinData = Skins.GetResourceData(skin);
                if (_oldSkinData != null && _oldSkinData.buffs != null)
                    RemoveBuffs(_oldSkinData.buffs);
            }

            // Set the new skin
            base.SetSkin(_id);

            // Add the buffs of the new skin if it's not None
            if (_id == SkinId.None)
            {
                EventDispatcher.Instance.Dispatch(EventName.OnPlayerFinishCalculateStats, this);
                return;
            }

            var _newSkinData = Skins.GetResourceData(_id);
            if (_newSkinData != null && _newSkinData.buffs != null)
                AddBuffs(_newSkinData.buffs);
            EventDispatcher.Instance.Dispatch(EventName.OnPlayerFinishCalculateStats, this);

            if (characterAnimator != null && _newSkinData != null && _newSkinData.playableAnimatorOverrideController != null)
                characterAnimator.SetAnimatorOverrideController(_newSkinData.playableAnimatorOverrideController);
        }
        public override void SetWeapon(WeaponId _id)
        {
            // Remove the buffs of the old weapon if it's not None
            if (weapon != null && weapon.id != WeaponId.None)
            {
                var _oldWeaponData = Weapons.GetResourceData(weapon.id);
                if (_oldWeaponData != null && _oldWeaponData.buffs != null)
                    RemoveBuffs(_oldWeaponData.buffs);
            }

            // Set the new weapon
            base.SetWeapon(_id);

            if (_id == WeaponId.None)
            {
                EventDispatcher.Instance.Dispatch(EventName.OnPlayerFinishCalculateStats, this);
                if (characterAnimator != null)
                    characterAnimator.SetWeaponRunAnim(false);
                return;
            }

            // Add the buffs of the new weapon if it's not None
            var _weaponData = Weapons.GetResourceData(_id);
            if (_weaponData != null && _weaponData.buffs != null)
                AddBuffs(_weaponData.buffs);
            EventDispatcher.Instance.Dispatch(EventName.OnPlayerFinishCalculateStats, this);

            // Reset the animator override controller
            if (characterAnimator != null)
                characterAnimator.SetWeaponRunAnim(true);
        }
        public override void SetWing(WingId _id)
        {
            // Remove the buffs of the old wing if it's not None
            if (wing != null && wing.id != WingId.None)
            {
                var _oldWingData = Wings.GetResourceData(wing.id);
                if (_oldWingData != null && _oldWingData.buffs != null)
                    RemoveBuffs(_oldWingData.buffs);
            }

            // Set the new wing
            base.SetWing(_id);

            // Add the buffs of the new wing if it's not None
            if (_id == WingId.None)
            {
                EventDispatcher.Instance.Dispatch(EventName.OnPlayerFinishCalculateStats, this);
                return;
            }

            var _wingData = Wings.GetResourceData(_id);
            if (_wingData != null && _wingData.buffs != null)
                AddBuffs(_wingData.buffs);
            EventDispatcher.Instance.Dispatch(EventName.OnPlayerFinishCalculateStats, this);
        }
        public void SetCamera(Camera _cam)
        {
            ThirdPersonController.SetCamera(_cam);
        }
        void CalculateBuffs()
        {
            var _buffs = BuffManager.GetActiveBuffs();

            //if (_buffs.Count == 0)
            //{
            BoxDamageDealer.SetDamage(new DamageInfor(playerDamage, FactionId.Grunt, this.gameObject));
            BoxDamageDealer.DamageInfor.criticalChance = baseCriticalChance;
            BoxDamageDealer.DamageInfor.criticalDamage = baseCriticalMultiplier;
            ThirdPersonController.MoveSpeed = baseMoveSpeed;
            //    return;
            //}

            foreach (var _buff in _buffs)
            {
                switch (_buff.buffType)
                {
                    case BuffType.Damage:
                        var _modifiedDamage = _buff.CalculateModifier(BoxDamageDealer.DamageInfor.damage);
                        BoxDamageDealer.DamageInfor.damage = _modifiedDamage;
                        break;
                    case BuffType.Health:
                        Debug.Log("Dont have health so skipping");
                        break;
                    case BuffType.CriticalChance:
                        var _modifiedCriticalChance = _buff.CalculateModifier(BoxDamageDealer.DamageInfor.criticalChance);
                        BoxDamageDealer.DamageInfor.criticalChance = _modifiedCriticalChance;
                        break;
                    case BuffType.CriticalDamage:
                        var _modifiedCriticalMultiplier = _buff.CalculateModifier(BoxDamageDealer.DamageInfor.criticalDamage);
                        BoxDamageDealer.DamageInfor.criticalDamage = _modifiedCriticalMultiplier;
                        break;
                    case BuffType.MoveSpeed:
                        var _modiedMoveSpeed = _buff.CalculateModifier(baseMoveSpeed);
                        ThirdPersonController.MoveSpeed = _modiedMoveSpeed;
                        break;
                }
            }
        }
        public virtual void AddBuff(Buff _buff)
        {
            BuffManager.AddBuff(_buff);
            CalculateBuffs();
        }
        public virtual void AddBuffs(List<Buff> _buffs)
        {
            BuffManager.AddBuffs(_buffs);
            CalculateBuffs();
        }
        public virtual void RemoveBuff(Buff _buff)
        {
            BuffManager.RemoveBuff(_buff);
            CalculateBuffs();
        }
        public virtual void RemoveBuffs(List<Buff> _buffs)
        {
            BuffManager.RemoveBuffs(_buffs);
            CalculateBuffs();
        }
        public virtual void ClearAllBuffs()
        {
            BuffManager.ClearAllBuffs();
            CalculateBuffs();
        }
        public void SetPlayerDamage(float damage)
        {
            playerDamage = Mathf.Max(0f, damage);
            CalculateBuffs();
        }
        public virtual List<Buff> GetActiveBuffs()
        {
            return BuffManager.GetActiveBuffs();
        }
        public virtual void SetCameraLookAt(Transform _target)
        {
            ThirdPersonController.SetCameraLookAt(_target);
        }

        public void SetGameplayActivity(bool active)
        {
            gameplayActive = active;

            if (StarterAssetsInputs != null)
            {
                StarterAssetsInputs.MoveInput(Vector2.zero);
                StarterAssetsInputs.LookInput(Vector2.zero);
                StarterAssetsInputs.JumpInput(false);
                StarterAssetsInputs.SprintInput(false);
                StarterAssetsInputs.AttackInput(false);
                StarterAssetsInputs.enabled = active;
            }

            if (ThirdPersonController != null)
            {
                if (!active)
                    ThirdPersonController.StopImmediately();

                ThirdPersonController.enabled = active;
            }

            var tactixMovePlayer = GetComponent<TactixMovePlayer>();
            if (tactixMovePlayer != null)
            {
                if (!active)
                    tactixMovePlayer.StopMoving();

                tactixMovePlayer.enabled = active;
            }

            if (BoxDamageDealer is Behaviour damageDealerBehaviour)
                damageDealerBehaviour.enabled = active;

#if ENABLE_INPUT_SYSTEM
            var playerInput = GetComponent<PlayerInput>();
            if (playerInput != null)
                playerInput.enabled = active;
#endif
        }

        #endregion
    }
}
