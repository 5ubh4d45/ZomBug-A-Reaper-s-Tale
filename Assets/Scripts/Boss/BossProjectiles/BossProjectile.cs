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
    [SerializeField] private float _aliveTime;
    [SerializeField] private float blastDuration = 0.3f;
    [SerializeField] private float impactForce;
    [SerializeField] private Animator anim;
    [SerializeField] private float introAnimationWaitTime;
    [SerializeField] private bool isHoming;
    [SerializeField] private float homingSpeed;
    [SerializeField] private Collider2D col;
    
    private float _damage;
    private Vector2 _direction;
    private string _attackTag;
    private Transform _targetPos;
    private float _speedModifier = 1f;
    #endregion


    #region Getters And Setters

    #endregion

    private void Start()
    {
        col = GetComponent<Collider2D>();
        col.enabled = false;
        
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
    }

    #region Unity Calls
    private void FixedUpdate()
    {
        ProjectileFlip();
        StartCoroutine(MoveProjectile());
        
    }

    private IEnumerator MoveProjectile()
    {
        yield return new WaitForSeconds(introAnimationWaitTime + 0.2f);

        col.enabled = true;
        
        //checks if if its homing or not
        if (isHoming)
        {
            transform.right = DirectionToVector(_targetPos.position);

            transform.position =
                Vector2.MoveTowards(transform.position, 
                    _targetPos.position, homingSpeed * Time.fixedDeltaTime * _speedModifier);
            
            yield break;
        }
        
        transform.right = (Vector3)_direction;
        
        transform.position += (Vector3)_direction * (speed * Time.deltaTime) * _speedModifier;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HealthObject healthObject = collision.gameObject.GetComponent<HealthObject>();

        //replaced layermask with compareTags 
        if (healthObject != null && collision.gameObject.CompareTag(_attackTag))
        {
            healthObject.HealthSystem.Damage(_damage);
            
                //adds cam shake when damage taken
                CameraShake.Instance.ShakeCamera(3f, 3f, 0.2f);
                    
                var player = collision.gameObject.GetComponent<Player>();
                    
                //plays the dmg effect
                StartCoroutine(player.DamageEffect());

                //adding a force to impact
            collision.gameObject.GetComponentInParent<Rigidbody2D>().AddForce(_direction * impactForce, ForceMode2D.Impulse);
            
            
        }
        //destrying and playing the blast
        StartCoroutine(DestroyBullet(0f));
    }
    #endregion


    #region Component Functions
    public void Initialise(float damage, Transform targetPos, string attacktag)
    {
        _damage = damage;
        _attackTag = attacktag;
        _targetPos = targetPos;
        _direction = DirectionToVector(_targetPos.position);
        
        StartCoroutine(DestroyBullet(_aliveTime));
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
    
    private IEnumerator DestroyBullet(float delay)
    {
        yield return new WaitForSeconds(delay);
            
        //destrying and playing the blast
        anim.SetTrigger("Blast");
        _speedModifier = 0f;
        Destroy(gameObject, blastDuration);
    }
    #endregion
}
