using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Compass
{
    public class Compass : MonoBehaviour
    {
        [SerializeField] private GameObject _prefabIcon;
        [SerializeField] private RawImage _compassImage;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private TextMeshProUGUI _compassDirectionText;
        [SerializeField] private List<CompassMarker> _markers = new List<CompassMarker>();
        
        private Dictionary<int, string> _displayAngle = new Dictionary<int, string>();
        private List<CompassMarker> _compassMarkers = new List<CompassMarker>();
        private float _compassUnit;

        private void Start()
        {
            _compassUnit = _compassImage.rectTransform.rect.width / 360;

            foreach (CompassMarker item in _markers)
            {
                AddCompassMarker(item);
            }
        }

        public void Update()
        {
            //_compassImage.uvRect = new Rect(_playerTransform.localEulerAngles.y / 360, 0, 1, 1);

            foreach (CompassMarker marker in _compassMarkers)
            {
                marker.Image.rectTransform.anchoredPosition = GetPositionOnCompass(marker);
            }
        
            Vector3 forward = _playerTransform.transform.forward;
            forward.y = 0;

            float headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;
            headingAngle = 5 * Mathf.RoundToInt((headingAngle / 5)) + 5;

            if (headingAngle > 360)
            {
                headingAngle -= 360;
            }
        
            if (_compassDirectionText != null)
            {
                _compassDirectionText.text = _displayAngle.ContainsKey((int)headingAngle) ? _displayAngle[(int)headingAngle] : headingAngle.ToString();
            }
        }
        public void AddCompassMarker(CompassMarker marker)
        {
            GameObject newMarker = Instantiate(_prefabIcon, _compassImage.transform);
            marker.Image = newMarker.GetComponent<Image>();
            marker.Image.sprite = marker.Icon;

            _compassMarkers.Add(marker);
        }

        Vector2 GetPositionOnCompass(CompassMarker marker)
        {
            Vector2 playerPos = new Vector2(_playerTransform.position.x, _playerTransform.position.z);
            Vector2 playerFwd = new Vector2(_playerTransform.forward.x, _playerTransform.forward.z);

            float angle = Vector2.SignedAngle(marker.Position - playerPos, playerFwd);

            return new Vector2(_compassUnit * angle, 0f);
        }
    }
}