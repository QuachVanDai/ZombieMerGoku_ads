using ExampleProject.Gameplay.Buffs;
using ExampleProject.Gameplay.Rarity;
using ExampleProject.Gameplay.Weapon;
using ExampleProject.UI.Shared;
using System.Collections.Generic;
using UnityEngine;

namespace ExampleProject.Gameplay.Wing
{
    [CreateAssetMenu(fileName = "WingData", menuName = "ScriptableObjects/Wing/WingData")]
    public class WingData : ScriptableObject
    {
        #region Fields

        public WingId id;
        public Sprite icon;
        public Wing prefab;
        public string localizedDescription;
        public string localizedName; public RarityId rarity;
        public UnlockCondition unlockCondition = new UnlockCondition();
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
