using System;
using UnityEngine;

namespace Game.HealthSystem
{
    /// <summary>
    /// This class should only be used for referencing health objects and 
    /// not for inheriting a health object, for that you should inherit the HealthObject<T> Class
    /// </summary>
    public class HealthObject : MonoBehaviour
    {
        #region Variables
        protected private HealthSystem _healthSystem;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        #endregion


        #region Component Functions
        public virtual void OnDead()
        {

        }

        public virtual void OnDamaged(float damageAmount)
        {

        }
        public virtual void OnHealed(float healAmount)
        {

        }
        #endregion
    }

    /// <summary>
    /// This class Should Be inherited by Classes that represent objects with health bars
    /// </summary>
    public class HealthObject<T> : HealthObject where T : HealthSystem
    {
        #region Variables
        [SerializeField] protected private HealthBar<T> _healthBar;
        [SerializeField] protected private new T _healthSystem;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls

        protected virtual void Awake()
        {
            _healthBar.Setup(_healthSystem);
            _healthSystem.OnDamaged += OnDamaged;
            _healthSystem.OnHealed += OnHealed;
            _healthSystem.OnDead += OnDead;
        }
        #endregion


        #region Component Functions
        #endregion
    }
}