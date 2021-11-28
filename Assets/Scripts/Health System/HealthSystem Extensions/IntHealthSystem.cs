using UnityEngine;

namespace Game.HealthSystem
{
    [System.Serializable]
    //This health system is used for Fill Amount Health Bar
    public class IntHealthSystem : HealthSystem
    {
        #region Variables
        [SerializeField] private float _current;
        [SerializeField] private float _maximum;
        private float _minimum = 0;
        #endregion


        #region Getters And Setters
        public float Current => _current;
        public float Minimum => _minimum;
        public float Maximum => _maximum;

        public override float Health01 => ((_current - _minimum) / (_maximum - _minimum));
        public override float HealthPercent => ((_current - _minimum) / (_maximum - _minimum)) * 100;
        #endregion


        #region Class Functions
        public override void Damage(float damageAmount)
        {
            _current -= damageAmount;
            if (_current <= _minimum)
            {
                _current = _minimum;
                if (!_isDead)
                {
                    _isDead = true;
                    OnDead?.Invoke();
                }
            }
            OnDamaged?.Invoke(damageAmount);
        }

        public override void Heal(float healAmount)
        {
            _current += healAmount;
            if (_current > _maximum) _current = _maximum;
            OnHealed?.Invoke(healAmount);
        }

        public override void Reset()
        {
            base.Reset();
            _current = _maximum;
        }
        #endregion


        #region Constructors
        public IntHealthSystem(float maximum = 100, float minimum = 0, float current = 100)
        {
            _minimum = minimum;
            _maximum = maximum;
            _current = current;
        }
        #endregion
    }
}