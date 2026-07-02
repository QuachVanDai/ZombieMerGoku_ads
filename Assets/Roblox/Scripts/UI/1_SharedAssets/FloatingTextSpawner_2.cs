using ExampleProject.Gameplay.Currency;
using UnityEngine;

namespace ExampleProject.UI.Shared
{
    public class FloatingTextSpawner_2 : MonoBehaviour
    {
        #region Fields

        [SerializeField] FloatingText_2 floatingTextPrefab;
        [SerializeField] RectTransform rect;
        [SerializeField] float minAngle = 45f;
        [SerializeField] float maxAngle = 135f;
        [SerializeField] float radius = 100f;

        #endregion

        #region Properties



        #endregion

        #region Properties



        #endregion

        #region LifeCycle   

        void OnDrawGizmosSelected()
        {
            if (rect == null)
            {
                return;
            }

            var _fromAngle = Mathf.Min(minAngle, maxAngle);
            var _toAngle = Mathf.Max(minAngle, maxAngle);
            var _center = rect.position;
            var _segments = Mathf.Max(2, Mathf.CeilToInt((_toAngle - _fromAngle) / 5f));

            Gizmos.color = Color.cyan;
            var _prevPoint = _center + (Vector3)(new Vector2(Mathf.Cos(_fromAngle * Mathf.Deg2Rad), Mathf.Sin(_fromAngle * Mathf.Deg2Rad)) * radius);

            for (int i = 1; i <= _segments; i++)
            {
                var _t = i / (float)_segments;
                var _angle = Mathf.Lerp(_fromAngle, _toAngle, _t) * Mathf.Deg2Rad;
                var _point = _center + (Vector3)(new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle)) * radius);
                Gizmos.DrawLine(_prevPoint, _point);
                _prevPoint = _point;
            }

            var _fromDir = new Vector2(Mathf.Cos(_fromAngle * Mathf.Deg2Rad), Mathf.Sin(_fromAngle * Mathf.Deg2Rad));
            var _toDir = new Vector2(Mathf.Cos(_toAngle * Mathf.Deg2Rad), Mathf.Sin(_toAngle * Mathf.Deg2Rad));

            Gizmos.DrawLine(_center, _center + (Vector3)(_fromDir * radius));
            Gizmos.DrawLine(_center, _center + (Vector3)(_toDir * radius));

            //Gizmos.color = Color.yellow;
            //Gizmos.DrawWireSphere(_center, 6f);
        }
        #endregion

        #region Private Methods

        Vector3 GetRandomPosInArc()
        {
            var _fromAngle = Mathf.Min(minAngle, maxAngle);
            var _toAngle = Mathf.Max(minAngle, maxAngle);
            var _angle = Random.Range(_fromAngle, _toAngle) * Mathf.Deg2Rad;
            var _distance = Mathf.Sqrt(Random.value) * radius;

            var _offset = new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle)) * _distance;
            var _center = rect.position;

            return new Vector3(_center.x + _offset.x, _center.y + _offset.y, _center.z);
        }



        #endregion

        #region Public Methods

        public void SpawnFloatingText(CurrencyType _type, string _text)
        {
            if (floatingTextPrefab == null || rect == null)
            {
                Debug.LogWarning("FloatingTextPrefab or Rect is not assigned.");
                return;
            }

            FloatingText_2 _floatingText = Instantiate(floatingTextPrefab, transform);
            var _randomPos = GetRandomPosInArc();

            _floatingText.SetAnchorPos(rect.anchoredPosition);
            _floatingText.SetEndPos(_randomPos);
            _floatingText.Show(_type,_text);
        }

        #endregion
    }
}