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
        public float HealthPercent { get => ((_current - _minimum) / (_maximum - _minimum)) * 100; }
        public float Health01 { get => ((_current - _minimum) / (_maximum - _minimum)); }
        #endregion


        #region Class Functions
        public override void Damage(float damageAmount)
        {
            _current -= damageAmount;
            if (_current <= _minimum)
            {
                _current = _minimum;
                if (!IsDead)
                {
                    IsDead = true;
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