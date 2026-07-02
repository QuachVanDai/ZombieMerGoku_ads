using System.Collections;
using ExampleProject.Gameplay.Scenes;
using ExampleProject.Gameplay.Unit;
using ExampleProject.Manager;
using ExampleProject.UI.BaseUI.BasePopup;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ExampleProject.UI.Playable
{
    public class ChestRewardPopup : BasePopup, IPointerClickHandler, IPointerDownHandler
    {
        [SerializeField] BaseTactixChest chest;
        [SerializeField] GameObject heroObject;
        [SerializeField] UnitId rewardUnitId = UnitId.RangedGrunt_4;
        [SerializeField] int rewardUnitLevel = 4;
        [SerializeField] bool openOnScreenTap = true;
        [SerializeField] bool showRewardPopupOnOpen = true;
        [SerializeField] float showRewardDelay = 0.6f;
        [SerializeField] GameObject hand;

        Coroutine showRewardCoroutine;
        bool isChestOpened;

        public BaseTactixChest Chest => chest;

        public void ShowChest()
        {
            isChestOpened = false;
            SetObjectActive(chest != null ? chest.gameObject : null, true);
            SetObjectActive(heroObject, false);
            Show();

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            TryOpenFromTap();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            TryOpenFromTap();
        }

        void TryOpenFromTap()
        {
            if (!CanOpenFromTap())
                return;

            OpenChest();
        }

        public void OpenChest()
        {
            Debug.Log("ChestRewardPopup: OpenChest");
            if (isChestOpened)
                return;
            if (hand != null)
                hand.SetActive(false);
            isChestOpened = true;

            if (chest != null)
                chest.PlayOpenAnimationOnly();

            // if (GameplayController.Instance != null)
            //     GameplayController.Instance.GameplayState = GameplayState.ChestOpen;

            if (showRewardPopupOnOpen)
                StartShowRewardPopup();
        }

        bool CanOpenFromTap()
        {
            return openOnScreenTap && !isChestOpened && IsShow;
        }

        public void ShowHero(string heroName = "")
        {
            SetObjectActive(heroObject, true);



            Show();
        }

        public void SetChest(BaseTactixChest target)
        {
            chest = target;
        }

        public void SetHeroObject(GameObject target)
        {
            heroObject = target;
        }

        public void SetRewardUnit(UnitId unitId)
        {
            rewardUnitId = unitId;
        }

        public void SetRewardUnit(UnitId unitId, int level)
        {
            rewardUnitId = unitId;
            rewardUnitLevel = level;
        }

        void StartShowRewardPopup()
        {
            if (showRewardCoroutine != null)
                StopCoroutine(showRewardCoroutine);

            showRewardCoroutine = StartCoroutine(ShowRewardPopupRoutine());
        }

        IEnumerator ShowRewardPopupRoutine()
        {
            if (showRewardDelay > 0f)
                yield return new WaitForSeconds(showRewardDelay);

            var rewardPopup = UIManager.Instance.GetTactixUnitPopup;
            if (rewardPopup != null)
                rewardPopup.Show();

            if (GameplayController.Instance != null)
                GameplayController.Instance.GameplayState = GameplayState.HeroReward;

            showRewardCoroutine = null;
        }

        static void SetObjectActive(GameObject target, bool active)
        {
            if (target != null)
                target.SetActive(active);
        }
    }
}
