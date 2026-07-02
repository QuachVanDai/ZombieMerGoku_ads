using ExampleProject.GameSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ExampleProject.UI.BaseUI
{
    public class BaseButton : MonoBehaviour, IPointerDownHandler
    {
        #region Fields

        Button button;
        RectTransform rectTransform;


        #endregion

        #region Properties

        protected Button ThisButton => button = button != null ? button : GetComponent<Button>();
        [HideInInspector] protected RectTransform ThisRectTransform => rectTransform = rectTransform != null ? rectTransform : GetComponent<RectTransform>();

        #endregion

        #region LifeCycle   

        protected virtual void OnEnable()
        {
            ThisButton.onClick.AddListener(OnClickListenerMethod);
        }
        protected virtual void OnDisable()
        {
            ThisButton.onClick.RemoveListener(OnClickListenerMethod);
        }

        #endregion

        #region Private Methods

        protected virtual void OnClickListenerMethod()
        {
            SoundSystem.Instance.PlayUIClick();
        }

        #endregion

        #region Public Methods

        public void SetActive(bool _value)
        {
            this.gameObject.SetActive(_value);
        }
        public void SetAnchorPos(Vector2 _newPos)
        {
            ThisRectTransform.anchoredPosition = _newPos;
        }
        public virtual void OnPointerDown(PointerEventData eventData)
        {

        }


        #endregion
    }
}