using System.Collections;
using System.Collections.Generic;
using ExampleProject.Tools;
using UnityEngine;

namespace ExampleProject
{
    public class TactixEnvironmentManager : Singleton<TactixEnvironmentManager>
    {
        #region Fields

    
        [SerializeField] List<Transform> spawnTowerPoints;
        [SerializeField] Transform spawnSlotPoint, spawnCooldownPoint, spawnPlayerPoint, legendaryChestSpawnPoint;
        [SerializeField] GameObject  limitAreaObject, buyItFloatingCanvas;
        [SerializeField] private Transform conveyorBelt, weaponTutorialTransform, zombieTutorialTransform;
      
        #endregion

        #region Properties
        
        public List<Transform> SpawnTowerPoints => spawnTowerPoints;
        public Transform SpawnSlotPoint => spawnSlotPoint;
        public Transform ConveyorBelt => conveyorBelt;
        public Transform WeaponTutorialTransform => weaponTutorialTransform;
        public Transform ZombieTutorialTransform => zombieTutorialTransform;
    
        public Transform SpawnCooldownPoint => spawnCooldownPoint;
        
        public Transform SpawnPlayerPoint => spawnPlayerPoint;  
        public Transform LegendaryChestSpawnPoint => legendaryChestSpawnPoint;

        public GameObject LimitAreaObject => limitAreaObject;
        
        public GameObject BuyItFloatingCanvas => buyItFloatingCanvas;
        
        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods


        #endregion
    }
}
