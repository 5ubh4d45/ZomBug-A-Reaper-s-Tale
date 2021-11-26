using System;
using UnityEngine;

namespace Game.HealthSystem
{
    /// <summary>
    /// This type of health bar is basically fill amount health but instead of relying on Image.fillAmount,
    ///  This relies on Sprite's scale
    /// </summary>
    public class SpriteFillHealthBar : HealthBar<IntHealthSystem>
    {
        #region Variables
        [SerializeField] private Gradient _tintGradient;
        [SerializeField] private Transform _barAnchor;
        [SerializeField] private SpriteRenderer _barFill;
        [SerializeField] private GameObject _backGroundBox;

        private bool _isHealthBarOn;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls

        #endregion

        private void OnEnable()
        {
            //turning off the sprite renderer of the health bar
            _barFill.enabled = false;
            _backGroundBox.SetActive(false);
            _isHealthBarOn = false;
        }

        #region Component Functions
        public override void Setup(IntHealthSystem healthSystem)
        {
            _healthSystem = healthSystem;
            _healthSystem.OnDamaged += HealthChanged;
            _healthSystem.OnHealed += HealthChanged;
        }

        private void HealthChanged(float changeAmount)
        {
            //turning on the sprite renderer of the health bar
            HealthBarToggle();

            Color color = _tintGradient.Evaluate(_healthSystem.Health01);
            _barFill.color = color;
            _barAnchor.localScale = new Vector3(_healthSystem.Health01, _barAnchor.localScale.y, _barAnchor.localScale.z);
        }

        private void HealthBarToggle()
        {
            if (_isHealthBarOn) return;
            _barFill.enabled = true;
            _backGroundBox.SetActive(true);
        }
        #endregion
    }
}