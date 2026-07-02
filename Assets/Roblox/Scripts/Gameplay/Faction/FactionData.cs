using UnityEngine;
using UnityEngine.Serialization;
using ExampleProject.Tools;

namespace ExampleProject.Gameplay.Faction
{
    [CreateAssetMenu(fileName = "FactionData", menuName = "ScriptableObjects/Faction/FactionData")]
    public class FactionData : ScriptableObject
    {
        #region Fields

        public FactionId id;
        public Gradient healthColor;

        #endregion

        #region Properties



        #endregion
    }

    public enum FactionId
    {
        None = 0,
        Grunt = 1,
        Zombie = 2,
    }
}