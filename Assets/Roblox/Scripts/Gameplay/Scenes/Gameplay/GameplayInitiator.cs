using ExampleProject.Gameplay.Characters;
using ExampleProject.Gameplay.GameplayCamera;
using ExampleProject.GameSystem;
using ExampleProject.Manager;
using ExampleProject.Tools;

using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace ExampleProject.Gameplay.Scenes
{
    public class GameplayInitiator : MonoBehaviour
    {
        #region Fields

        [SerializeField] GameplayController gameplayControllerPrefab;
        [SerializeField] Camera mainCameraPrefab;
        [SerializeField] TactixEnvironmentManager enviromentPrefab;
        [SerializeField] PlayablePlayer playerPrefab;

        [Header("Scene Instances")]
        [SerializeField] TactixEnvironmentManager sceneEnvironment;
        [SerializeField] PlayablePlayer scenePlayer;


        [SerializeField] GameplayController gameplayController;
        [SerializeField] Camera mainCamera;
        [SerializeField] TactixEnvironmentManager enviroment;
        public PlayablePlayer player;


        #endregion

        #region Properties

        [SerializeField] ObjectsPlacementSpawner placement;

        #endregion

        #region LifeCycle   

        public void Init()
        {
            ResolveRuntimeObjects();
            InitSpawnedObjects();
            CreateObjects();
            //PrepareGame();
        }

        #endregion

        #region Private Methods







        void SpawnPlayerObjects()
        {


            player = scenePlayer != null ? scenePlayer : FindObjectOfType<PlayablePlayer>();
            if (player == null && playerPrefab != null)
                player = Instantiate(playerPrefab);

            if (player == null)
                Debug.LogError("GameplayInitiator cannot spawn player: playerPrefab is missing and no PlayablePlayer exists in scene.");


        }

        public void InitSpawnedObjects()
        {
            if (!ResolveRuntimeObjects())
                return;

            if (placement != null)
            {
                placement.Spawn();
                if (placement.GameplayPlacement != null)
                    player.SetParent(placement.GameplayPlacement);
            }

            // Initialize Gameplay Controller
            gameplayController.SetPlayer(player);
            player.SetCamera(mainCamera);
            player.Init();
            SetupCameraFollow();
            gameplayController.Init();
        }
        void CreateObjects()
        {

        }
        void SetupCameraFollow()
        {
            if (mainCamera == null || player == null)
                return;

            var cameraFollow = mainCamera.GetComponent<PlayerCameraFollow>();
            if (cameraFollow == null)
                cameraFollow = mainCamera.gameObject.AddComponent<PlayerCameraFollow>();

            cameraFollow.SetPlayer(player);
            cameraFollow.SetFollowActive(true, true);
        }
        void PrepareGame()
        {
            if (!ResolveRuntimeObjects())
                return;
        }

        bool ResolveRuntimeObjects()
        {
            if (placement == null)
                placement = FindObjectOfType<ObjectsPlacementSpawner>();

            if (placement != null)
                placement.Spawn();

            if (gameplayController == null)
                gameplayController = GameplayController.Instance != null ? GameplayController.Instance : FindObjectOfType<GameplayController>();
            if (gameplayController == null && gameplayControllerPrefab != null)
                gameplayController = Instantiate(gameplayControllerPrefab, placement != null ? placement.ManagerPlacement : null);

            if (mainCamera == null)
                mainCamera = Camera.main != null ? Camera.main : FindObjectOfType<Camera>();
            if (mainCamera == null && mainCameraPrefab != null)
                mainCamera = Instantiate(mainCameraPrefab, placement != null ? placement.SetupPlacement : null);


            if (enviroment == null)
                enviroment = sceneEnvironment != null ? sceneEnvironment : TactixEnvironmentManager.Instance;
            if (enviroment == null)
                enviroment = FindObjectOfType<TactixEnvironmentManager>();
            if (enviroment == null && enviromentPrefab != null)
                enviroment = Instantiate(enviromentPrefab, placement != null ? placement.EnviromentPlacement : null);

            if (player == null)
                SpawnPlayerObjects();

            if (player == null)
            {
                Debug.LogError("GameplayInitiator init failed: PlayablePlayer is missing. Assign scenePlayer, place a PlayablePlayer in scene, or assign playerPrefab.");
                return false;
            }

            if (mainCamera == null)
            {
                Debug.LogError("GameplayInitiator init failed: mainCamera is missing. Assign mainCamera, tag a camera as MainCamera, or assign mainCameraPrefab.");
                return false;
            }

            if (gameplayController == null)
            {
                Debug.LogError("GameplayInitiator init failed: gameplayController is missing. Assign gameplayController or gameplayControllerPrefab.");
                return false;
            }
            if (enviroment == null)
            {
                Debug.LogError("GameplayInitiator init failed: environment is missing. Assign sceneEnvironment, place TactixEnvironmentManager in scene, or assign enviromentPrefab.");
                return false;
            }

            return true;
        }

        #endregion

        #region Public Methods



        #endregion
    }
}
