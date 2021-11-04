using UnityEngine;

namespace Game.HealthSystem
{
    /// <summary>
    /// This Class should only be used for referencing different 
    /// types of health bar and should not be inherited by any type of Health bar
    /// </summary>
    public abstract class HealthBar : MonoBehaviour
    {
        protected private HealthSystem _healthSystem;
        public abstract void Setup(HealthSystem healthSystem);
    }

    /// <summary>
    /// This class should bu used for referencing to a very specific type of health bar 
    /// It's main purpose is to be inherited by different types of health bars
    /// </summary>
    public abstract class HealthBar<T> : HealthBar where T : HealthSystem
    {
        protected private new T _healthSystem;
        public abstract void Setup(T healthSystem);
        public override void Setup(HealthSystem healthSystem)
        {
            this.Setup(healthSystem as T);
        }
    }
}