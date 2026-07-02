using ExampleProject.GameSystem;
using ExampleProject.Manager;
using ExampleProject.UI.UIAnimation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ExampleProject.UI.BaseUI.BasePopup
{
    [RequireComponent(typeof(PopupAnimationControl))]
    public class BasePopup : MonoBehaviour
    {
        #region Fields
        [SerializeField] protected List<Button> closeButtons;
        protected Action onStartShowAction;
        protected Action onCompleteShowAction;
        protected Action onStartHideAction;
        protected Action onCompleteHideAction;
        [SerializeField] PopupId id;
        [SerializeField] bool isDestroyOnHide = false;
        [SerializeField] bool isDoAnimation = true;
        [SerializeField] bool useSimpleSetActive = true;
        PopupAnimationControl menuAnimationControl;
        protected object data;
        bool listenersAdded;
        #endregion
        #region Properties
        public bool IsShow => this.gameObject.activeSelf;
        public PopupId Id => id;
        protected GameObject RaycastShield => transform.GetChild(transform.childCount - 1).gameObject;
        protected PopupAnimationControl ThisMenuAnimationControl => menuAnimationControl = menuAnimationControl != null ? menuAnimationControl : GetComponent<PopupAnimationControl>();
        #endregion
        #region LifeCycle   
        #endregion
        #region Private Methods
        protected virtual void Init() { }
        protected virtual void InitData() { }
        protected virtual void OnShowStarted()
        {
            SetActive(true);
            SetInteractable(false);
            AddListener();
            InitData();
            Init();
            this.onStartShowAction?.Invoke();
        }
        protected virtual void OnShowCompleted()
        {
            SetInteractable(true);
            this.onCompleteShowAction?.Invoke();
        }
        protected virtual void OnHideStarted()
        {
            SetInteractable(false);
            this.onStartHideAction?.Invoke();
        }
        protected virtual void OnHideCompleted()
        {
            SetInteractable(true);
            this.onCompleteHideAction?.Invoke();
            RemoveListener();
            if (isDestroyOnHide)
            {
                UIManager.Instance.RemovePopup(id);
                Destroy(this.gameObject);
            }
            else
                SetActive(false);
        }
        protected virtual void AddListener()
        {
            if (listenersAdded)
                return;

            foreach (var _button in closeButtons)
                _button.onClick.AddListener(OnClickCloseListener);

            listenersAdded = true;
        }
        protected virtual void RemoveListener()
        {
            if (!listenersAdded)
                return;

            foreach (var _button in closeButtons)
                _button.onClick.RemoveListener(OnClickCloseListener);

            listenersAdded = false;
        }
        protected virtual void OnClickCloseListener()
        {
            this.Hide();
            SoundSystem.Instance.PlayUIClick();
        }
        #endregion
        #region Public Methods
        public BasePopup SetData(object _data)
        {
            this.data = _data;
            return this;
        }
        public BasePopup SetOnStartShow(Action _actionOnStartShow)
        {
            this.onStartShowAction = _actionOnStartShow;
            return this;
        }
        public BasePopup SetOnCompleteShow(Action _actionOnCompleteShow)
        {
            this.onCompleteShowAction = _actionOnCompleteShow;
            return this;
        }
        public BasePopup SetOnStartHide(Action _actionOnStartHide)
        {
            this.onStartHideAction = _actionOnStartHide;
            return this;
        }
        public BasePopup SetOnCompleteHide(Action _actionOnCompleteHide)
        {
            this.onCompleteHideAction = _actionOnCompleteHide;
            return this;
        }
        public BasePopup SetIsDoAnimation(bool _value)
        {
            isDoAnimation = _value;
            return this;
        }
        public void SetId(PopupId _id)
        {
            id = _id;
        }
        public void SetActive(bool _value)
        {
            this.gameObject.SetActive(_value);
        }
        public void SetInteractable(bool _value)
        {
            if (transform.childCount == 0)
                return;

            RaycastShield.SetActive(!_value);
        }
        public virtual void Show()
        {
            if (useSimpleSetActive)
            {
                OnShowStarted();
                OnShowCompleted();
                return;
            }

            ThisMenuAnimationControl.StartShow(isDoAnimation, _onShowStarted: OnShowStarted, _onShowCompleted: OnShowCompleted);
        }
        public virtual void Hide()
        {
            if (useSimpleSetActive)
            {
                OnHideStarted();
                OnHideCompleted();
                return;
            }

            ThisMenuAnimationControl.StartHide(isDoAnimation, _onHideStarted: OnHideStarted, _onHideCompleted: OnHideCompleted);
        }
        public void PretendShow()
        {
            ThisMenuAnimationControl.StartShow(isDoAnimation, null, null);
        }
        public void PretendHide()
        {
            ThisMenuAnimationControl.StartHide(isDoAnimation, null, null);
        }
        public void DisableInteractable(float _duration = 0.6f)
        {
            //foreach (var btn in disableButtons)
            //{
            //    btn.interactable = false;
            //}
            //SetInteractable(false);
            //StartCoroutine(IEDisableInteractable());
            //IEnumerator IEDisableInteractable()
            //{
            //    yield return new WaitForSeconds(_duration);
            //    foreach (var btn in disableButtons)
            //    {
            //        btn.interactable = true;
            //    }
            //    SetInteractable(true);
            //}
        }
        public BasePopup ClearOnCompleteHideAction()
        {
            this.onCompleteHideAction = null;
            return this;
        }
        #endregion
    }
}
