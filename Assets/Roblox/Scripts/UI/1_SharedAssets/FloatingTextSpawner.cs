using UnityEngine;

namespace ExampleProject.UI.Shared
{
    public class FloatingTextSpawner : MonoBehaviour
    {
        #region Fields

        [SerializeField] FloatingText floatingTextPrefab;
        [SerializeField] FloatingText floatingCriticalTextPrefab;
        [SerializeField] float randomRadius = 10;

        #endregion

        #region Properties



        #endregion

        #region Properties



        #endregion

        #region LifeCycle   



        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public void SpawnFloatingText(string _text)
        {
            if (floatingTextPrefab == null)
            {
                Debug.LogWarning("FloatingTextPrefab or SpawnPoint is not assigned.");
                return;
            }

            FloatingText _floatingText = Instantiate(floatingTextPrefab, this.transform);
            var _randomOffset = Random.insideUnitCircle * randomRadius;
            _floatingText.SetAnchorPos(_randomOffset);
            _floatingText.Show(_text);
        }
        public void SpawnFloatingCriticalText(string _text)
        {
            if (floatingCriticalTextPrefab == null)
            {
                Debug.LogWarning("FloatingCriticalTextPrefab or SpawnPoint is not assigned.");
                return;
            }
            FloatingText _floatingText = Instantiate(floatingCriticalTextPrefab, this.transform);
            var _randomOffset = Random.insideUnitCircle * randomRadius;
            _floatingText.SetAnchorPos(_randomOffset);
            _floatingText.Show(_text);
        }

        #endregion
    }
}