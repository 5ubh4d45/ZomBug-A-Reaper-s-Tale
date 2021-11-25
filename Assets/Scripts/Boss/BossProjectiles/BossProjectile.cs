using System;
using System.Collections;
using System.Collections.Generic;
using Game.Combat;
using UnityEngine;
using Game.HealthSystem;
public class BossProjectile : MonoBehaviour
{

    #region Variables
    [SerializeField] private float speed;
    [SerializeField] private float aliveTime;
    [SerializeField] private float impactForce;
    [SerializeField] private float introAnimationWaitTime;
    [SerializeField] private bool isHoming;
    [SerializeField] private float homingSpeed;
    [SerializeField] private Collider2D col;
    
    private float _damage;
    private Vector2 _direction;
    private string _attackTag;
    private Transform _targetPos;
    #endregion


    #region Getters And Setters

    #endregion

    private void Start()
    {
        col = GetComponent<Collider2D>();
        col.enabled = false;
    }

    #region Unity Calls
    private void FixedUpdate()
    {
        ProjectileFlip();
        StartCoroutine(MoveProjectile());
        
    }

    private IEnumerator MoveProjectile()
    {
        yield return new WaitForSeconds(introAnimationWaitTime);

        col.enabled = true;
        
        //checks if if its homing or not
        if (isHoming)
        {
            transform.right = DirectionToVector(_targetPos.position);

            transform.position =
                Vector2.MoveTowards(transform.position, 
                    _targetPos.position, homingSpeed * Time.fixedDeltaTime);
            
            yield break;
        }
        
        transform.right = (Vector3)_direction;
        
        transform.position += (Vector3)_direction * (speed * Time.deltaTime);
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
        Destroy(gameObject);
    }
    #endregion


    #region Component Functions
    public void Initialise(float damage, Transform targetPos, string attacktag)
    {
        _damage = damage;
        _attackTag = attacktag;
        _targetPos = targetPos;
        _direction = DirectionToVector(_targetPos.position);
        Destroy(gameObject, aliveTime);
    }

    private Vector2 DirectionToVector(Vector2 target)
    {
        return ((Vector3)target - transform.position).normalized;
    }

    private void ProjectileFlip()
    {
        //is on right
        if (DirectionToVector(_targetPos.position).x > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, -1f, 1f);
        }
    }
    #endregion
}
