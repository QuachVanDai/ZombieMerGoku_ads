using DG.Tweening;
using ExampleProject.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ExampleProject.UI.UIAnimation
{
    public class PopupAnimationControl : MonoBehaviour
    {
        #region Fields
        [SerializeField] List<PopupItem> popupItemList;
        Sequence showSequence;
        Sequence hideSequence;
        #endregion
        #region Properties
        public PopupAnimationState CurrentAnimationState { get; private set; }
        #endregion
        #region LifeCycle   
        #endregion
        #region Private Methods
        void SetShowSequence()
        {
            //showSequence = DOTween.Sequence();
            //foreach (var _item in popupItemList)
            //{
            //    showSequence.Join(_item.GetShowTween());
            //}
            showSequence = DOTween.Sequence();
            Tween tween;
            foreach (var _item in popupItemList)
            {
                tween = _item.GetShowTween();
                showSequence.Insert(0, tween); // All tweens start at time 0, delays are respected per tween
            }
        }
        void SetHideSequence()
        {
            hideSequence = DOTween.Sequence();
            Tween tween;
            foreach (var _item in popupItemList)
            {
                //hideSequence.Join(_item.GetHideTween());
                tween = _item.GetHideTween();
                hideSequence.Insert(0, tween);
            }
        }
        void HideAllImmediately()
        {
            foreach (var item in popupItemList)
            {
                item.HideImmediately();
            }
            CurrentAnimationState = PopupAnimationState.None;
        }
        void ShowAllImmediately()
        {
            foreach (var item in popupItemList)
            {
                item.ShowImmediately();
            }
            CurrentAnimationState = PopupAnimationState.Showed;
        }
        void GetAllMenuItem()
        {
            popupItemList.Clear();
            // Get all PopupItem components, then filter to only those whose GameObject is active in hierarchy
            popupItemList = Helpers.GetAllChildrenComponent<PopupItem>(this.transform).FindAll(item => item.gameObject.activeInHierarchy);
        }
        #endregion
        #region Public Methods
        public void StartShow(bool _isDoAnimation, Action _onShowStarted, Action _onShowCompleted)
        {
            _onShowStarted?.Invoke();
            //Hide first, then show
            HideAllImmediately();
            SetShowSequence();
            CurrentAnimationState = PopupAnimationState.Showing;
            showSequence.Play().SetUpdate(true).OnComplete(() =>
            {
                _onShowCompleted?.Invoke();
                CurrentAnimationState = PopupAnimationState.Showed;
            });
            if (_isDoAnimation is false)
            {
                showSequence.Complete();
            }
        }
        public void StartHide(bool _isDoAnimation, Action _onHideStarted, Action _onHideCompleted)
        {
            SetHideSequence();
            CurrentAnimationState = PopupAnimationState.Hiding;
            _onHideStarted?.Invoke();
            hideSequence.Play().SetUpdate(true).OnComplete(() =>
            {
                _onHideCompleted?.Invoke();
                CurrentAnimationState = PopupAnimationState.Hidden;
            });
            if (_isDoAnimation is false)
            {
                hideSequence.Complete();
            }
        }
        #endregion
    }
    public enum PopupAnimationState
    {
        None,
        Showing,
        Showed,
        Hiding,
        Hidden,
    }
}