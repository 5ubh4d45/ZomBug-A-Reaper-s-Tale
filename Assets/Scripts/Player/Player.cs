using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.HealthSystem;
using Game.Core;
using System;
using Game.Levels;
using FMODUnity;


//this class will contain all the functionalities
public class Player : HealthObject<HeartHealthSystem>
{
    private static Player _instance;
    public static Player Instance => _instance;


    [Header("CameraShake")]
    [SerializeField] private float camShakeIntensity = 1f;
    [SerializeField] private float camShakeFrequency = 3f;
    [SerializeField] private float camShakeTime = 0.2f;

    [Space]
    [SerializeField] private float moveSpeed = 10;

    [SerializeField] private float deathAnimationTime;

    private PlayerCombat _playerCombat;
    private PlayerAnimator _playerAnimator;
    private PlayerMovement _playerMovement;
    private SpriteRenderer _renderer;
    private CameraShake _cameraShake => CameraShake.Instance;

    private float _meleeDamageCoolDown = 1f;
    private float _nextMeleeDamageTime = 0f;
    private bool _isDead = false;

    public float MoveSpeed => moveSpeed;
    public IInteractable Interactable { get; set; }
    public PlayerCombat PlayerCombat => _playerCombat;
    public PlayerAnimator PlayerAnimator => _playerAnimator;
    public PlayerMovement PlayerMovement => _playerMovement;

    public bool IsDead => _isDead;

    public void Reset()
    {
        _healthSystem.Heal(_healthSystem.MaxHealth);
        PlayerCombat.Reset();
    }

    protected override void Awake()
    {
        base.Awake();
        _instance = this;
        GameManager.Instance.OnGameStateChanged += UpdateState;
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.enabled = false;
    }

    private void UpdateState()
    {
        if (GameManager.Instance.GameState == GameState.GAME)
        {
            _renderer.enabled = true;
        }
        else
        {
            _renderer.enabled = false;
        }
    }

    private void Start()
    {
        _playerCombat = GetComponent<PlayerCombat>();
        _playerAnimator = GetComponent<PlayerAnimator>();
        _playerMovement = GetComponentInChildren<PlayerMovement>();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {

            if (Interactable != null)
            {
                Interactable.Interact(this);
            }
        }

        _nextMeleeDamageTime += Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (_nextMeleeDamageTime >= _meleeDamageCoolDown)
        {
            if (collision2D.gameObject.CompareTag("Enemy"))
            {
                _healthSystem.Damage(collision2D.gameObject.GetComponent<EnemyCombat>().MeleeDamage);

                _nextMeleeDamageTime = 0f;

                //starts the dmg effect
                StartCoroutine(DamageEffect());

                //adds cam shake when damage taken
                CameraShake.Instance.ShakeCamera(camShakeIntensity, camShakeFrequency, camShakeTime);
            }

        }

    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Health Point"))
        {
            _healthSystem.Heal(1);

        }
    }

    public override void OnDead()
    {
        //playing the death effects
        StartCoroutine(DeathEffects());
        
        
        GameManager.Instance.DidWon = false;
        
    }

    private IEnumerator DeathEffects()
    {
        _isDead = true;
        _playerAnimator.PlayDeath();
        StartCoroutine(_playerMovement.StopMovement(deathAnimationTime));

        // disabling all colliders
        GetComponent<Collider2D>().enabled = false;
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
        
        yield return new WaitForSeconds(deathAnimationTime - 0.01f);

        // enabling colliders after death animation
        _isDead = false;

        GetComponent<Collider2D>().enabled = true;
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }
        
        _renderer.enabled = false;
        LevelSceneManager.Instance.LoadEndScreen(false);
        
    }
    
    public IEnumerator DamageEffect()
    {
        //plays sound of player being hurt
        RuntimeManager.PlayOneShot("event:/SFX_player_hurt");
        
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
    
}
