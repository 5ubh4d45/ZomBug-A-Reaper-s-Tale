using System.Collections;
using EnemyBehavior;
using UnityEngine;
using Game.HealthSystem;
using Game.Score;
using Game.Pointer;
using Game.Levels;

public class Enemy : HealthObject<IntHealthSystem>
{
    #region Variables
    [Tooltip("The Amount of Score to be added for every enemy of this type")]
    [SerializeField] private int _scorePerHit;
    
    [Space]
    // reference to the sound holder use _soundHolder.(your sound string variable)
    // at the Fmod sound string like
    // RuntimeManager.PlayOneShot(soundHolder.DeathSound);
    [SerializeField] private EnemySoundHolder soundHolder;

    private float _delay;
    #endregion


    #region Getters And Setters
    public int ScorePerHit => _scorePerHit;
    #endregion


    #region Unity Calls
    protected override void Awake()
    {
        base.Awake();
        LevelManager.Instance.RegisterEnemy();

        if (soundHolder == null)
        {
            soundHolder = GetComponent<EnemySoundHolder>();
        }
    }
    #endregion


    #region Component Functions
    public override void OnDead()
    {
        PointerManager.Instance.SetDefaultCursor();
        
        DeadSetUp();

        LevelManager.Instance.UnregisterEnemy();
        Destroy(this.gameObject, _delay);
        
    }

    public override void OnDamaged(float damageAmount)
    {
        ScoreManager.Instance.AddScore(_scorePerHit);

        StartCoroutine(DamageEffect());
    }

    private void DeadSetUp()
    {
        
        //playing the death animation and gets the death animation duration
        var anim = GetComponent<EnemyAnimator>();
        var behavior = GetComponent<EnemyBehavior.EnemyBehavior>();
        
        //puts the enemy into dead state
        anim.PlayDeathAnimation();
        behavior._state = EnemyBehavior.EnemyBehavior.EnemyState.Dead;
        
        _delay = anim.DeathAnimationTime;
        
        GetComponent<Collider2D>().enabled = false;
        var collider2Ds = GetComponentsInChildren<Collider2D>();
        foreach (var collider2D in collider2Ds)
        {
            collider2D.enabled = false;
        }
    }

    private IEnumerator DamageEffect()
    {
        //adds cam shake when damage taken
        CameraShake.Instance.ShakeCamera(3f, 3f, 0.2f);
        
        var sprtRnd = GetComponent<SpriteRenderer>();
        float flashDelay = 0.06f;
        
        sprtRnd.color = Color.red;
        yield return new WaitForSeconds(flashDelay);
        
        sprtRnd.color = new Color(255f, 255, 255f, 255f);
        yield return new WaitForSeconds(flashDelay);
        
        sprtRnd.color = Color.red;
        yield return new WaitForSeconds(flashDelay);

        sprtRnd.color = new Color(255f, 255, 255f, 255f);

    }
    #endregion
}