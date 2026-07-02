using System;
using System.Collections.Generic;
using System.Linq;
using ExampleProject.Gameplay.Unit;
using ExampleProject.Manager;
using UnityEngine;

namespace ExampleProject
{
    [Serializable]
    public class FormationProgress
    {
        #region Fields

        public List<UnitSetupData> fieldUnits = new List<UnitSetupData>();
        public List<UnitSetupData> bagUnits = new List<UnitSetupData>();
        public List<SlotData> slotLockStatus = new List<SlotData>();
        public List<UnitNoticeConfirm> unitNoticeConfirms = new List<UnitNoticeConfirm>(); public bool isFirstPlay = true;
        public int openGrunt1Count = 0;
        public int openRanged1Count = 0;
        public int totalOpenChestsCount = 0;
        public int totalZombieKilled = 0;
        #endregion

        #region Properties

        public static FormationProgress Progress
        {
            get => UserDataManager.FormationProgress;
            set => UserDataManager.FormationProgress = value;
        }

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods


        #endregion

        #region Public Methods

        public void CheckFirstPlay()
        {
            if (Progress.slotLockStatus.Count > 0)
                isFirstPlay = false;
        }
        public void SetupDefaultFormation()
        {
            fieldUnits.Clear();
            bagUnits.Clear();
            unitNoticeConfirms.Clear();
            openGrunt1Count = 0;
            openRanged1Count = 0;
            totalOpenChestsCount = 0;
            totalZombieKilled = 0;

            fieldUnits.Add(new UnitSetupData
            {
                id = 10,
                unitID = UnitId.MeleeGrunt_5,
                chestID = ChestId.None,
                level = 2,
                ratioHealth = 2.12f,
                ratioDamage = 2.12f
            });

            fieldUnits.Add(new UnitSetupData
            {
                id = 14,
                unitID = UnitId.MeleeGrunt_8,
                chestID = ChestId.Chest_1,
                level = 1,
                ratioHealth = 1.04f,
                ratioDamage = 1.04f
            });

            fieldUnits.Add(new UnitSetupData
            {
                id = 11,
                unitID = UnitId.RangedGrunt_4,
                chestID = ChestId.Chest_1,
                level = 1,
                ratioHealth = 1.04f,
                ratioDamage = 1.04f
            });

            fieldUnits.Add(new UnitSetupData
            {
                id = 13,
                unitID = UnitId.RangedGrunt_4,
                chestID = ChestId.Chest_1,
                level = 1,
                ratioHealth = 1.04f,
                ratioDamage = 1.04f
            });
        }
        public static SlotData GetSlotData(int _id)
        {
            return Progress.slotLockStatus.FirstOrDefault(x => x.id == _id);
        }
        public static bool IsNewUnitShow(UnitId _id)
        {
            UnitNoticeConfirm _noticeConfirm = Progress.unitNoticeConfirms.FirstOrDefault(x => x.unitID == _id);
            if (_noticeConfirm == null)
            {
                Progress.unitNoticeConfirms.Add(new UnitNoticeConfirm
                {
                    unitID = _id,
                    isShowNewUnit = false
                });
                Debug.Log($"First time show unit {_id}");
                return true;
            }
            else
            {
                return _noticeConfirm.isShowNewUnit;
            }
        }
        public static void ChangeNewUnitShow(UnitId _id, bool _isShow)
        {
            UnitNoticeConfirm _noticeConfirm = Progress.unitNoticeConfirms.FirstOrDefault(x => x.unitID == _id);
            if (_noticeConfirm != null)
            {
                _noticeConfirm.isShowNewUnit = _isShow;
            }
            UserDataManager.SaveData();
        }
        public static UnitTactixData GetUnitTactixData(UnitId _id)
        {
            var _data = UnitTactix.GetUnitTactixResourceDataList().FirstOrDefault(x => x.unitID == _id);
            if (_data == null)
                return null;
            return _data;
        }
        public static void RandomizeRatios(UnitSetupData setup)
        {
            var tactix = GetUnitTactixData(setup.unitID);
            if (tactix == null)
            {
                setup.ratioHealth = 1f;
                setup.ratioDamage = 1f;
                Debug.LogWarning($"Không có UnitTactixData cho {setup.unitID} → ratio = 1");
                return;
            }

            int _random = UnityEngine.Random.Range(0, 100);
            if (_random < 95)
            {
                setup.ratioHealth = UnityEngine.Random.Range(tactix.minHealthRatio, tactix.maxHealthRatio);
                setup.ratioDamage = UnityEngine.Random.Range(tactix.minDamageRatio, tactix.maxDamageRatio);
            }
            else
            {
                setup.ratioHealth = tactix.maxHealthRatio;
                setup.ratioDamage = tactix.maxDamageRatio;
            }
        }

        public static void ResetProgress()
        {
            Progress.slotLockStatus = new List<SlotData>();
            Progress.SetupDefaultFormation();
        }
        #endregion
    }

    [Serializable]
    public class UnitSetupData
    {
        public int id;
        public UnitId unitID;
        public ChestId chestID;
        public int level;
        public float ratioHealth;
        public float ratioDamage;
    }
    [Serializable]
    public class SlotData
    {
        public int id;
        public bool isLocked;
        public int watchAdsCount;
    }
    [Serializable]
    public class UnitNoticeConfirm
    {
        public UnitId unitID;
        public bool isShowNewUnit;
    }
}
