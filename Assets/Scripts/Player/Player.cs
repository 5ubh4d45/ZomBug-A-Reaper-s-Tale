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

    private PlayerCombat _playerCombat;
    private PlayerAnimator _playerAnimator;
    private PlayerMovement _playerMovement;
    private SpriteRenderer _renderer;
    private CameraShake _cameraShake => CameraShake.Instance;

    private float _meleeDamageCoolDown = 1f;
    private float _nextMeleeDamageTime = 0f;

    public float MoveSpeed => moveSpeed;
    public IInteractable Interactable { get; set; }
    public PlayerCombat PlayerCombat => _playerCombat;
    public PlayerAnimator PlayerAnimator => _playerAnimator;
    public PlayerMovement PlayerMovement => _playerMovement;

    public void Reset()
    {
        _healthSystem.Heal(_healthSystem.MaxHealth);
        PlayerCombat.Reset();
    }

    protected override void Awake()
    {
        base.Awake();
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
        _instance = this;
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
                _healthSystem.Damage(1);
                
                _nextMeleeDamageTime = 0f;
                
                //plays sound of player being hurt
                RuntimeManager.PlayOneShot("event:/SFX_player_hurt");

                //adds cam shake when damage taken
                _cameraShake.ShakeCamera(camShakeIntensity, camShakeFrequency, camShakeTime);
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
        _renderer.enabled = false;
        GameManager.Instance.DidWon = false;
        LevelSceneManager.Instance.LoadEndScreen(false);
    }
}
