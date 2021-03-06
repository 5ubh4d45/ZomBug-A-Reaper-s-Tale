using System;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace Game.HealthSystem
{
    [Serializable]
    /// <summary>
    /// The Health System used in the heart based health bar
    /// </summary>
    public class HeartHealthSystem : HealthSystem
    {
        #region Variables
        [SerializeField] private int _numberOfHearts;
        [SerializeField] private int _maxFragments;
        [SerializeField] private int _health;
        private List<Heart> _hearts;
        #endregion


        #region Getters And Setters
        public override float MaxHealth => _numberOfHearts * _maxFragments;
        public int HeartCount => _numberOfHearts;
        public int Health
        {
            get
            {
                if (_hearts == null) SetupHearts(_maxFragments);
                int health = 0;
                for (int i = 0; i < _hearts.Count; i++)
                {
                    health += _hearts[i].Fragments;
                }
                return health;
            }
        }
        public List<Heart> Hearts => _hearts;

        public override float HealthPercent => Health / (float)MaxHealth * 100;
        public override float Health01 => Health / (float)MaxHealth;
        #endregion


        #region Class Functions
        public override void Damage(float damageAmount)
        {
            int damage = Mathf.RoundToInt(damageAmount);
            int damageLeft = damage;

            for (int i = _hearts.Count - 1; i >= 0; i--)
            {
                Heart heart = _hearts[i];
                int heartFrags = heart.Fragments;
                if (damageLeft > heart.Fragments)
                {
                    heart.Damage(damageLeft);
                    damageLeft -= heartFrags;
                }
                else if (damageLeft <= heart.Fragments)
                {
                    heart.Damage(damageLeft);
                    damageLeft = 0;
                    break;
                }
            }

            _health -= damage;
            if (_health <= 0)
            {
                _health = 0;
                Debug.Log(_isDead);
                if (!_isDead)
                {
                    OnDead?.Invoke();
                    _isDead = true;
                }
            }
            OnDamaged?.Invoke(damage);
        }

        public override void Heal(float healAmount)
        {
            int heal = Mathf.RoundToInt(healAmount);
            int healLeft = heal;

            for (int i = 0; i < _hearts.Count; i++)
            {
                Heart heart = _hearts[i];
                int missingFrags = heart.MaxFragments - heart.Fragments;
                if (healLeft > missingFrags)
                {
                    healLeft -= missingFrags;
                    heart.Heal(missingFrags);
                }
                else
                {
                    healLeft = 0;
                    heart.Heal(heal);
                    break;
                }
            }

            _health += heal;
            if (_health > MaxHealth) _health = (int)MaxHealth;
            OnHealed?.Invoke(heal);
        }

        public override void Reset()
        {
            base.Reset();
            _isDead = false;
            Heal(MaxHealth);
            Debug.Log("Healing Player After Death" + _health);
        }
        #endregion


        #region Constructors
        public HeartHealthSystem(int numberOfHearts, int maxFragments)
        {
            _maxFragments = maxFragments;
            _numberOfHearts = numberOfHearts;
            _health = (int)MaxHealth;

            SetupHearts(maxFragments);
        }

        public HeartHealthSystem(int numberOfHearts, int health, int maxFragments)
        {
            _maxFragments = maxFragments;
            _numberOfHearts = numberOfHearts;
            _health = health;

            SetupHearts(maxFragments);
        }

        private void SetupHearts(int maxFragments)
        {
            _hearts = new List<Heart>();
            for (int i = 0; i < _numberOfHearts; i++)
            {
                Heart heart = new Heart(maxFragments);
                _hearts.Add(heart);
            }
        }
        #endregion
    }

    /// <summary>
    /// Class Representing a Single Heart
    /// </summary>
    public class Heart
    {
        private int _fragments;
        private int _maxFragments;

        public int Fragments => _fragments;
        public int MaxFragments => _maxFragments;

        public Heart(int maxFragments)
        {
            _maxFragments = maxFragments;
            _fragments = _maxFragments;
        }

        public Heart(int maxFragments, int fragments)
        {
            _maxFragments = maxFragments;
            _fragments = fragments;
        }

        public void Damage(int damageAmount)
        {
            if (damageAmount > _fragments)
            {
                _fragments = 0;
            }
            else if (damageAmount <= _fragments)
            {
                _fragments -= damageAmount;
            }
        }

        public void Heal(int healAmount)
        {
            if (_fragments + healAmount > _maxFragments)
            {
                _fragments = _maxFragments;
            }
            else
            {
                _fragments += healAmount;
            }
        }
    }
}