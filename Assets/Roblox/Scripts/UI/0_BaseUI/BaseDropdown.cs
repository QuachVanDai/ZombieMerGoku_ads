using ExampleProject.GameSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ExampleProject.UI.BaseUI
{
    public class BaseDropdown : MonoBehaviour, IPointerDownHandler
    {
        #region Fields

        Dropdown dropdown;

        #endregion

        #region Properties

        protected Dropdown ThisDropdown => dropdown = dropdown != null ? dropdown : GetComponent<Dropdown>();

        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods

        protected virtual void OnEnable()
        {
            ThisDropdown.onValueChanged.AddListener(ListenerMethod);
        }
        protected virtual void OnDisable()
        {
            ThisDropdown.onValueChanged.RemoveListener(ListenerMethod);
        }
        protected virtual void ListenerMethod(int _value)
        {
            SoundSystem.Instance.PlayUIClick();
       
        }

        #endregion

        #region Public Methods

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            SoundSystem.Instance.PlayUIClick();
          
        }

        #endregion
    }
}