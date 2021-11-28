using System;
using System.Collections;
using Game.HealthSystem;
using UnityEngine;

namespace Game.Combat
{
    /// <summary>
    /// This script is attached to objects that are supposed to be projectiles emmitted from ranged weapons
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        
        #region Variables
        [SerializeField] private float _speed;
        [SerializeField] private float _aliveTime;
        [SerializeField] private float impactForce;
        [SerializeField] private Animator anim;
        
        private float _damage;
        private Vector3 _direction;
        private LayerMask _attackLayer;
        private string _attackTag;
        private float _speedModifier = 1f;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls

        private void Start()
        {
            if (anim == null)
            {
                anim = GetComponent<Animator>();
            }
        }

        private void Update()
        {
            transform.position += _direction * (_speed * Time.deltaTime) * _speedModifier;
            
            //pointing the bullet at correct direction
            transform.right = _direction;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            HealthObject healthObject = collision.gameObject.GetComponent<HealthObject>();
            
            // if (healthObject != null && (_attackLayer.value & (1 << collision.gameObject.layer)) > 0)
            
            //replaced layermask with compareTags 
            if (healthObject != null && collision.gameObject.CompareTag(_attackTag))
            {
                healthObject.HealthSystem.Damage(_damage);
                // healthObject.HealthSystem().Damage(_damage);
                //adding a force to impact
                collision.gameObject.GetComponentInParent<Rigidbody2D>().AddForce(_direction * impactForce, ForceMode2D.Impulse);
                
                
            }
            
            //destrying and playing the blast
            StartCoroutine(DestroyBullet(0f));
        }
        #endregion


        #region Component Functions
        public void Initialise(float damage, Vector3 target, string attacktag)
        {
            _damage = damage;
            _attackTag = attacktag;
            _direction = DirectionToVector(target);

            StartCoroutine(DestroyBullet(_aliveTime));
        }

        private Vector2 DirectionToVector(Vector2 target)
        {
            return ((Vector3)target - transform.position).normalized;
        }

        private IEnumerator DestroyBullet(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            //destrying and playing the blast
            anim.SetTrigger("Blast");
            _speedModifier = 0f;
            Destroy(gameObject, 0.07f);
        }
        
        #endregion
    }
}