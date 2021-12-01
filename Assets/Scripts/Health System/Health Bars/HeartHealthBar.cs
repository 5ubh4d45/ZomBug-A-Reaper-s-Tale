using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.HealthSystem
{
    /// <summary>
    /// The Heart Base Health Bar to create one simply create an empty object and attach this class to it
    /// </summary>
    public class HeartHealthBar : HealthBar<HeartHealthSystem>
    {
        #region Variables
        [SerializeField] private Sprite[] _heartSprites;
        [SerializeField] private Color _heartColor;
        [SerializeField] private float _heartSize;
        [SerializeField] private Vector2 _referenceCanvasSize;
        private List<Image> _hearts;
        #endregion


        #region Getters And Setters
        public void SetHealthSystem(HeartHealthSystem healthSystem)
        {
            // Unsubscribe the old System
            if (_healthSystem != null)
            {
                _healthSystem.OnDamaged -= HealthChanged;
                _healthSystem.OnHealed -= HealthChanged;
            }

            _healthSystem = healthSystem;

            //Destroy all old hearts
            if (_hearts != null)
            {
                foreach (var heart in _hearts)
                {
                    Destroy(heart.gameObject);
                }
            }

            // initialise ne hearts
            _hearts = new List<Image>();
            Vector2 anchoredPosition = Vector2.zero;
            for (int i = 0; i < _healthSystem.HeartCount; i++)
            {
                float heartSize = _heartSize / _referenceCanvasSize.x * Screen.width;
                CreateHeartImage(anchoredPosition, i);
                anchoredPosition += new Vector2(heartSize, 0);
            }

            // subscribe to new health system
            _healthSystem.OnDamaged += HealthChanged;
            _healthSystem.OnHealed += HealthChanged;
        }
        #endregion


        #region Unity Calls

        #endregion


        #region Component Functions
        public override void Setup(HeartHealthSystem healthSystem)
        {
            SetHealthSystem(healthSystem);
            HealthChanged(_healthSystem.Health);
        }

        private void HealthChanged(float changeAmount)
        {
            Debug.Log("Starting HealthChanged");

            for (int i = 0; i < _healthSystem.HeartCount; i++)
            {
                Heart heart = _healthSystem.Hearts[i];
                _hearts[i].sprite = _heartSprites[heart.Fragments];

                Debug.Log("Health Changing:  " + heart.Fragments);
            }

            Debug.Log("Ending HealthChanged");
        }

        private Image CreateHeartImage(Vector2 anchoredPosition, int index)
        {
            GameObject heartGameObject = new GameObject($"Heart {index}", typeof(Image));
            float heartSizeX = _heartSize / _referenceCanvasSize.x * Screen.width;
            float heartSizeY = _heartSize / _referenceCanvasSize.y * Screen.height;

            heartGameObject.transform.SetParent(transform);
            heartGameObject.transform.localPosition = Vector3.zero;

            heartGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
            heartGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(heartSizeX, heartSizeY);

            Image heartImage = heartGameObject.GetComponent<Image>();
            heartImage.sprite = _heartSprites[_heartSprites.Length - 1];
            heartImage.color = _heartColor;

            _hearts.Add(heartImage);
            return heartImage;
        }
        #endregion
    }
}