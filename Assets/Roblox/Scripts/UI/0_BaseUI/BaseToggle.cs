using ExampleProject;
using ExampleProject.GameSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.UI.BaseUI
{
    public class BaseToggle : MonoBehaviour
    {
        #region Fields

        Toggle toggle;

        #endregion

        #region Properties

        protected Toggle ThisToggle => toggle = toggle != null ? toggle : GetComponent<Toggle>();

        #endregion

        #region LifeCycle   

        protected virtual void OnEnable()
        {
            ThisToggle.onValueChanged.AddListener(ListenerMethod);
        }


        protected virtual void OnDisable()
        {
            ThisToggle.onValueChanged.RemoveListener(ListenerMethod);
        }

        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        protected virtual void ListenerMethod(bool _value)
        {
            SoundSystem.Instance.PlayUIClick();
        }

        #endregion
    }
}