using System;
using System.Collections;
using System.Collections.Generic;
using Game.Combat;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using FMODUnity;

public class BossCombat : MonoBehaviour
{
    [Header("Required Componenets")]
    [SerializeField] private CatBoss boss;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject slashProjectile;
    [SerializeField] private GameObject skullProjectile;

    [Space]
    [Header("Ranges")]
    [SerializeField] private float minMeleeDistance;
    [SerializeField] private float maxMeleeDistance;
    [SerializeField] private float minRangedDistance;
    [SerializeField] private float maxRangedDistance;

    [Space]
    [Header("Combat Variables")]
    [SerializeField] private float jumpAttackRate;

    [SerializeField] private float meleeFireRate;
    [SerializeField] private int meleeDamage;

    [SerializeField] private float slashFireRate;
    [SerializeField] private int slashDamage;
    [SerializeField] private float skullFireRate;
    [SerializeField] private int skullDamage;
    
    [Space]
    // reference to the sound holder use _soundHolder.(your sound string variable)
    // at the Fmod sound string like
    // RuntimeManager.PlayOneShot(soundHolder.DeathSound);
    [SerializeField] private EnemySoundHolder soundHolder;

    private string _attackTag = "Player";

    //private floats & vectors
    private float _nextJumpAttackTime = 0f;
    private float _nextMeleeFireTime = 0f;
    private float _nextSlashFireTime = 0f;
    private float _nextSkullFireTime = 0f;

    private Vector3 _aimDirection;
    private Vector3 targetPos;

    //private bools
    private bool _inRange;
    private bool _inAttackRange;
    private bool _inBackOffRange;
    private bool _inMeleeRange;
    private bool _canRangeAttack;

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
        if (soundHolder == null)
        {
            soundHolder = GetComponent<EnemySoundHolder>();
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
        targetPos = boss.Target.position;

        //checks for aim direction
        _aimDirection = (targetPos - firePoint.position).normalized;

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
        if (_nextJumpAttackTime <= jumpAttackRate) return;

        var stpMv = boss.BossMovement.StopMovement(1f);
        StartCoroutine(stpMv);

        boss.BossAnimator.PlayJumpAttack();

        _nextJumpAttackTime = 0f;
    }


    //this attack in played on the animation itself
    public void JumpedRangedAttack()
    {
        //skull chances are big
        int rnd = Random.Range(0, 3);
        if (rnd < 1)
        {
            //play sound for SlashAttack
            RuntimeManager.PlayOneShot("event:/SFX_cat_slash");

            // no delay as the attack played by animator
            StartCoroutine(SlashAttack(0.0f));
            StartCoroutine(SlashAttack(0.1f));
            StartCoroutine(SlashAttack(0.2f));

        }
        else
        {
            //play  sound for SkullAttack
            RuntimeManager.PlayOneShot("event:/SFX_cat_skull");

            // no delay as the attack played by animator
            StartCoroutine(SkullAttack(0.0f));
            StartCoroutine(SkullAttack(0.1f));
            StartCoroutine(SkullAttack(0.2f));

        }
    }


    public void RangedAttack()
    {
        // slash chances are big
        int rnd = Random.Range(0, 10);
        if (rnd > 0)
        {
            if (_nextSlashFireTime <= slashFireRate) return;

            //play sound for SlashAttack
            RuntimeManager.PlayOneShot("event:/SFX_cat_slash");

            boss.BossAnimator.PlayMeleeAttack();

            //adds the delay to sync
            StartCoroutine(SlashAttack(0.5f));

            _nextSlashFireTime = 0f;

        }
        else
        {

            if (_nextSlashFireTime <= slashFireRate) return;

            //play  sound for SkullAttack
            RuntimeManager.PlayOneShot("event:/SFX_cat_skull");

            boss.BossAnimator.PlayMeleeAttack();

            //adds the delay to sync
            StartCoroutine(SkullAttack(0.5f));

            _nextSlashFireTime = 0f;
        }
    }


    private IEnumerator SlashAttack(float delay)
    {
        //wait for a delay
        yield return new WaitForSeconds(delay);

        GameObject slash = Instantiate(slashProjectile, firePoint.position, Quaternion.identity);

        slash.GetComponent<BossProjectile>().Initialise(slashDamage, boss.Target, _attackTag);

    }

    private IEnumerator SkullAttack(float delay)
    {
        //wait for a delay
        yield return new WaitForSeconds(delay);

        GameObject slash = Instantiate(skullProjectile, firePoint.position, Quaternion.identity);

        slash.GetComponent<BossProjectile>().Initialise(slashDamage, boss.Target, _attackTag);

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
