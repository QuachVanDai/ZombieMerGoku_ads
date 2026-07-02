using ExampleProject.GameSystem;
using ExampleProject.Manager;
using ExampleProject.Tools;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace ExampleProject.Gameplay.Scenes
{
    public class GameInitiator : MonoBehaviour
    {
        #region Fields

        [SerializeField] KeepObject keepObjectPrefab;
        [Space]
        [SerializeField] GameManager gameManagerPrefab;
        [SerializeField] UIManager uiManagerPrefab;

        [Space]
        [SerializeField] MusicSystem musicSystemPrefab;
        [SerializeField] SoundSystem soundSystemPrefab;



        [Space]
        [SerializeField] EventSystem eventSystemPrefab;
        [SerializeField] Camera uiCameraPrefab;
        [SerializeField] SharedCanvas sharedCanvasPrefab;



        [SerializeField] KeepObject keepObject;
        [SerializeField] GameManager gameManager;
        [SerializeField] UIManager uiManager;

        [SerializeField] MusicSystem musicSystem;
        [SerializeField] SoundSystem soundSystem;

        [SerializeField] EventSystem eventSystem;
        [SerializeField] Camera uiCamera;
        [SerializeField] SharedCanvas sharedCanvas;

        bool hasSpawnedObjects;
        bool hasInitializedObjects;
        bool hasPreparedGame;



        #endregion

        #region Properties



        #endregion

        #region LifeCycle   

        // Everything start from here
        void Start()
        {

            ResolveSceneObjects();
            //   SpawnObjects();
            InitSpawnedObjects();
            CreateObjects();
            PrepareGame();
        }

        #endregion

        #region Private Methods



        public void SpawnObjects()
        {
            ResolveSceneObjects();

            if (hasSpawnedObjects)
                return;

            SpawnKeepObject();
            SpawnManagers();
            SpawnSystems();
            SpawnPersistentSceneObjects();
            hasSpawnedObjects = HasRequiredObjects();
        }

        public void ResolveSceneObjects()
        {
            hasSpawnedObjects = HasRequiredObjects();
        }

        /*
        Camera FindUICamera()
        {
            if (uiCamera != null)
                return uiCamera;

            var cameras = FindObjectsOfType<Camera>();
            foreach (var camera in cameras)
            {
                if (camera.name.Contains("UI"))
                    return camera;
            }

            return null;
        }
        */

        bool HasRequiredObjects()
        {
            return keepObject != null
                   && gameManager != null
                   && uiManager != null
                   && musicSystem != null
                   && soundSystem != null

                   && eventSystem != null
                   && uiCamera != null
                   && sharedCanvas != null;
        }

        void SpawnKeepObject()
        {
            if (keepObject != null)
                return;

            if (keepObjectPrefab == null)
            {
                Debug.LogError("GameInitiator cannot spawn KeepObject: keepObjectPrefab is missing.");
                return;
            }

            keepObject = Instantiate(keepObjectPrefab);
        }

        void SpawnManagers()
        {
            if (keepObject == null)
                return;

            if (gameManager == null)
                gameManager = Instantiate(gameManagerPrefab, keepObject.ManagerPlacement);
            if (uiManager == null)
                uiManager = Instantiate(uiManagerPrefab, keepObject.ManagerPlacement);

        }

        void SpawnSystems()
        {
            if (keepObject == null)
                return;

            if (eventSystem == null)
                eventSystem = Instantiate(eventSystemPrefab, keepObject.SystemPlacement);
            if (musicSystem == null)
                musicSystem = Instantiate(musicSystemPrefab, keepObject.SystemPlacement);
            if (soundSystem == null)
                soundSystem = Instantiate(soundSystemPrefab, keepObject.SystemPlacement);

        }

        void SpawnPersistentSceneObjects()
        {
            if (keepObject == null)
                return;

            if (uiCamera == null)
                uiCamera = Instantiate(uiCameraPrefab, keepObject.transform);
            if (sharedCanvas == null)
                sharedCanvas = Instantiate(sharedCanvasPrefab, keepObject.transform);

        }

        public void InitSpawnedObjects()
        {
            if (hasInitializedObjects)
                return;

            if (!hasSpawnedObjects)
                SpawnObjects();

            if (!HasRequiredObjects())
            {
                Debug.LogError("GameInitiator cannot init: required objects are missing. Click 'Spawn Required Objects' and check prefab references.");
                return;
            }

            sharedCanvas.AsignCamera(uiCamera);
            uiManager.Init(uiCamera, sharedCanvas);
            hasInitializedObjects = true;
        }
        void CreateObjects()
        {
        }
        public void PrepareGame()
        {
            if (hasPreparedGame)
                return;

            if (!hasInitializedObjects)
                InitSpawnedObjects();

            gameManager.State = GameState.PlayingScene;
            hasPreparedGame = true;
        }

        #endregion

        #region Public Methods



        #endregion
    }
}
