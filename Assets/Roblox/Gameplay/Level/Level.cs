using BreakInfinity;
using ExampleProject.Gameplay.Unit;
using ExampleProject.Gameplay.Weapon;
using ExampleProject.UI.BaseUI.BasePopup;
using System;
using System.Collections;
using System.Collections.Generic;
using ExampleProject.Gameplay.Buffs;
using UnityEngine;
using Units = ExampleProject.Gameplay.Unit.Units;

namespace ExampleProject.Gameplay.Level
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/Level/LevelData")]
    public class LevelData : ScriptableObject
    {
        #region Fields

        public int levelNumber;
        public int coinWinLevel;
        public int gemWinLevel;
        
        public List<LevelPhase> levelPhase;
        public List<Buff> levelZombieBuffs;
        public List<Buff> levelTowerBuffs;

        #endregion

        #region Properties
     


        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        
        public bool IsOutOfWave(int _phase, int _wave)
        {
            return _wave >= levelPhase[_phase].waves.Count;
        }
        public bool IsOutOfPhase(int _phase)
        {
            return _phase >= levelPhase.Count;
        }
        public List<Buff> GetPhaseTowerBuffs(int _phase)
        {
            return levelPhase[_phase].phaseTowerBuffs;
        }
        public List<Buff> GetLevelTowerBuffs()
        {
            return levelTowerBuffs;
        }
        public List<Buff> GetWaveZombieBuffs(int _phase, int _wave)
        {
            return levelPhase[_phase].GetWave(_wave).waveBuffs;
        }
        public List<Buff> GetLevelZombieBuffs()
        {
            return levelZombieBuffs;
        }
        public LevelPhase GetLevelPhase(int _phase)
        {
            return levelPhase[_phase];
        }
        public List<WaveInfo> GetWaveInfos(int _phase, int _wave)
        {
            //return levelPhase[_phase].waves[_wave].units;
            return GetLevelPhase(_phase).GetWave(_wave).units;
        }
        public List<LevelPhase> GetLevelPhases()
        {
            return levelPhase;
        }

        #endregion
    }

    [Serializable]
    public class LevelPhase
    {
        public List<Wave> waves;
        public List<Buff> phaseTowerBuffs;
        public UnitId towerUnitId;

        public Wave GetWave(int _index)
        {
            return waves[_index];
        }
        public List<Buff> GetWaveZombieBuffs(int _wave)
        {
            return waves[_wave].waveBuffs;
        }
    }
    [Serializable]
    public class Wave
    {
        public List<WaveInfo> units;
        public List<Buff> waveBuffs;

        public WaveInfo GetWaveInfo(int _index)
        {
            return units[_index];
        }
    }
    [Serializable]
    public class WaveInfo
    {
        public UnitId unitId;
        public int quantity;
    }
}
