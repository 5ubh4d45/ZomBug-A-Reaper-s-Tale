using System.Collections;
using System.Collections.Generic;
using Game.Combat;
using Pathfinding.Ionic.Zlib;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{   
    [SerializeField] private GameObject enemyProjectile;
    [SerializeField] private EnemyAnimator enemyAnimator;

    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate;
    [SerializeField] private float attackDamage;

    [SerializeField] private float meleeRate;
    [SerializeField] private float meleeDamage;

    
    private Enemy _enemy;
    private float _nextFireTime = 0f;
    private float _nextMeleeTime = 0f;
    private string _attackTag = "Player";
    
   
    void Start()
    {
        _enemy = GetComponent<Enemy>();
        if (enemyAnimator == null)
        {
            enemyAnimator = GetComponent<EnemyAnimator>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        _nextMeleeTime += Time.deltaTime;
        _nextFireTime += Time.deltaTime;
    }

    public void RangedAttack(Vector3 bulletPosition, Vector3 targetPosition)
    {
        if (_nextFireTime >= fireRate)
        {   
            enemyAnimator.PlayRangedAttack();
            GameObject bullet = Instantiate(enemyProjectile, firePoint.position, Quaternion.identity);
            
            bullet.GetComponent<Projectile>().Initialise(attackDamage, targetPosition, _attackTag);
            _nextFireTime = 0f;
        }
    }

    public void MeleeAttack()
    {
        if (_nextMeleeTime >= meleeRate)
        {
            enemyAnimator.PlayMeleeAttack();
            
            Debug.Log("Played Enemy Melee Attack");
            
            _nextMeleeTime = 0f;
            

        }
    }

}
