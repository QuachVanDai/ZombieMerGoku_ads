using UnityEngine;

namespace ExampleProject.Gameplay.Scenes
{
    public class SpawnInGrid : MonoBehaviour
    {
        #region Fields

        [SerializeField] Vector3 center;
        [SerializeField] int rows = 5;
        [SerializeField] int columns = 5;
        [SerializeField] float spacingX = 1f;
        [SerializeField] float spacingY = 1f;
        [SerializeField] Vector3 offset;

    
        #endregion

        #region Public Methods

        public void SetGridSize(int _rows, int _columns)
        {
            rows = Mathf.Max(1, _rows);
            columns = Mathf.Max(1, _columns);
        }
        public void SetPosition(Vector3 vector3)
        {
            this.transform.position = vector3;
        }
        public void SetGridSpacing(float _spacingX, float _spacingY)
        {
            spacingX = Mathf.Max(0.01f, _spacingX);
            spacingY = Mathf.Max(0.01f, _spacingY);
        }

        public Vector3 GetSpawnPosition(int _index)
        {
            int _maxIndex = rows * columns - 1;
            _index = Mathf.Clamp(_index, 0, _maxIndex);
            
            int _row = _index / columns;
            int _column = _index % columns;

            Vector3 _bottomLeft = GetBottomLeft();
            return _bottomLeft + transform.right * (_column * spacingX) + transform.forward * (_row * spacingY);
        }
        public Vector3 GetRandomSpawnPosition()
        {
            int _totalPoints = rows * columns;
            int _randomIndex = Random.Range(0, _totalPoints);
            return GetSpawnPosition(_randomIndex);
        }

        #endregion

        #region Private Methods

        Vector3 GetWorldCenter()
        {
            return transform.TransformPoint(center + offset);
        }

        Vector3 GetBottomLeft()
        {
            float _width = (columns - 1) * spacingX;
            float _depth = (rows - 1) * spacingY;
            Vector3 _worldCenter = GetWorldCenter();

            return _worldCenter - transform.right * (_width * 0.5f) - transform.forward * (_depth * 0.5f);
        }

     

        #endregion

        #region LifeCycle



        #endregion
    }
}