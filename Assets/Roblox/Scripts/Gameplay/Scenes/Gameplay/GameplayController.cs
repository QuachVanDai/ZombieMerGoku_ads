using ExampleProject.Gameplay.Characters;
using ExampleProject.Gameplay.Unit;
using ExampleProject.Manager;
using ExampleProject.Tools;
using ExampleProject.UI.BaseUI.BasePopup;
using ExampleProject.UI.Input;
using System;
using System.Collections;
using UnityEngine;
using Units = ExampleProject.Gameplay.Unit.Units;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Linq;
using ExampleProject.Gameplay.Level;
using ExampleProject.Gameplay.Faction;
using BreakInfinity;
using ExampleProject.Gameplay.Currency;
using ExampleProject.Gameplay.Rarity;
using ExampleProject.GameSystem;
using ExampleProject.Interface;
using ExampleProject.Gameplay.GameplayCamera;

using ExampleProject.UI.Playable;
using ExampleProject.AI;
using StarterAssets;
namespace ExampleProject.Gameplay.Scenes
{
    // static class GameplayControllerMutedDebug
    // {
    //     public static void Log(object message) { }
    //     public static void LogWarning(object message) { }
    //     public static void LogError(object message) { }
    // }

    public class GameplayController : Singleton<GameplayController>
    {
        #region Fields
        [LunaPlaygroundField("IsShowPlayer", 1, "Game Settings")]
        public bool isShowPlayer = true;
        [SerializeField] GameplayState gameplayState;
        [SerializeField] int gruntUnitSlotRow = 4;
        [SerializeField] int gruntUnitSlotColumn = 4;
        [SerializeField] float gruntUnitSlotSpacingX = 3;
        [SerializeField] float gruntUnitSlotSpacingY = 3;
        [SerializeField] SpawnInGrid zombieSpawnInGridPrefab;
        [SerializeField] TactixSlot slotPrefab;
        [SerializeField] List<TactixSlot> gruntSlots = new List<TactixSlot>();
        [SerializeField] SpawnInGrid gruntSpawnInGrid;
        [SerializeField] List<SpawnInGrid> zombieSpawnInGrids;
        [SerializeField] List<Tower> towers;
        [SerializeField] List<CreepUnit> gruntCreepUnit;
        [SerializeField] List<CreepUnit> zombieCreepUnit;
        [SerializeField] int phase;
        [SerializeField] int wave;

        [SerializeField] Transform movePreviewUnitObjectParent;
        [SerializeField] bool isTopDown;
        [SerializeField] AudioClip spawnWaveAudio;
        [SerializeField] AudioClip winLevelAudio;
        [SerializeField] AudioClip soundBossAppear;
        [SerializeField] AudioClip spawnOneUnitAudio;
        [SerializeField] AudioSource audioSource;
        [SerializeField] List<TactixRarityData> rarityDataList;
        [SerializeField] TactixMovePlayer movePlayer;
        [SerializeField] private bool isShowGetUnit = false;
        [SerializeField] private GameObject effectMergePrefab;
        [SerializeField] private bool spawnMergeEffect;
        private LoopingSoundHandle _currentWaveLoopHandle;
        private Coroutine _waveReplayCoroutine;
        private Coroutine _nextWaveCoroutine;
        bool hasPreparedZombieWave;
        int preparedZombieLevel = -1;
        int preparedZombiePhase = -1;
        int preparedZombieWave = -1;
        [SerializeField] private float waveGapDelay = 0.8f;
        [SerializeField] private float waveSpawnDelay = 3f;
        [SerializeField] private float levelOneBossEndHealthPercent = 0.3333333f;
        [SerializeField] private bool levelOneBossEndCardTriggered;
        [Header("Playable Tower Flow")]
        [SerializeField] UnitId mainTowerUnitId = UnitId.Tower_4;
        [SerializeField] Transform mainTowerSpawnPoint;
        [SerializeField] int mainTowerSpawnPointIndex;
        [SerializeField] int bossPhaseIndex = 1;
        [SerializeField] int bossWaveIndex;
        private Dictionary<int, CreepUnit> slotToUnitMap = new Dictionary<int, CreepUnit>();
        [SerializeField] PlayableFadeImage playableFadeImage;
        [SerializeField] BaseTactixChest playableChest;
        [SerializeField] Camera chestCamera;
        [SerializeField] Camera armyIntroCamera;
        [SerializeField] Camera gameplayCamera;
        [SerializeField] PlayableCameraPathMove armyIntroCameraMove;
        [SerializeField] Transform playerArmyIntroPoint;
        [SerializeField] Transform preparedZombieSpawnPoint;
        [SerializeField] float armyIntroCameraMoveTimeout = 4f;
        [SerializeField] float inputStartFightThreshold = 0.1f;
        [SerializeField] float playerMoveStartFightDistance = 0.05f;
        [Header("Playable Optimization")]
        [SerializeField] bool optimizePlayableRuntime = true;
        [SerializeField] int maxPlayableGruntUnits = 5;
        [SerializeField] bool hideSlotsDuringFight = true;
        [SerializeField] bool pausePreparedZombieAnimators = true;
        [SerializeField] float deadUnitDestroyDelay = 1.2f;
        [Header("Zombie Target Marker")]
        [SerializeField] bool showPreparedZombieTargetMarkers = true;
        [SerializeField] Transform tutorial;
        Coroutine stateRoutine;
        bool hasStateCompletion;
        GameplayState stateCompletion;
        private TactixSlot currentNextSlot;
        [SerializeField] public Camera MainCam;
        #endregion
        #region Properties
        GameManager GameManager => GameManager.instance;
        UIManager UIManager => UIManager.instance;
        EventDispatcher EventDispatcher => EventDispatcher.Instance;
        public GameplayState GameplayState
        {
            get => gameplayState;
            set
            {
                RequestState(value);
            }
        }
        public PlayablePlayer Player { get; private set; }
        public Transform MovePreviewUnitObjectParent => movePreviewUnitObjectParent;
        public bool IsTopDown => isTopDown;
        public TactixMovePlayer MovePlayer => movePlayer;
        public int SlotCount => gruntUnitSlotRow * gruntUnitSlotColumn;
        public bool IsShowGetUnit
        {
            get => isShowGetUnit;
            set
            {
                isShowGetUnit = value;
            }
        }
        public TactixSlot CurrentNextSlot => currentNextSlot;
        public List<TactixSlot> GruntSlots => gruntSlots;
        public List<CreepUnit> GruntCreepUnit => gruntCreepUnit;
        public List<CreepUnit> ZombieCreepUnit => zombieCreepUnit;
        public List<Tower> Towers => towers;
        public SpawnInGrid GruntSpawnInGrid => gruntSpawnInGrid;
        public Camera GameplayCamera => gameplayCamera;
        public Dictionary<int, CreepUnit> SlotToUnitMap => slotToUnitMap;
        public int Level
        {
            get => GameData.CurrentLevel;
            set => GameData.CurrentLevel = value;
        }
        public int Phase => phase;
        #endregion
        #region LifeCycle
        private void OnEnable()
        {

        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            // ==================== CLEANUP TIMER ====================

            // ==================== CLEANUP WAVE SOUND + COROUTINE ====================
            if (_currentWaveLoopHandle != null)
            {
                _currentWaveLoopHandle.Kill();
                _currentWaveLoopHandle = null;
            }
            if (_waveReplayCoroutine != null)
            {
                StopCoroutine(_waveReplayCoroutine);
                _waveReplayCoroutine = null;
            }
            ClearPreparedZombieWave();
            StopNextWaveCoroutine();
            if (UIManager == null) return;
            if (UIManager.IsHasPopup(PopupId.Input, out BasePopup inputPopup))
                inputPopup.Hide();
            slotToUnitMap.Clear();
        }

        IEnumerator GetStateCoroutine(GameplayState state)
        {

            switch (state)
            {
                case GameplayState.Prepare:
                case GameplayState.PlayableSetup:
                    return HandlePrepare();
                // case GameplayState.ChestIntro:
                //     return HandleChestIntro();
                // case GameplayState.ChestOpen:
                //     return HandleChestOpen();
                case GameplayState.HeroReward:
                    return HandleHeroReward();
                case GameplayState.ArmyIntro:
                    return HandleArmyIntro();
                case GameplayState.Idle:
                    return HandleIdle();
                case GameplayState.Fight:
                    return HandleFight();
                case GameplayState.Endcard:
                    return HandleEndcard();
                default:
                    return null;
            }
        }


        #endregion
        #region Private Methods
        void RequestState(GameplayState nextState)
        {
            if (gameplayState == nextState && stateRoutine != null)
                return;

            GameplayState previousState = gameplayState;
            gameplayState = nextState;
            RunState(previousState, gameplayState);
        }

        void RunState(GameplayState previousState, GameplayState nextState)
        {
            if (stateRoutine != null)
                StopCoroutine(stateRoutine);

            ClearStateCompletion();
            stateRoutine = StartCoroutine(StateRoutine(previousState, nextState));
        }

        IEnumerator StateRoutine(GameplayState previousState, GameplayState nextState)
        {
            EventDispatcher.Dispatch(EventName.OnBeforeGameStateChange, null);


            IEnumerator stateCoroutine = GetStateCoroutine(nextState);
            if (stateCoroutine != null)
                yield return stateCoroutine;

            EventDispatcher.Dispatch(EventName.OnAfterGameStateChange, null);
            stateRoutine = null;

            if (hasStateCompletion)
            {
                GameplayState completedNextState = stateCompletion;
                ClearStateCompletion();
                RequestState(completedNextState);
            }
        }

        void FinishStateWith(GameplayState nextState)
        {
            hasStateCompletion = true;
            stateCompletion = nextState;
        }

        void ClearStateCompletion()
        {
            hasStateCompletion = false;
            stateCompletion = GameplayState.None;
        }


        // void ResolvePlayableChest()
        // {
        //     if (playableChest != null)
        //         return;

        //     var chestRewardPopup = UIManager.ChestRewardPopup;
        //     if (chestRewardPopup != null)
        //         playableChest = chestRewardPopup.Chest;
        // }

        void UseOnlyCamera(Camera activeCamera)
        {
            SetCameraActive(chestCamera, activeCamera == chestCamera);
            SetCameraActive(armyIntroCamera, activeCamera == armyIntroCamera);
            SetCameraActive(gameplayCamera, activeCamera == gameplayCamera);
            UpdateGameplayCameraFollow(activeCamera);
        }

        void SetCameraActive(Camera targetCamera, bool active)
        {
            if (targetCamera != null)
                targetCamera.gameObject.SetActive(active);
        }

        void UpdateGameplayCameraFollow(Camera activeCamera)
        {
            SetCameraFollowActive(chestCamera, false);
            SetCameraFollowActive(armyIntroCamera, false);

            if (gameplayCamera == null)
                return;

            PlayerCameraFollow follow = gameplayCamera.GetComponent<PlayerCameraFollow>();
            if (follow == null)
                follow = gameplayCamera.gameObject.AddComponent<PlayerCameraFollow>();

            if (Player != null)
                follow.SetPlayer(Player);

            follow.SetFollowActive(activeCamera == gameplayCamera, true);
        }

        void SetCameraFollowActive(Camera targetCamera, bool active)
        {
            if (targetCamera == null)
                return;

            PlayerCameraFollow follow = targetCamera.GetComponent<PlayerCameraFollow>();
            if (follow != null)
                follow.SetFollowActive(active, false);
        }

        void MovePlayerToArmyIntroPoint()
        {
            if (Player == null || playerArmyIntroPoint == null)
                return;

            Player.transform.position = playerArmyIntroPoint.position;
            Player.transform.rotation = playerArmyIntroPoint.rotation;
            Player.SetActive(isShowPlayer);
        }



        IEnumerator HandlePrepare()
        {
            UIManager.PlayableCtaPopup?.Show();
            levelOneBossEndCardTriggered = false;
            phase = 0;
            wave = 0;
            // ==================== CLEANUP WAVE SOUND ====================
            if (_currentWaveLoopHandle != null)
            {
                _currentWaveLoopHandle.Kill();
                _currentWaveLoopHandle = null;
            }
            if (_waveReplayCoroutine != null)
            {
                StopCoroutine(_waveReplayCoroutine);
                _waveReplayCoroutine = null;
            }

            FinishStateWith(GameplayState.HeroReward);

            yield break;
        }
        // IEnumerator HandleChestIntro()
        // {
        //     UseOnlyCamera(chestCamera);

        //     var chestRewardPopup = UIManager.ChestRewardPopup;
        //     if (playableChest == null && chestRewardPopup != null)
        //         playableChest = chestRewardPopup.Chest;

        //     chestRewardPopup?.ShowChest();
        //     yield break;
        // }
        // IEnumerator HandleChestOpen()
        // {
        //     Debug.Log("HandleChestOpen");
        //     ResolvePlayableChest();
        //     if (playableChest != null)
        //         playableChest.HandleChestOpenState();

        //     yield break;
        // }
        IEnumerator HandleHeroReward()
        {
            Debug.Log("HandleHeroReward");
            // yield return new WaitForSeconds(2f);
            // if (playableFadeImage != null)
            //     playableFadeImage.FadeBlack(0.4f);
            // yield return new WaitForSeconds(.4f);
            SpawnGrids();
            SetGruntSlotsVisible(true);
            SpawnMainTower();
            UIManager.GetTactixUnitPopup?.Hide();
            //    UIManager.ChestRewardPopup?.Hide();
            if (playableChest != null)
            {
                playableChest.HideChest();
                playableChest = null;
            }
            MovePlayerToArmyIntroPoint();
            SpawnGruntUnit();
            PreSpawnFirstZombieWave();
            yield return new WaitForSeconds(.5f);
            // FinishStateWith(GameplayState.ArmyIntro);
        }
        IEnumerator HandleArmyIntro()
        {
            HideTutorial();
            Player.SetActive(true);
            yield return new WaitForSeconds(0.4f);
            Debug.Log("HandleArmyIntro");
            SetGruntSlotsVisible(true);
            UseOnlyCamera(armyIntroCamera);
            if (playableFadeImage != null)
                playableFadeImage.FadeClear(0.5f);

            yield return PlayArmyIntroCameraMove();
            SetPreparedZombieTargetMarkers(true);
            yield return new WaitForSeconds(0.4f);
            FinishStateWith(GameplayState.Idle);
        }

        IEnumerator PlayArmyIntroCameraMove()
        {
            Debug.Log("PlayArmyIntroCameraMove");
            if (armyIntroCameraMove == null)
                yield break;

            bool completed = false;
            Coroutine moveRoutine = StartCoroutine(PlayArmyIntroCameraMoveRoutine(() => completed = true));
            float elapsed = 0f;
            float timeout = Mathf.Max(0.1f, armyIntroCameraMoveTimeout);

            while (!completed && elapsed < timeout)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (!completed)
            {
                if (moveRoutine != null)
                    StopCoroutine(moveRoutine);

                armyIntroCameraMove.Stop();
            }
        }

        IEnumerator PlayArmyIntroCameraMoveRoutine(Action onComplete)
        {
            yield return armyIntroCameraMove.Play();
            onComplete?.Invoke();
        }

        IEnumerator HandleIdle()
        {
            UseOnlyCamera(gameplayCamera);

            InputPopup inputPopup = UIManager.InputPopup;
            if (inputPopup != null)
            {
                inputPopup.SetIsDoAnimation(false);
                inputPopup.Show();
                inputPopup.ShowOnlyMoveJoystick(true);
                inputPopup.SetAlphaMoveJoystick(1f);

                if (Player != null)
                    inputPopup.SetStarterAssetsInputs(Player.StarterAssetsInputs);
            }

            Vector3 startPlayerPosition = Player != null ? Player.transform.position : Vector3.zero;
            if (Player != null && Player.StarterAssetsInputs != null)
                Player.StarterAssetsInputs.MoveInput(Vector2.zero);

            while (gameplayState == GameplayState.Idle)
            {
                if (IsFightStartInputDetected(inputPopup, startPlayerPosition))
                {
                    FinishStateWith(GameplayState.Fight);
                    yield break;
                }

                yield return null;
            }

            yield break;
        }

        bool IsFightStartInputDetected(InputPopup inputPopup, Vector3 startPlayerPosition)
        {
            float threshold = Mathf.Max(0f, inputStartFightThreshold);
            if (inputPopup != null && inputPopup.HasMoveInput(threshold))
                return true;

            if (Player == null)
                return false;

            StarterAssetsInputs starterInputs = Player.StarterAssetsInputs;
            if (starterInputs != null && starterInputs.move.sqrMagnitude > threshold * threshold)
                return true;

            float movedDistance = Vector3.Distance(Player.transform.position, startPlayerPosition);
            return movedDistance > Mathf.Max(0f, playerMoveStartFightDistance);
        }
        IEnumerator HandleFight()
        {
            UseOnlyCamera(gameplayCamera);
            SetGruntSlotsVisible(!hideSlotsDuringFight);
            StartPreparedZombieWaveOrSpawn(Level, phase, wave);
            yield break;
        }
        void SpawnGrids()
        {
            CleanupSpawnGrids();
            gruntSpawnInGrid = Instantiate(zombieSpawnInGridPrefab, transform);
            gruntSpawnInGrid.name = "GruntSpawnInGrid";
            gruntSpawnInGrid.SetPosition(TactixEnvironmentManager.Instance.SpawnSlotPoint.position);
            gruntSpawnInGrid.SetGridSize(gruntUnitSlotRow, gruntUnitSlotColumn);
            gruntSpawnInGrid.SetGridSpacing(gruntUnitSlotSpacingX, gruntUnitSlotSpacingY);
            foreach (var slot in gruntSlots) if (slot != null) Destroy(slot.gameObject);
            gruntSlots.Clear();
            for (int i = 0; i < gruntUnitSlotColumn * gruntUnitSlotRow; i++)
            {
                Vector3 pos = gruntSpawnInGrid.GetSpawnPosition(i);
                TactixSlot slotObj = Instantiate(slotPrefab, pos, Quaternion.identity, transform);
                slotObj.name = $"Grunt_Slot_{i}";
                slotObj.SlotIndex = i;
                gruntSlots.Add(slotObj);
            }
            // Reorder: từ trên xuống dưới, trái sang phải
            gruntSlots = gruntSlots
                .OrderByDescending(s => s.transform.position.z)
                .ThenBy(s => s.transform.position.x)
                .ToList();
        }
        void CleanupSpawnGrids()
        {
            foreach (var grid in zombieSpawnInGrids)
            {
                if (grid != null)
                    Destroy(grid.gameObject);
            }
            zombieSpawnInGrids.Clear();
            if (gruntSpawnInGrid != null)
            {
                Destroy(gruntSpawnInGrid.gameObject);
                gruntSpawnInGrid = null;
            }
        }

        void SpawnTower(int _level)
        {
            SpawnMainTower();
        }

        void SpawnMainTower()
        {
            foreach (var t in towers) if (t != null) Destroy(t.gameObject);
            towers.Clear();

            UnitData data = Units.GetTowerResourceData(mainTowerUnitId);
            if (data == null)
            {
                Debug.LogError($"SpawnMainTower failed: tower UnitData is null. mainTowerUnitId={mainTowerUnitId}");
                return;
            }

            if (data.unitPrefab == null)
            {
                Debug.LogError($"SpawnMainTower failed: tower prefab is null. mainTowerUnitId={mainTowerUnitId}");
                return;
            }

            Transform spawnPoint = GetMainTowerSpawnPoint();
            if (spawnPoint == null)
            {
                Debug.LogError("SpawnMainTower failed: main tower spawn point is missing.");
                return;
            }

            var tower = Instantiate(data.unitPrefab) as Tower;
            if (tower == null)
            {
                Debug.LogError($"SpawnMainTower failed: prefab is not Tower. prefab={data.unitPrefab.name}, mainTowerUnitId={mainTowerUnitId}");
                return;
            }

            tower.SetData(data.unitId);
            tower.SetPosition(spawnPoint.position);
            tower.SetFaction(FactionId.Zombie);
            tower.Init();
            tower.SetHealth(data.health);
            tower.AddBuffs(Levels.GetLevelTowerBuffs(Level));
            towers.Add(tower);
            tower.CanDamage(false);
            tower.OnDie += OnTowerDieListener;
        }

        Transform GetMainTowerSpawnPoint()
        {
            if (mainTowerSpawnPoint != null)
                return mainTowerSpawnPoint;

            if (TactixEnvironmentManager.Instance == null ||
                TactixEnvironmentManager.Instance.SpawnTowerPoints == null ||
                TactixEnvironmentManager.Instance.SpawnTowerPoints.Count == 0)
            {
                return null;
            }

            int index = Mathf.Clamp(mainTowerSpawnPointIndex, 0, TactixEnvironmentManager.Instance.SpawnTowerPoints.Count - 1);
            return TactixEnvironmentManager.Instance.SpawnTowerPoints[index];
        }

        Tower GetMainTower()
        {
            for (int i = 0; i < towers.Count; i++)
            {
                if (towers[i] != null)
                    return towers[i];
            }

            return null;
        }
        private void SpawnGruntUnit()
        {
            foreach (var g in gruntCreepUnit) if (g != null) Destroy(g.gameObject);
            gruntCreepUnit.Clear();
            slotToUnitMap.Clear();
            var formationProgress = FormationProgress.Progress;
            if (formationProgress.fieldUnits == null || formationProgress.fieldUnits.Count == 0)
                formationProgress.SetupDefaultFormation();

            var fieldUnits = formationProgress.fieldUnits;
            if (fieldUnits == null || fieldUnits.Count == 0)
            {
                Debug.LogWarning("Formation fieldUnits is empty, no Grunt units will be spawned.");
                return;
            }
            int spawned = 0;
            int max = gruntUnitSlotRow * gruntUnitSlotColumn;
            int spawnLimit = GetPlayableGruntSpawnLimit(fieldUnits.Count);
            foreach (var fieldUnitSetup in fieldUnits)
            {
                if (spawned >= spawnLimit)
                    break;

                if (SpawnSingleGruntUnit(fieldUnitSetup)) spawned++;
            }
        }

        int GetPlayableGruntSpawnLimit(int unitCount)
        {
            if (!optimizePlayableRuntime || maxPlayableGruntUnits <= 0)
                return unitCount;

            return Mathf.Min(unitCount, maxPlayableGruntUnits);
        }
        void SpawnZombieWave(int _level, int _phase, int _wave)
        {
            if (!TrySpawnZombieWaveObjects(_level, _phase, _wave, true, false, out LevelPhase phaseData))
                return;

            StartZombieWaveFight(_level, _phase, _wave, phaseData);
        }

        void PreSpawnFirstZombieWave()
        {
            if (hasPreparedZombieWave)
                return;

            if (TrySpawnZombieWaveObjects(Level, phase, wave, false, true, out _))
            {
                hasPreparedZombieWave = true;
                preparedZombieLevel = Level;
                preparedZombiePhase = phase;
                preparedZombieWave = wave;
                SetZombieBrainsEnabled(false);
                SetPreparedZombieAnimatorsEnabled(false);
            }
        }

        void StartPreparedZombieWaveOrSpawn(int _level, int _phase, int _wave)
        {
            if (hasPreparedZombieWave && preparedZombieLevel == _level && preparedZombiePhase == _phase && preparedZombieWave == _wave)
            {
                LevelPhase phaseData = Levels.GetPhase(_level, _phase);
                ClearPreparedZombieWave();
                SetPreparedZombieTargetMarkers(false);
                ResetPreparedZombiesForFight();
                StartZombieWaveFight(_level, _phase, _wave, phaseData);
                return;
            }

            SpawnZombieWave(_level, _phase, _wave);
        }

        void StartZombieWaveFight(int _level, int _phase, int _wave, LevelPhase phaseData)
        {
            Tower mainTower = GetMainTower();
            if (mainTower != null)
                mainTower.CanDamage(WaveContainsBoss(_level, _phase, _wave));

            if (_currentWaveLoopHandle != null)
            {
                _currentWaveLoopHandle.Kill();
                _currentWaveLoopHandle = null;
            }
            if (_waveReplayCoroutine != null)
            {
                StopCoroutine(_waveReplayCoroutine);
                _waveReplayCoroutine = null;
            }
            _currentWaveLoopHandle = SoundSystem.instance.PlayLoopingSound(spawnWaveAudio);
            _currentWaveLoopHandle.audioSource.loop = false;
            _waveReplayCoroutine = StartCoroutine(ReplayWaveWithGap(_currentWaveLoopHandle.audioSource, spawnWaveAudio));
        }

        bool TrySpawnZombieWaveObjects(int _level, int _phase, int _wave, bool allowRepairTower, bool usePreparedSpawnPoint, out LevelPhase phaseData)
        {
            phaseData = Levels.GetPhase(_level, _phase);
            if (phaseData == null)
            {
                Debug.LogError($"Phase {_phase} does not exist at level {_level} -> cannot spawn wave");
                return false;
            }
            if (_wave >= phaseData.waves.Count)
            {
                return false;
            }
            var waveData = Levels.GetWave(_level, _phase, _wave);
            if (waveData == null) return false;
            var waveInfos = Levels.GetWaveInfos(_level, _phase, _wave);
            if (waveInfos == null || waveInfos.Count == 0) return false;
            Tower targetTower = GetMainTower();
            if (targetTower == null || targetTower.IsDead)
            {
                if (allowRepairTower && TryRepairMainTower() && GetMainTower() != null && !GetMainTower().IsDead)
                {
                    targetTower = GetMainTower();
                }
                else
                {
                    Debug.LogError(
                        $"No valid main tower -> cannot spawn zombies. " +
                        $"level={_level}, wave={_wave}, towers.Count={towers.Count}, " +
                        $"phaseDataNull={phaseData == null}, waveDataNull={waveData == null}, waveInfosCount={(waveInfos == null ? -1 : waveInfos.Count)}, " +
                        $"mainTower={GetTowerDebugInfo(targetTower)}");
                    return false;
                }
            }
            Vector3 towerPos = targetTower.transform.position;
            float spawnOffsetZ = -5f;
            float spawnOffsetXRandom = 3f;
            bool playedBossAppearSound = false;
            foreach (var info in waveInfos)
            {
                if (!playedBossAppearSound && IsBossUnit(info.unitId))
                {
                    if (soundBossAppear != null && SoundSystem.instance != null)
                        SoundSystem.instance.PlaySoundOneShot(soundBossAppear);
                    playedBossAppearSound = true;
                }

                for (int j = 0; j < info.quantity; j++)
                {
                    var data = Units.GetUnitData(info.unitId);
                    if (data == null || data.unitPrefab == null) continue;
                    var zombie = Instantiate(data.unitPrefab) as CreepUnit;
                    zombie.SetData(info.unitId);
                    if (phase >= 3)
                        spawnOffsetZ = -7f;
                    Vector3 baseSpawnPos = GetZombieSpawnPosition(towerPos, spawnOffsetZ, spawnOffsetXRandom, usePreparedSpawnPoint);
                    zombie.SetPosition(baseSpawnPos);
                    FaceZombieToNearestGrunt(zombie);
                    zombie.SetFaction(FactionId.Zombie);
                    zombie.Init();
                    zombie.SetZombieTargetMarkerActive(false);
                    zombie.AddBuffs(Levels.GetWaveZombieBuffs(_level, _phase, _wave));
                    zombie.AddBuffs(Levels.GetLevelZombieBuffs(_level));
                    zombieCreepUnit.Add(zombie);
                    zombie.OnDie += OnZombieDieListener;
                    RegisterLevelOneBossEndCardCheck(zombie);
                }
            }
            return true;
        }

        Vector3 GetZombieSpawnPosition(Vector3 towerPos, float spawnOffsetZ, float spawnOffsetXRandom, bool usePreparedSpawnPoint)
        {
            if (usePreparedSpawnPoint && preparedZombieSpawnPoint != null)
            {
                Vector3 spawnPoint = preparedZombieSpawnPoint.position;
                return new Vector3(
                    spawnPoint.x + Random.Range(-spawnOffsetXRandom, spawnOffsetXRandom),
                    spawnPoint.y,
                    spawnPoint.z + Random.Range(-1f, 1f)
                );
            }

            return new Vector3(
                towerPos.x + Random.Range(-spawnOffsetXRandom, spawnOffsetXRandom),
                towerPos.y,
                towerPos.z + spawnOffsetZ + Random.Range(-1f, 0f)
            );
        }

        bool IsBossUnit(UnitId unitId)
        {
            UnitData unitData = Units.GetUnitData(unitId);
            if (unitData != null)
                return unitData.unitType == UnitType.Boss;

            return unitId == UnitId.Boss_1 ||
                   unitId == UnitId.Boss_2 ||
                   unitId == UnitId.Boss_3 ||
                   unitId == UnitId.Boss_4;
        }

        bool WaveContainsBoss(int _level, int _phase, int _wave)
        {
            var waveInfos = GetWaveInfosSafe(_level, _phase, _wave);
            if (waveInfos == null)
                return false;

            return waveInfos.Any(info => IsBossUnit(info.unitId));
        }

        List<WaveInfo> GetWaveInfosSafe(int _level, int _phase, int _wave)
        {
            var levelData = Levels.GetResourceData(_level);
            var phases = levelData?.GetLevelPhases();
            if (phases == null || _phase < 0 || _phase >= phases.Count)
                return null;

            var waves = phases[_phase]?.waves;
            if (waves == null || _wave < 0 || _wave >= waves.Count)
                return null;

            return waves[_wave]?.units;
        }

        bool TryGetBossWave(int _level, out int bossPhase, out int bossWave)
        {
            bossPhase = bossPhaseIndex;
            bossWave = bossWaveIndex;

            if (WaveContainsBoss(_level, bossPhase, bossWave))
                return true;

            var levelData = Levels.GetResourceData(_level);
            var phases = levelData?.GetLevelPhases();
            if (phases == null)
                return false;

            for (int i = 0; i < phases.Count; i++)
            {
                LevelPhase phaseData = phases[i];
                if (phaseData?.waves == null)
                    continue;

                for (int j = 0; j < phaseData.waves.Count; j++)
                {
                    if (!WaveContainsBoss(_level, i, j))
                        continue;

                    bossPhase = i;
                    bossWave = j;
                    return true;
                }
            }

            return false;
        }

        public void SetPreparedZombieTargetMarkers(bool isActive)
        {
            if (!showPreparedZombieTargetMarkers && isActive)
                return;

            foreach (CreepUnit zombie in zombieCreepUnit)
            {
                if (zombie == null || zombie.IsDead)
                    continue;

                bool shouldShow = isActive &&
                                  hasPreparedZombieWave &&
                                  zombie.Faction == FactionId.Zombie;

                zombie.SetZombieTargetMarkerActive(shouldShow);
            }
        }

        void ResetPreparedZombiesForFight()
        {
            foreach (CreepUnit zombie in zombieCreepUnit)
            {
                if (zombie == null || zombie.IsDead)
                    continue;

                zombie.ClearTarget();

                TranslateTargetMovable movable = zombie.GetComponent<TranslateTargetMovable>();
                if (movable != null)
                {
                    movable.enabled = true;
                    movable.StopMove();
                }

                LookAtTarget lookAtTarget = zombie.GetComponent<LookAtTarget>();
                if (lookAtTarget != null)
                    lookAtTarget.enabled = true;

                FaceZombieToNearestGrunt(zombie);
                zombie.SetAnimatorEnabled(true);
                zombie.PlayIdleAnim();
            }

            SetZombieBrainsEnabled(true);
        }

        void SetZombieBrainsEnabled(bool isEnabled)
        {
            foreach (CreepUnit zombie in zombieCreepUnit)
            {
                if (zombie == null)
                    continue;

                SimpleUnitBrain simpleUnitBrain = zombie.GetComponent<SimpleUnitBrain>();
                if (simpleUnitBrain != null)
                    simpleUnitBrain.enabled = isEnabled;
            }
        }

        void SetPreparedZombieAnimatorsEnabled(bool isEnabled)
        {
            if (!pausePreparedZombieAnimators)
                return;

            foreach (CreepUnit zombie in zombieCreepUnit)
            {
                if (zombie == null || zombie.IsDead)
                    continue;

                zombie.SetAnimatorEnabled(isEnabled);
            }
        }

        void SetGruntSlotsVisible(bool isVisible)
        {
            if (gruntSlots == null)
                return;


        }

        void FaceZombieToNearestGrunt(CreepUnit zombie)
        {
            if (zombie == null)
                return;

            CreepUnit target = FindNearestAliveGrunt(zombie.transform.position);
            if (target == null)
                return;

            Vector3 direction = target.transform.position - zombie.transform.position;
            direction.y = 0f;
            if (direction.sqrMagnitude <= 0.0001f)
                return;

            zombie.transform.rotation = Quaternion.LookRotation(direction);
        }

        CreepUnit FindNearestAliveGrunt(Vector3 position)
        {
            CreepUnit nearest = null;
            float nearestSqrDistance = float.MaxValue;

            foreach (CreepUnit grunt in gruntCreepUnit)
            {
                if (grunt == null || grunt.IsDead)
                    continue;

                float sqrDistance = (grunt.transform.position - position).sqrMagnitude;
                if (sqrDistance >= nearestSqrDistance)
                    continue;

                nearestSqrDistance = sqrDistance;
                nearest = grunt;
            }

            return nearest;
        }

        void ClearPreparedZombieWave()
        {
            hasPreparedZombieWave = false;
            preparedZombieLevel = -1;
            preparedZombiePhase = -1;
            preparedZombieWave = -1;
        }
        void ScheduleNextWave(int _level, int _phase, int _wave, LevelPhase _phaseData)
        {
            StopNextWaveCoroutine();
            int nextWave = _wave + 1;
            if (_phaseData == null || nextWave >= _phaseData.waves.Count)
            {
                return;
            }
            _nextWaveCoroutine = StartCoroutine(SpawnNextWaveAfterDelay(_level, _phase, nextWave));
        }
        IEnumerator SpawnNextWaveAfterDelay(int _level, int _phase, int _nextWave)
        {
            yield return new WaitForSeconds(waveSpawnDelay);
            _nextWaveCoroutine = null;
            if (gameplayState != GameplayState.Fight)
                yield break;
            if (_phase != phase)
                yield break;
            if (_phase < 0 || _phase >= towers.Count || towers[_phase] == null || towers[_phase].IsDead)
                yield break;
            wave = _nextWave;
            SpawnZombieWave(_level, _phase, wave);
        }
        void StopNextWaveCoroutine()
        {
            if (_nextWaveCoroutine == null)
                return;
            StopCoroutine(_nextWaveCoroutine);
            _nextWaveCoroutine = null;
        }

        void StopWaveLoopSound()
        {
            if (_currentWaveLoopHandle != null)
            {
                _currentWaveLoopHandle.Kill();
                _currentWaveLoopHandle = null;
            }

            if (_waveReplayCoroutine != null)
            {
                StopCoroutine(_waveReplayCoroutine);
                _waveReplayCoroutine = null;
            }
        }

        void RegisterLevelOneBossEndCardCheck(CreepUnit zombie)
        {
            if (zombie == null || Level != 1 || !IsBossUnit(zombie.UnitId))
                return;
            zombie.Damageable.OnTakeDamage += _ => CheckLevelOneBossHealth(zombie);
        }

        void CheckLevelOneBossHealth(CreepUnit boss)
        {
            if (levelOneBossEndCardTriggered || boss == null)
                return;
            var damageable = boss.Damageable;
            if (damageable == null || damageable.MaxHealth <= 0)
                return;
            BigDouble thresholdHealth = damageable.MaxHealth * (double)levelOneBossEndHealthPercent;
            if (damageable.CurrentHealth <= thresholdHealth)
                TriggerLevelOneBossEndCard(boss);
        }
        void TriggerLevelOneBossEndCard(CreepUnit boss)
        {
            if (levelOneBossEndCardTriggered)
                return;
            levelOneBossEndCardTriggered = true;
            ClearPreparedZombieWave();
            StopNextWaveCoroutine();
            StopWaveLoopSound();
            TurnOnOffAllAtkUnits(false);
            TurnOnOffAllAtkZombie(false);
            if (Player != null)
                Player.SetGameplayActivity(false);
            GameplayState = GameplayState.Endcard;
        }
        IEnumerator HandleEndcard()
        {

            TurnOnOffAllAtkUnits(false);
            TurnOnOffAllAtkZombie(false);
            StopNextWaveCoroutine();
            StopWaveLoopSound();
            if (Player != null)
                Player.SetGameplayActivity(false);
            foreach (var tower in towers)
            {
                if (tower != null)
                    tower.CanDamage(false);
            }

            UIManager.PlayableCtaPopup?.Hide();
            UIManager.InputPopup?.Hide();
            UIManager.PlayableEndcardPopup?.Show();
            /*if (Popups.GetResourceData(PopupId.TactixWinLose) != null)
                UIManager.GetPopup(PopupId.TactixWinLose)?.Show();*/
            Luna.Unity.LifeCycle.GameEnded();
            yield break;
        }

        bool TryRepairMainTower()
        {
            Tower tower = GetMainTower();
            if (tower == null || (tower.MaxHealth > 0 && !tower.IsDead))
                return false;

            UnitData unitData = Units.GetTowerResourceData(mainTowerUnitId);
            if (unitData == null || unitData.health <= 0)
                return false;

            tower.SetData(mainTowerUnitId);
            tower.SetFaction(FactionId.Zombie);
            tower.SetHealth((BigDouble)unitData.health);
            return tower.MaxHealth > 0 && !tower.IsDead;
        }

        bool TryRepairInvalidTower(int _level, int _phase)
        {
            if (_phase < 0 || _phase >= towers.Count)
                return false;
            var tower = towers[_phase];
            if (tower == null || (tower.MaxHealth > 0 && !tower.IsDead))
                return false;
            var phaseData = Levels.GetPhase(_level, _phase);
            if (phaseData == null)
            {
                Debug.LogError($"Cannot repair tower health: phase data is null. level={_level}, phase={_phase}, tower={GetTowerDebugInfo(tower)}");
                return false;
            }
            var unitData = Units.GetTowerResourceData(phaseData.towerUnitId);
            if (unitData == null || unitData.health <= 0)
            {
                Debug.LogError(
                    $"Cannot repair tower health: tower data is invalid. " +
                    $"level={_level}, phase={_phase}, towerUnitId={phaseData.towerUnitId}, " +
                    $"dataNull={unitData == null}, dataHealth={(unitData == null ? "null" : unitData.health.ToString())}, tower={GetTowerDebugInfo(tower)}");
                return false;
            }
            Debug.LogError(
                $"Repairing invalid tower before spawning zombies. " +
                $"level={_level}, phase={_phase}, towerUnitId={phaseData.towerUnitId}, dataHealth={unitData.health}, beforeRepair={GetTowerDebugInfo(tower)}");
            tower.SetData(phaseData.towerUnitId);
            tower.SetFaction(FactionId.Zombie);
            tower.SetHealth((BigDouble)unitData.health);
            return tower.MaxHealth > 0 && !tower.IsDead;
        }
        static string GetTowerDebugInfo(Tower tower)
        {
            if (tower == null)
                return "null=True";
            var damageable = tower.Damageable;
            var causeOfDeath = damageable?.CauseOfDeath;
            var deathSource = causeOfDeath?.source == null ? "null" : causeOfDeath.source.name;
            var deathDamage = causeOfDeath == null ? "null" : causeOfDeath.damage.ToString();
            var deathFaction = causeOfDeath == null ? "null" : causeOfDeath.faction.ToString();
            return
                $"null=False, name={tower.name}, unitId={tower.UnitId}, dead={tower.IsDead}, " +
                $"health={(damageable == null ? "null" : damageable.CurrentHealth.ToString())}, " +
                $"maxHealth={(damageable == null ? "null" : damageable.MaxHealth.ToString())}, " +
                $"deathSource={deathSource}, deathDamage={deathDamage}, deathFaction={deathFaction}, " +
                $"pos={tower.transform.position}";
        }
        // ==================== COROUTINE DELAY WAVE SOUND ====================
        private IEnumerator ReplayWaveWithGap(AudioSource source, AudioClip clip)
        {
            while (source != null && source.gameObject != null)
            {
                yield return new WaitForSeconds(clip.length + waveGapDelay);
                if (source != null)
                    source.Play();
            }
        }
        public int solancong;
        void OnZombieDieListener(BaseUnit _baseUnit)
        {
            CleanupDeadCreepAfterDelay(_baseUnit, zombieCreepUnit);
            var _id = _baseUnit.UnitId;
            FormationProgress.Progress.totalZombieKilled += 1;
            solancong += 1;
            bool allDead = zombieCreepUnit.All(z => z == null || z.IsDead);
            if (!allDead)
            {
                return;
            }
            if (_currentWaveLoopHandle != null)
            {
                _currentWaveLoopHandle.Kill();
                _currentWaveLoopHandle = null;
            }
            if (_waveReplayCoroutine != null)
            {
                StopCoroutine(_waveReplayCoroutine);
                _waveReplayCoroutine = null;
            }

            if (WaveContainsBoss(Level, phase, wave))
                return;

            if (!TryGetBossWave(Level, out int bossPhase, out int bossWave))
                return;

            phase = bossPhase;
            wave = bossWave;
            SpawnZombieWave(Level, phase, wave);
        }
        void OnTowerDieListener(BaseUnit _baseUnit)
        {
            CleanupDeadTowerAfterDelay(_baseUnit as Tower);
            StopNextWaveCoroutine();
        }
        void OnGruntDieListener(BaseUnit _baseUnit)
        {
            CleanupDeadCreepAfterDelay(_baseUnit, gruntCreepUnit);
        }

        void CleanupDeadCreepAfterDelay(BaseUnit unit, List<CreepUnit> sourceList)
        {
            CreepUnit creep = unit as CreepUnit;
            if (creep == null)
                return;

            StartCoroutine(CleanupDeadCreepRoutine(creep, sourceList));
        }

        IEnumerator CleanupDeadCreepRoutine(CreepUnit creep, List<CreepUnit> sourceList)
        {
            yield return new WaitForSeconds(Mathf.Max(0f, deadUnitDestroyDelay));

            if (creep == null)
                yield break;

            if (sourceList != null)
                sourceList.Remove(creep);

            if (creep.gameObject != null)
                Destroy(creep.gameObject);
        }

        void CleanupDeadTowerAfterDelay(Tower tower)
        {
            if (tower == null)
                return;

            int towerIndex = towers.IndexOf(tower);
            StartCoroutine(CleanupDeadTowerRoutine(tower, towerIndex));
        }

        IEnumerator CleanupDeadTowerRoutine(Tower tower, int towerIndex)
        {
            yield return new WaitForSeconds(Mathf.Max(0f, deadUnitDestroyDelay));

            if (tower == null)
                yield break;

            if (towerIndex >= 0 && towerIndex < towers.Count && towers[towerIndex] == tower)
                towers[towerIndex] = null;

            if (tower.gameObject != null)
                Destroy(tower.gameObject);
        }

        float GetScaleBasedOnLevel(int _level)
        {
            if (_level <= 1) return 1f;
            if (_level == 2) return 1.1f;
            if (_level == 3) return 1.2f;
            return 1.2f + (_level - 3) * 0.1f; // higher levels get slightly more scale increase
        }
        private void TestSetLook()
        {
            Player.SetCameraLookAt(
                GruntSlots[0].transform);
        }
        private void TestOnOffPlayer()
        {
            Player.SetActive(!Player.gameObject.activeSelf);
        }
        #endregion
        #region Public Methods
        public void TurnOnOffAllAtkUnits(bool _isOn)
        {
            foreach (var gruntUnit in gruntCreepUnit)
            {
                if (gruntUnit == null)
                    continue;
                var simpleUnitBrain = gruntUnit.GetComponent<SimpleUnitBrain>();
                if (simpleUnitBrain != null)
                    simpleUnitBrain.enabled = _isOn;
                if (!_isOn)
                {
                    gruntUnit.ClearTarget();
                    var movable = gruntUnit.GetComponent<TranslateTargetMovable>();
                    if (movable != null)
                        movable.StopMove();
                }
            }
        }
        public void TurnOnOffAllAtkZombie(bool _isOn)
        {
            foreach (var zombieUnit in zombieCreepUnit)
            {
                if (zombieUnit == null)
                    continue;
                var simpleUnitBrain = zombieUnit.GetComponent<SimpleUnitBrain>();
                if (simpleUnitBrain != null)
                    simpleUnitBrain.enabled = _isOn;
                if (!_isOn)
                {
                    zombieUnit.ClearTarget();
                    TranslateTargetMovable _movable = zombieUnit.GetComponent<TranslateTargetMovable>();
                    LookAtTarget lookAtTarget = zombieUnit.GetComponent<LookAtTarget>();
                    if (lookAtTarget != null)
                        lookAtTarget.enabled = false;
                    if (_movable != null)
                    {
                        _movable.SetSpeed(0);
                        _movable.StopMove();
                        _movable.enabled = false;
                    }
                }
            }
        }
        public void TurnOffMainTower()
        {
            Tower mainTower = GetMainTower();
            if (mainTower != null)
                mainTower.CanDamage(false);
        }

        public bool SpawnSingleGruntUnit(UnitSetupData gruntSetupData)
        {
            if (gruntSetupData == null) return false;
            int slotIndex = gruntSetupData.id;
            int maxSlots = gruntUnitSlotRow * gruntUnitSlotColumn;
            if (slotIndex < 0 || slotIndex >= maxSlots) return false;
            var data = Units.GetCreepResourceData(gruntSetupData.unitID);
            if (data == null || data.unitPrefab == null) return false;
            var spawnedGruntUnit = Instantiate(data.unitPrefab) as CreepUnit;
            if (spawnedGruntUnit == null) return false;
            spawnedGruntUnit.SetData(gruntSetupData.unitID);
            Vector3 pos = gruntSpawnInGrid.GetSpawnPosition(slotIndex);
            TactixSlot slot = gruntSlots.Find(s => s.SlotIndex == slotIndex);
            if (slot != null) slot.Unit = spawnedGruntUnit;
            spawnedGruntUnit.ApplyMultiplier(gruntSetupData.ratioHealth, gruntSetupData.ratioDamage);
            spawnedGruntUnit.SetPosition(pos);
            spawnedGruntUnit.SetFaction(FactionId.Grunt);
            spawnedGruntUnit.Init();
            spawnedGruntUnit.SetScale(GetScaleBasedOnLevel(gruntSetupData.level));
            spawnedGruntUnit.slot = slotIndex;
            var _cp = Units.GetCombatPoint(gruntSetupData.unitID, gruntSetupData.ratioDamage, gruntSetupData.ratioHealth);
            var _level = gruntSetupData.level;
            var tactixUnit = spawnedGruntUnit.GetComponent<TactixUnit>();
            if (tactixUnit != null)
            {
                tactixUnit.UpdateTextCP(_cp);
                tactixUnit.UpdateLevel(gruntSetupData.unitID + " " + Units.GetCreepResourceData(gruntSetupData.unitID).rarity, _level);
            }
            gruntCreepUnit.Add(spawnedGruntUnit);
            spawnedGruntUnit.OnDie += OnGruntDieListener;
            slotToUnitMap[slotIndex] = spawnedGruntUnit;
            return true;
        }
        public bool SpawnSingleGruntUnit2(UnitSetupData gruntSetupData, int slotIndex)
        {
            if (gruntSetupData == null) return false;
            var data = Units.GetCreepResourceData(gruntSetupData.unitID);
            var spawnedGruntUnit = Instantiate(data.unitPrefab) as CreepUnit;
            spawnedGruntUnit.SetData(gruntSetupData.unitID);
            Vector3 pos = gruntSpawnInGrid.GetSpawnPosition(slotIndex);
            TactixSlot slot = gruntSlots.Find(s => s.SlotIndex == slotIndex);
            if (slot != null) slot.Unit = spawnedGruntUnit;
            spawnedGruntUnit.ApplyMultiplier(gruntSetupData.ratioHealth, gruntSetupData.ratioDamage);
            spawnedGruntUnit.SetPosition(pos);
            spawnedGruntUnit.SetFaction(FactionId.Grunt);
            spawnedGruntUnit.Init();
            spawnedGruntUnit.SetHaloActive(true);
            spawnedGruntUnit.SetScale(GetScaleBasedOnLevel(gruntSetupData.level) * 1.2f);
            var _cp = Units.GetCombatPoint(gruntSetupData.unitID, gruntSetupData.ratioDamage, gruntSetupData.ratioHealth);
            var _level = gruntSetupData.level;
            var tactixUnit = spawnedGruntUnit.GetComponent<TactixUnit>();
            if (tactixUnit != null)
            {
                tactixUnit.UpdateTextCP(_cp);
                tactixUnit.UpdateLevel(gruntSetupData.unitID + " " + Units.GetCreepResourceData(gruntSetupData.unitID).rarity, _level);
            }
            gruntCreepUnit.Add(spawnedGruntUnit);
            spawnedGruntUnit.OnDie += OnGruntDieListener;
            slotToUnitMap[slotIndex] = spawnedGruntUnit;
            return true;
        }

        public void TurnOnOffStarUnit(bool _isOn)
        {
            var progress = FormationProgress.Progress;
            var fieldUnits = progress.fieldUnits;
            foreach (var kvp in slotToUnitMap)
            {
                int slot = kvp.Key;
                CreepUnit mappedUnit = kvp.Value;
                if (mappedUnit == null || mappedUnit.IsDead) continue;
                var fieldUnitSetup = fieldUnits.FirstOrDefault(fieldUnit => fieldUnit.id == slot);
                if (fieldUnitSetup == null) continue;
                var _tactixUnit = mappedUnit.GetComponent<TactixUnit>();
                if (_tactixUnit != null)
                {
                    _tactixUnit.TurnOffStars(_isOn);
                }
            }
        }
        public void UpdateAllUnitCPText()
        {
            var progress = FormationProgress.Progress;
            if (progress == null || progress.fieldUnits == null)
                return;
            var fieldUnits = progress.fieldUnits;
            foreach (var kvp in slotToUnitMap)
            {
                int slot = kvp.Key;
                CreepUnit mappedUnit = kvp.Value;
                if (mappedUnit == null || mappedUnit.IsDead)
                    continue;
                var fieldUnitSetup = fieldUnits.FirstOrDefault(fieldUnit => fieldUnit.id == slot);
                if (fieldUnitSetup == null)
                    continue;
                // === Phần code cũ của bạn ===
                mappedUnit.ApplyMultiplier(fieldUnitSetup.ratioHealth, fieldUnitSetup.ratioDamage);
                mappedUnit.Init();
                mappedUnit.SetScale(GetScaleBasedOnLevel(fieldUnitSetup.level));
                var _tactixUnit = mappedUnit.GetComponent<TactixUnit>();
                if (_tactixUnit != null)
                {
                    var _cp = Units.GetCombatPoint(fieldUnitSetup.unitID, fieldUnitSetup.ratioDamage, fieldUnitSetup.ratioHealth);
                    var _level = fieldUnitSetup.level;
                    _tactixUnit.UpdateTextCP(_cp);
                    _tactixUnit.UpdateLevel(fieldUnitSetup.unitID + " " + Units.GetCreepResourceData(fieldUnitSetup.unitID).rarity, _level);
                    _tactixUnit.OnUpdateFloatingUI();
                }
            }
            TurnOnOffStarUnit(false);
        }
        public BaseUnit FindClosestTarget(BaseUnit sourceUnit)
        {
            if (sourceUnit == null) return null;
            BaseUnit closest = null;
            float minDist = float.MaxValue;
            Vector3 myPos = sourceUnit.transform.position;
            void Check(BaseUnit candidate)
            {
                if (candidate == null || candidate == sourceUnit || candidate.Faction == sourceUnit.Faction || candidate.IsDead) return;
                float dist = Vector3.Distance(myPos, candidate.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = candidate;
                }
            }
            if (sourceUnit.Faction == FactionId.Grunt)
            {
                foreach (var z in zombieCreepUnit) Check(z);
                foreach (var t in towers) Check(t);
            }
            else if (sourceUnit.Faction == FactionId.Zombie)
            {
                foreach (var g in gruntCreepUnit) Check(g);
            }
            else
            {
                foreach (var g in gruntCreepUnit) Check(g);
                foreach (var z in zombieCreepUnit) Check(z);
                foreach (var t in towers)
                {
                    Check(t);
                }
            }
            return closest;
        }
        public void SetPlayer(PlayablePlayer player)
        {
            if (player == null)
                return;

            Player = player;
            Player.SetGameplayActivity(true);
            MovePlayerToStartPoint();
            movePlayer = Player.GetComponent<TactixMovePlayer>();
            if (movePlayer == null)
                movePlayer = Player.gameObject.AddComponent<TactixMovePlayer>();
            movePlayer.Init();
            // movePlayer.MoveTo(new Vector3(0,0,0));
        }

        void MovePlayerToStartPoint()
        {
            if (Player == null || TactixEnvironmentManager.Instance == null || TactixEnvironmentManager.Instance.SpawnPlayerPoint == null)
                return;

            Transform spawnPoint = TactixEnvironmentManager.Instance.SpawnPlayerPoint;
            Player.transform.position = spawnPoint.position;
            Player.transform.rotation = spawnPoint.rotation;
        }
        public void Init()
        {
            GameplayState = GameplayState.PlayableSetup;
        }
        public void StackCamera(Camera cam)
        {
            MainCam = cam;
            if (gameplayCamera == null)
                gameplayCamera = cam;
            UIManager.StackCamera(MainCam);
        }

        public void CheckEventOnDieUnit(BaseUnit _unit)
        {
            if (_unit == null) return;
            _unit.OnDie -= OnGruntDieListener;
            _unit.OnDie += OnGruntDieListener;
            foreach (var grunt in gruntCreepUnit)
            {
                if (grunt == _unit) return;
            }
            gruntCreepUnit.Add(_unit as CreepUnit);
        }
        public void SpawnUnitInSlot(UnitId unitId, int slotID)
        {
            var fieldUnits = FormationProgress.Progress.fieldUnits;
            var newUnitSetupData = new UnitSetupData()
            {
                id = slotID,
                unitID = unitId,
                chestID = ChestId.Chest_1,
                level = 1,
                ratioHealth = 1.055f,
                ratioDamage = 1.055f
            };
            fieldUnits.Add(newUnitSetupData);
            if (newUnitSetupData != null)
            {
                SpawnSingleGruntUnit(newUnitSetupData);
                if (spawnMergeEffect && effectMergePrefab != null)
                {
                    GameObject _effectSpawn = Instantiate(effectMergePrefab, gruntSpawnInGrid.GetSpawnPosition(slotID), Quaternion.identity, transform);
                    Destroy(_effectSpawn, 1f);
                }
            }
            SoundSystem.instance.PlaySoundOneShot(audioSource, spawnOneUnitAudio);
        }
        public TactixRarityData GetRarityData(RarityId _rarityId)
        {
            var _rarityData = rarityDataList.Find(data => data.rarity == _rarityId);
            return _rarityData;
        }
        public void ResetField()
        {
            foreach (var t in towers) if (t != null) Destroy(t.gameObject);
            towers.Clear();
            foreach (var g in gruntCreepUnit) if (g != null) Destroy(g.gameObject);
            gruntCreepUnit.Clear();
            foreach (var z in zombieCreepUnit) if (z != null) Destroy(z.gameObject);
            zombieCreepUnit.Clear();
            ClearPreparedZombieWave();
            slotToUnitMap.Clear();
            wave = 0;
            phase = 0;
            SpawnGrids();
            SpawnTower(Level);
            SpawnGruntUnit();
        }
        public void HideTutorial()
        {
            tutorial.gameObject.SetActive(false);
        }


        #endregion
    }
    public enum GameplayState
    {
        None = 0,
        Prepare = 1,
        Idle = 2,
        Fight = 3,
        PlayableSetup = 10,
        ChestIntro = 11,
        HeroReward = 13,
        ArmyIntro = 14,
        Endcard = 15,
    }
    [Serializable]
    public class TactixRarityData
    {
        public RarityId rarity;
        public GameObject rarityText;
        public Color rarityColor;
        public Sprite rarityBGFrame;
        public GameObject rarityReceiveVFXPrefab;
    }
}
