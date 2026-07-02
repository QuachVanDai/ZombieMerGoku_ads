using System.Collections.Generic;
using BreakInfinity;
using ExampleProject.Gameplay.Faction;
using ExampleProject.Gameplay.Scenes;
using ExampleProject.Gameplay.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject
{
    public class TactixUnit : MonoBehaviour
    {
        #region Fields

        [SerializeField] bool hideNonBossFloatingUI = true;
        [SerializeField] Text unitCPText;
        [SerializeField] GameObject healthBar,stars, unitCP;
        [SerializeField] List<GameObject> starList;
        [SerializeField] BaseUnit unit;
        #endregion

        #region Properties



        #endregion

        #region LifeCycle
        
        void OnEnable()
        {
            EventDispatcher.Instance.AddListener(EventName.OnAfterGameStateChange,OnUpdateFloatingUI);
            ResolveUnit();
            OnUpdateFloatingUI();
        }

        void OnDisable()
        {
            EventDispatcher.Instance.RemoveListener(EventName.OnAfterGameStateChange,OnUpdateFloatingUI);
        }
        #endregion

        #region Private Methods

        void ResolveUnit()
        {
            if (unit != null)
                return;

            unit = GetComponent<BaseUnit>();
            if (unit == null)
                unit = GetComponentInParent<BaseUnit>();
        }

        bool IsBossUnit()
        {
            ResolveUnit();
            if (unit == null)
                return false;

            UnitData unitData = Units.GetUnitData(unit.UnitId);
            if (unitData != null)
                return unitData.unitType == UnitType.Boss;

            return unit.UnitId == UnitId.Boss_1 ||
                   unit.UnitId == UnitId.Boss_2 ||
                   unit.UnitId == UnitId.Boss_3 ||
                   unit.UnitId == UnitId.Boss_4;
        }

        void SetFloatingObjectsActive(bool isActive)
        {
            if (healthBar != null)
                healthBar.SetActive(isActive);
            if (stars != null)
                stars.SetActive(false);
            if (unitCP != null)
                unitCP.SetActive(false);
        }

        public void OnUpdateFloatingUI(EventName _name = EventName.NONE, object _obj = null)
        {
            ResolveUnit();

            if (hideNonBossFloatingUI && !IsBossUnit())
            {
                SetFloatingObjectsActive(false);
                return;
            }

            bool isFight = GameplayController.Instance != null &&
                           GameplayController.Instance.GameplayState == GameplayState.Fight;
            if (!isFight)
            {
                if (healthBar != null)
                    healthBar.SetActive(false);
                if (unit != null && unit.Faction == FactionId.Grunt)
                {
                //    unitCP.SetActive(true);
                    //   stars.SetActive(true);
                }
            }
            else
            {
                if (healthBar != null)
                    healthBar.SetActive(true);
                if (unit != null && unit.Faction == FactionId.Grunt)
                {
//unitCP.SetActive(false);
                    if (stars != null)
                        stars.SetActive(false);
                }
            }
        }
        public void TurnOffStars(bool _isOn)
        {
            if (stars != null)
                stars.SetActive(_isOn);
        }
        #endregion

        #region Public Methods

        public void UpdateLevel(string _unitName,int _level)
        {
            if (starList == null || starList.Count == 0)
                return;

            for (int i = 0; i < starList.Count; i++)
            {
                if (starList[i] == null)
                    continue;

                if (i < _level)
                {
                    starList[i].SetActive(true);
                }
                else
                {
                    starList[i].SetActive(false);
                }
            }
        }
        public void UpdateTextCP(BigDouble _cp)
        {
//            unitCPText.text = BigDouble.FormatShorthand(_cp).ToString() ;
        }

        #endregion
    }
}
