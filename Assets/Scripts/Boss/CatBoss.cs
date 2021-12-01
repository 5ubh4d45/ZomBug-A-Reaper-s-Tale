using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.HealthSystem;
using Game.Score;
using Game.Pointer;
using Game.Levels;

public class CatBoss : Enemy
{
    [Header("Required Componenets")]
    [SerializeField] private BossMovement bossMovement;
    [SerializeField] private BossAnimator bossAnimator;
    [SerializeField] private BossCombat bossCombat;
    [SerializeField] private BossBehavior bossBehavior;

    [Space] [SerializeField] private Transform target;
    [SerializeField] private SpriteRenderer _sprtRnd;


    #region Getters

    public BossAnimator BossAnimator => bossAnimator;
    public BossMovement BossMovement => bossMovement;
    public BossCombat BossCombat => bossCombat;
    public BossBehavior BossBehavior => bossBehavior;


    public Transform Target => target;

    #endregion
    

    // Start is called before the first frame update
    void Start()
    {
        //setting up components
        if (bossAnimator == null)
        {
            bossAnimator = GetComponent<BossAnimator>();
        }
        if (bossMovement == null)
        {
            bossMovement = GetComponent<BossMovement>();
        }
        if (bossCombat == null)
        {
            bossCombat = GetComponent<BossCombat>();
        }

        if (bossBehavior == null)
        {
            bossBehavior = GetComponent<BossBehavior>();
        }

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (_sprtRnd == null)
        {
            _sprtRnd = GetComponent<SpriteRenderer>();
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FlipCheck();
    }

    private void FlipCheck()
    {
        if (bossMovement.IsFacingRight)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        if (!bossMovement.IsFacingRight)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    public override void OnDead()
    {
        PointerManager.Instance.SetDefaultCursor();
        
        DeadSetUp();
        
        Destroy(this.gameObject, bossAnimator.DeathAnimationTime);
    }

    public override void OnDamaged(float damageAmount)
    {
        ScoreManager.Instance.AddScore(this.ScorePerHit);
        
        StartCoroutine(DamageEffect());
        
    }

    private IEnumerator DeadSetUp()
    {
        //plays the death animation and stops all movements of the boss
        Debug.Log("Playing Boss death");
        bossAnimator.PlayDeathAniamtion();
        bossBehavior._state = BossBehavior.BossState.Dead;
        
        //disables the healthbar
        _healthBar.gameObject.SetActive(false);

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().mass = 1000f;
        
        var collider2Ds = GetComponentsInChildren<Collider2D>();
        foreach (var collider2D in collider2Ds)
        {
            collider2D.enabled = false;
        }

        yield return new WaitForSeconds(bossAnimator.DeathAnimationTime);
        
        LevelManager.Instance.UnregisterEnemy();
        
    }
    
    private IEnumerator DamageEffect()
    {
        Debug.Log("lPlaying boss damage effect");

        //adds cam shake when damage taken
        CameraShake.Instance.ShakeCamera(3f, 3f, 0.2f);
        
        float flashDelay = 0.05f;
        
        _sprtRnd.color = Color.red;
        yield return new WaitForSeconds(flashDelay);
        
        _sprtRnd.color = new Color(255f, 255, 255f, 255f);
        yield return new WaitForSeconds(flashDelay);
        
        _sprtRnd.color = Color.red;
        yield return new WaitForSeconds(flashDelay);

        _sprtRnd.color = Color.white;

    }

}
