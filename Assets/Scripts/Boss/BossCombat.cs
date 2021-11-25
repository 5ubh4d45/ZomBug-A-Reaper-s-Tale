using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : MonoBehaviour
{
    [Header("Required Componenets")]
    [SerializeField] private CatBoss boss;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject slashProjectile;
    [SerializeField] private GameObject skullProjectile;
    
    [Space][Header("Ranges")]
    [SerializeField] private float minMeleeDistance;
    [SerializeField] private float maxMeleeDistance;
    [SerializeField] private float minRangedDistance;
    [SerializeField] private float maxRangedDistance;

    [Space] [Header("Combat Variables")]
    [SerializeField] private float jumpAttackRate;
    
    [SerializeField] private float meleeFireRate;
    [SerializeField] private int meleeDamage;
    
    [SerializeField] private float slashFireRate;
    [SerializeField] private int slashDamage;
    [SerializeField] private float skullFireRate;
    [SerializeField] private int skullDamage;
    
    
    //private floats & vectors
    private float _nextJumpAttackTime = 0f;
    private float _nextMeleeFireTime = 0f;
    private float _nextSlashFireTime = 0f;
    private float _nextSkullFireTime = 0f;
    
    private Vector3 _aimDirection;
    
    //private bools
    private bool _inRange;
    private bool _inAttackRange;
    private bool _inBackOffRange;
    private bool _inMeleeRange;

    // getters
    public Vector3 AimDirection => _aimDirection;
    
    public bool InRange => _inRange;
    public bool InAttackRange => _inAttackRange;
    public bool InBackOffRange => _inBackOffRange;
    public bool InMeleeRange => _inMeleeRange;

    public int MeleeDamage => meleeDamage;
    public int SlashDamage => slashDamage;
    public int SkullDamage => skullDamage;
    
    public float MinMeleeDistance => minMeleeDistance;
    public float MaxMeleeDistance => maxMeleeDistance;
    public float MinRangedDistance => minRangedDistance;
    public float MaxRangedDistance => maxRangedDistance;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //setting up components
        if (boss == null)
        {
            boss = GetComponent<CatBoss>();
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        CheckParameters();
        CheckRange();
    }

    private void CheckRange()
    {
        float distance = boss.BossMovement.CurrentDistance;
        
        _inRange = distance < maxRangedDistance;
        _inAttackRange = distance <= maxRangedDistance && distance >= minRangedDistance;
        _inBackOffRange = distance <= minRangedDistance && distance >= maxMeleeDistance;
        _inMeleeRange = distance <= maxMeleeDistance && distance >= minMeleeDistance;
    }

    private void CheckParameters()
    {
        Vector3 targetPos = boss.Target.position;
        
        //checks for aim direction
        _aimDirection = (targetPos - firePoint.position);
        
        //all cooldowns
        _nextJumpAttackTime += Time.deltaTime;
        _nextMeleeFireTime += Time.deltaTime;
        _nextSkullFireTime += Time.deltaTime;
        _nextSlashFireTime += Time.deltaTime;
    }

    public void MeleeAttack()
    {
        if (_nextMeleeFireTime >= meleeFireRate)
        {
            boss.BossAnimator.PlayMeleeAttack();
        
            Debug.Log("played melee");
            

            _nextMeleeFireTime = 0f;
        }
        
        var backoffImpulse = boss.BossMovement.BackoffImpulse();
        StartCoroutine(backoffImpulse);
       
    }

    public void JumpAttack()
    {
        boss.BossAnimator.PlayJumpAttack();
    }
    
    public void RangedAttack()
    {
        
    }

    public void CanRangeAttack()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        
        Gizmos.DrawWireSphere(transform.position, minRangedDistance);
        Gizmos.DrawWireSphere(transform.position, maxRangedDistance);
        
        Gizmos.DrawWireSphere(transform.position, minMeleeDistance);
        Gizmos.DrawWireSphere(transform.position, maxMeleeDistance);
    }
}
