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
            target = FindObjectOfType<Player>().gameObject.transform;
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
        LevelManager.Instance.UnregisterEnemy();
        Destroy(this.gameObject);
    }

    public override void OnDamaged(float damageAmount)
    {
        ScoreManager.Instance.AddScore(this.ScorePerHit);
        
        StartCoroutine(DamageEffect());
    }
    
    private IEnumerator DamageEffect()
    {
        //adds cam shake when damage taken
        CameraShake.Instance.ShakeCamera(3f, 3f, 0.2f);
        
        var sprtRnd = GetComponent<SpriteRenderer>();
        float flashDelay = 0.05f;
        
        sprtRnd.color = Color.red;
        yield return new WaitForSeconds(flashDelay);
        
        sprtRnd.color = new Color(255f, 255, 255f, 255f);
        yield return new WaitForSeconds(flashDelay);
        
        sprtRnd.color = Color.red;
        yield return new WaitForSeconds(flashDelay);

        sprtRnd.color = new Color(255f, 255, 255f, 255f);

    }

}
