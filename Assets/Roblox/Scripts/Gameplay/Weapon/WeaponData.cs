using ExampleProject.Gameplay.Buffs;
using ExampleProject.Gameplay.Rarity;
using ExampleProject.Gameplay.Wing;
using ExampleProject.UI.Shared;
using System.Collections.Generic;
using UnityEngine;

namespace ExampleProject.Gameplay.Weapon
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/Weapon/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        #region Fields

        public WeaponId id;
        public Sprite icon;
        public Weapon prefab;
        public string localizedName;
        public string localizedDescription;
        public UnlockCondition unlockCondition = new UnlockCondition();        public RarityId rarity;
        public List<Buff> buffs;

        #endregion

        #region Properties



        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods



        #endregion
    }
}
