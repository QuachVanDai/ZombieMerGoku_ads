using ExampleProject.UI.Shared;
using UnityEngine;

namespace ExampleProject.Gameplay.Rarity
{
    [CreateAssetMenu(fileName = "RarityData", menuName = "ScriptableObjects/Rarity/RarityData")]
    public class RarityData : ScriptableObject
    {
        #region Fields

        public RarityId rarity;
        public float darkMultiplier = 0.8f;
        public float lightMultiplier = 1.5f;
        
        public ColorPack colorPack;
        public Gradient gradient2;

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