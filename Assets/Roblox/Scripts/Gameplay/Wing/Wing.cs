using ExampleProject.Gameplay.Weapon;
using ExampleProject.Tools;
using UnityEngine;

namespace ExampleProject.Gameplay.Wing
{
    public class Wing : MonoBehaviour
    {
        #region Fields

        public WingId id;

        #endregion

        #region Properties



        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public void SetActive(bool _isActive)
        {
            gameObject.SetActive(_isActive);
        }
        public void SelfDestroy()
        {
            Destroy(gameObject);
        }
        public void Init(WingId _id)
        {
            this.id = _id;
        }

        #endregion
    }
}