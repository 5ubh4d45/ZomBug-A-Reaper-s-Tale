using System;
using Game.HealthSystem;
using UnityEngine;

namespace Game.Combat
{
    public class Projectile : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float _speed;
        [SerializeField] private float _aliveTime;
        private float _damage;
        private Vector2 _direction;
        private LayerMask _attackLayer;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        private void Update()
        {
            transform.position += (Vector3)_direction * _speed * Time.deltaTime;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            HealthObject healthObject = collision.gameObject.GetComponent<HealthObject>();
            if (healthObject != null)
            {
                healthObject.HealthSystem().Damage(_damage);
            }
            Destroy(gameObject);
        }
        #endregion


        #region Component Functions
        public void Initialise(float damage, Vector2 target, LayerMask attackLayer)
        {
            _damage = damage;
            _attackLayer = attackLayer;
            _direction = DirectionToVector(target);
            Destroy(gameObject, _aliveTime);
        }

        private Vector2 DirectionToVector(Vector2 target)
        {
            return ((Vector3)target - transform.position).normalized;
        }
        #endregion
    }
}