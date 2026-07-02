using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExampleProject.Tools;

namespace ExampleProject.Tools
{
    public class KeepObject : MonoBehaviour
    {
        #region Fields

        [SerializeField] Transform systemPlacement;
        [SerializeField] Transform managerPlacement;

        #endregion

        #region Properties

        public Transform SystemPlacement { get { return systemPlacement; } }
        public Transform ManagerPlacement { get { return managerPlacement; } }

        #endregion

        #region LifeCycle    
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        #endregion

        #region Private Methods
        #endregion

        #region Public Methods
        #endregion
    }
}