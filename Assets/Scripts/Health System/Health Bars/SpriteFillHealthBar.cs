using System;
using UnityEngine;

namespace Game.HealthSystem
{
    public class SpriteFillHealthBar : HealthBar<IntHealthSystem>
    {
        #region Variables
        [SerializeField] private Gradient _tintGradient;
        [SerializeField] private Transform _barAnchor;
        [SerializeField] private SpriteRenderer _barFill;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls

        #endregion


        #region Component Functions
        public override void Setup(IntHealthSystem healthSystem)
        {
            _healthSystem = healthSystem;
            _healthSystem.OnDamaged += HealthChanged;
            _healthSystem.OnHealed += HealthChanged;
        }

        private void HealthChanged(float changeAmount)
        {
            Color color = _tintGradient.Evaluate(_healthSystem.Health01);
            _barFill.color = color;
            _barAnchor.localScale = new Vector3(_healthSystem.Health01, _barAnchor.localScale.y, _barAnchor.localScale.z);
        }
        #endregion
    }
}