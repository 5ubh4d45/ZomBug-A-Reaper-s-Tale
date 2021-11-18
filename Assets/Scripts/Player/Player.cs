using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.HealthSystem;


//this class will contain all the functionalities
public class Player : HealthObject<HeartHealthSystem>
{
    private static Player _instance;
    public static Player Instance => _instance;


    [Header("CameraShake")]
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private float camShakeIntensity = 1f;
    [SerializeField] private float camShakeFrequency = 3f;
    [SerializeField] private float camShakeTime = 0.2f;

    [Space]
    [SerializeField] private float moveSpeed = 10;

    private PlayerCombat _playerCombat;

    public float MoveSpeed => moveSpeed;
    public IInteractable Interactable { get; set; }
    public PlayerCombat PlayerCombat => _playerCombat;

    private void Start()
    {
        _instance = this;
        _playerCombat = GetComponent<PlayerCombat>();
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
    }
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("NPC"))
        {
            _healthSystem.Damage(1);

            //adds cam shake when damage taken
            cameraShake.ShakeCamera(camShakeIntensity, camShakeFrequency, camShakeTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Health Point"))
        {
            _healthSystem.Heal(1);

        }
    }
}