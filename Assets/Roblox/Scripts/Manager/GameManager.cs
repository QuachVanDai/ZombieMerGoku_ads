using System.Collections;
using ExampleProject.Tools;
using UnityEngine;
using ExampleProject.Gameplay.Scenes;
namespace ExampleProject.Manager
{
    [RequireComponent(typeof(ObjectsPlacementSpawner))]
    public class GameManager : Singleton<GameManager>
    {
        #region Fields
        GameState state;
        [SerializeField] GameplayInitiator gameplayInitiatorPrefab;
        ObjectsPlacementSpawner objectsPlacementSpawner;
        GameplayInitiator gameplayInitiator;
        Coroutine gameplayInitCoroutine;
        bool hasInitializedGameplay;
        #endregion
        #region Properties
        public GameState State
        {
            get => state;
            set
            {
                if (state == value)
                    return;
                state = value;
                ChangeState(value);
            }
        }
        public ObjectsPlacementSpawner ObjectsPlacementSpawner
        {
            get
            {
                if (objectsPlacementSpawner == null)
                    objectsPlacementSpawner = GetComponent<ObjectsPlacementSpawner>();
                return objectsPlacementSpawner;
            }
        }
        EventDispatcher EventDispatcher => EventDispatcher.Instance;
        #endregion
        #region LifeCycle   
        #endregion
        #region Private Methods
        void ChangeState(GameState _state)
        {
            EventDispatcher.Dispatch(EventName.OnBeforeGameStateChange, state);
            switch (_state)
            {
                case GameState.PlayingScene:
                    HandlePlayingScene();
                    break;
            }
            EventDispatcher.Dispatch(EventName.OnAfterGameStateChange, state);
        }
        void HandlePlayingScene()
        {
            ObjectsPlacementSpawner.Spawn();
            ResolveGameplayInitiator();
            if (gameplayInitiator == null)
            {
                Debug.LogError("GameManager cannot start PlayingScene: GameplayInitiator is missing and prefab is not assigned.");
                return;
            }
            if (hasInitializedGameplay || gameplayInitCoroutine != null)
                return;
          InitGameplay();
        }
        void InitGameplay()
        {
                gameplayInitiator.Init();
            hasInitializedGameplay = true;
            gameplayInitCoroutine = null;
        }
        void ResolveGameplayInitiator()
        {
            if (gameplayInitiator != null)
                return;
            hasInitializedGameplay = false;
            gameplayInitiator = FindObjectOfType<GameplayInitiator>();
            if (gameplayInitiator != null)
                return;
            if (gameplayInitiatorPrefab != null)
                gameplayInitiator = Instantiate(gameplayInitiatorPrefab);
        }
        public void Spawn()
        {
            ObjectsPlacementSpawner.Spawn();
            ResolveGameplayInitiator();
        }
        #endregion
        #region Protected Methods
        #endregion
        #region Public Methods
        #endregion
    }
    public enum GameState
    {
        None,
        PlayingScene,
    }
}
