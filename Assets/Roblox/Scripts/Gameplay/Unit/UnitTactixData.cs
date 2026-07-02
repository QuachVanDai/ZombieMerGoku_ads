using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using ExampleProject.Gameplay.Unit;
using UnityEngine;

namespace ExampleProject
{
    [CreateAssetMenu(fileName = "TactixUnitData", menuName = "ScriptableObjects/Tactix/UnitData")]
    public class UnitTactixData : ScriptableObject
    {
        public UnitId unitID;
        public float minHealthRatio = 0.9f;   
        public float maxHealthRatio = 1.1f;
        public float minDamageRatio = 0.9f;
        public float maxDamageRatio = 1.1f;
        public float SellingPriceX = 0.5f;
    }
}
