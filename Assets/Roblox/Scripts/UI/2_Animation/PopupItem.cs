using DG.Tweening;
using System.Collections;
using UnityEngine;
namespace ExampleProject.UI.UIAnimation
{
    public abstract class PopupItem : MonoBehaviour
    {
        #region Fields
        [SerializeField] protected float delayShow;
        [SerializeField] protected float delayHide;
        [SerializeField] protected float timeShow = 0.3f;
        [SerializeField] protected float timeHide = 0.3f;
        #endregion
        #region Properties
        public Tween ShowTween { get; protected set; }
        public Tween HideTween { get; protected set; }
        #endregion
        #region LifeCycle   
        #endregion
        #region Private Methods
        #endregion
        #region Public Methods
        public abstract Tween GetShowTween();
        public abstract Tween GetHideTween();
        public abstract void SetThisAsShow();
        public abstract void SetThisAsHide();
        public abstract void ShowImmediately();
        public abstract void HideImmediately();
        #endregion
    }
}