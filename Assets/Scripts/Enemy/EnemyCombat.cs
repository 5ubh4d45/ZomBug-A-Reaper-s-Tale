using System.Collections;
using System.Collections.Generic;
using Game.Combat;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{   
    [SerializeField] private GameObject enemyProjectile;
    
    private Enemy _enemy;
    [SerializeField] private float fireRate;
    [SerializeField] private float attackDamage;
    // private float _meleeAttackSpeed;
    // private float _canMeleeAttack = 0f;
    private float _nextFireTime = 0f;
    private string _attackTag = "Player";
    
   
    void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        fireRate -= Time.deltaTime;
    }

    public void RangedAttack(Vector3 bulletPosition, Vector3 targetPosition)
    {
        if (fireRate > 0)
        {
            GameObject bullet = Instantiate(enemyProjectile);
            
            bullet.GetComponent<Projectile>().Initialise(attackDamage, targetPosition, _attackTag);
            fireRate = _nextFireTime;
        }
    }

}
