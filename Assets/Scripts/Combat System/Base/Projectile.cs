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
        [SerializeField] private float impactForce;
        
        private float _damage;
        private Vector2 _direction;
        private LayerMask _attackLayer;
        private string _attackTag;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        private void Update()
        {
            transform.position += (Vector3)_direction * (_speed * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            HealthObject healthObject = collision.gameObject.GetComponent<HealthObject>();
            
            // if (healthObject != null && (_attackLayer.value & (1 << collision.gameObject.layer)) > 0)
            
            //replaced layermask with compareTags 
            if (healthObject != null && collision.gameObject.CompareTag(_attackTag))
            {
                healthObject.HealthSystem().Damage(_damage);
                
                //adding a force to impact
                collision.gameObject.GetComponentInParent<Rigidbody2D>().AddForce(_direction * impactForce, ForceMode2D.Impulse);
                
            }
            Destroy(gameObject);
        }
        #endregion


        #region Component Functions
        public void Initialise(float damage, Vector2 target, string attacktag)
        {
            _damage = damage;
            _attackTag = attacktag;
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