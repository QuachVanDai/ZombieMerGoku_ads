using ExampleProject.Gameplay.Rarity;
using ExampleProject.Manager;
using ExampleProject.UI.BaseUI.BasePopup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VTLTools.Effect;

namespace ExampleProject.Gameplay.Buffs
{
    [CreateAssetMenu(fileName = "BuffData", menuName = "ScriptableObjects/Buff/BuffData")]
    public class BuffData : ScriptableObject
    {
        #region Fields

        public BuffType type;
        public Sprite icon;

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
    public enum BuffType
    {
        None,
        Damage,
        Health,
        CriticalChance,
        CriticalDamage,
        AttackRange,
        MoveSpeed,
    }

    public enum ModifierType
    {
        None,
        Flat,           // Adds/subtracts a flat value (e.g., +10 damage)
        Percentage,     // Adds/subtracts a percentage (e.g., +20% damage)
        Multiplier      // Multiplies the base value (e.g., x1.5 damage)
    }
}
