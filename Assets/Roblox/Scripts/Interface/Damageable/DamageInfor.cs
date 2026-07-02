using BreakInfinity;
using ExampleProject.Gameplay.Faction;
using ExampleProject.Tools;
using UnityEngine;

namespace ExampleProject.Interface
{
    public enum DamageType
    {
        None,
        Physical,
        Magic,
        Fire,
        Ice,
        Lightning,
        Poison,
        True // Ignores armor/resistance
    }

    public class DamageInfor
    {
        public BigDouble damage;
        public float criticalChance;
        public float criticalDamage;

        public DamageType damageType;
        public FactionId faction;
        public GameObject source; // Who dealt the damage
        public object additionalData; // For special effects, status data, etc.

        public bool IsCritical { get; private set; }

        // Constructor for simple damage
        public DamageInfor(BigDouble _damage, FactionId _faction, GameObject _source = null)
        {
            this.damage = _damage;
            this.faction = _faction;
            this.source = _source;
        }
        public void CalculateCritical()
        {
           IsCritical = Helpers.RandomByWeight(new float[] { 100 - criticalChance, criticalChance }) == 1;
        }
    }
}