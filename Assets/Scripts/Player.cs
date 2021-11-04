using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.HealthSystem;


//this class will contain all the functionalities
public class Player : HealthObject<HeartHealthSystem>
{
    [SerializeField] private float moveSpeed = 10;

    public float MoveSpeed => moveSpeed;
    public IInteractable Interactable { get; set; }



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
