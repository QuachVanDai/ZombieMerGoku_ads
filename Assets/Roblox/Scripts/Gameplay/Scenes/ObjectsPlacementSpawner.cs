using UnityEngine;

namespace ExampleProject.Gameplay.Scenes
{
    public class ObjectsPlacementSpawner : MonoBehaviour
    {
        #region Fields

        [SerializeField] GameObject spaceLinePrefab;

        #endregion

        const string ManagerPlacementName = "Managers";
        const string SetupPlacementName = "Setup";
        const string EnvironmentPlacementName = "Enviroment";
        const string GameplayPlacementName = "Gameplay";
        const string DynamicPlacementName = "Dynamic";

        #region Properties



        #endregion

        #region LifeCycle

        public Transform ManagerPlacement;
        public Transform SetupPlacement ;
        public Transform EnviromentPlacement ;
        public Transform GameplayPlacement ;
        public Transform DynamicPlacement ;
        public bool HasPlacements =>
            ManagerPlacement != null &&
            SetupPlacement != null &&
            EnviromentPlacement != null &&
            GameplayPlacement != null &&
            DynamicPlacement != null;


        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public void Spawn()
        {
            ResolvePlacements();
            if (HasPlacements)
                return;

            SpawnSeparator();
            ManagerPlacement = ResolveOrCreatePlacement(ManagerPlacementName);

            SpawnSeparator();
            SetupPlacement = ResolveOrCreatePlacement(SetupPlacementName);

            SpawnSeparator();
            EnviromentPlacement = ResolveOrCreatePlacement(EnvironmentPlacementName);

            SpawnSeparator();
            GameplayPlacement = ResolveOrCreatePlacement(GameplayPlacementName);

            SpawnSeparator();
            DynamicPlacement = ResolveOrCreatePlacement(DynamicPlacementName);

            SpawnSeparator();
        }

        public void ResolvePlacements()
        {
            if (ManagerPlacement == null)
                ManagerPlacement = FindPlacement(ManagerPlacementName);
            if (SetupPlacement == null)
                SetupPlacement = FindPlacement(SetupPlacementName);
            if (EnviromentPlacement == null)
                EnviromentPlacement = FindPlacement(EnvironmentPlacementName);
            if (GameplayPlacement == null)
                GameplayPlacement = FindPlacement(GameplayPlacementName);
            if (DynamicPlacement == null)
                DynamicPlacement = FindPlacement(DynamicPlacementName);
        }

        public void ClearPlacements()
        {
            DestroyObject(ManagerPlacement == null ? null : ManagerPlacement.gameObject);
            DestroyObject(SetupPlacement == null ? null : SetupPlacement.gameObject);
            DestroyObject(EnviromentPlacement == null ? null : EnviromentPlacement.gameObject);
            DestroyObject(GameplayPlacement == null ? null : GameplayPlacement.gameObject);
            DestroyObject(DynamicPlacement == null ? null : DynamicPlacement.gameObject);

            ManagerPlacement = null;
            SetupPlacement = null;
            EnviromentPlacement = null;
            GameplayPlacement = null;
            DynamicPlacement = null;

            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                var child = transform.GetChild(i);
                DestroyObject(child.gameObject);
            }
        }

        Transform ResolveOrCreatePlacement(string placementName)
        {
            var placement = FindPlacement(placementName);
            if (placement != null)
                return placement;

            var gameObject = new GameObject(placementName);
            gameObject.transform.SetParent(transform, false);
            return gameObject.transform;
        }

        Transform FindPlacement(string placementName)
        {
            var child = transform.Find(placementName);
            if (child != null)
                return child;

            var existing = GameObject.Find(placementName);
            if (existing == null)
                return null;

            existing.transform.SetParent(transform, true);
            return existing.transform;
        }

        void SpawnSeparator()
        {
            if (spaceLinePrefab == null)
                return;

            Instantiate(spaceLinePrefab, transform);
        }

        void DestroyObject(GameObject target)
        {
            if (target == null)
                return;

            if (Application.isPlaying)
                Destroy(target);
            else
                DestroyImmediate(target);
        }

        #endregion
    }
}
