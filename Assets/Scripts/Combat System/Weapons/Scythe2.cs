using System;
using UnityEngine;
using  Game.HealthSystem;
using Pathfinding.Ionic.Zlib;

namespace Game.Combat
{

    public class Scythe2 : MonoBehaviour
    {
        [SerializeField] private string attackTag;
        [SerializeField] private float melee1Damage = 10f;
        [SerializeField] private float melee2Damage = 30f;
        [SerializeField] private float impactForce;
        [SerializeField] private Player player;

        private float _damage;
        public float Damage { get => _damage; set => _damage = value; }
        
        public float Melee1Damage => melee1Damage;
        public float Melee2Damage => melee2Damage;

        private void Awake()
        {
            if (player == null)
            {
                player = GetComponentInParent<Player>();
                
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            HealthObject healthObject = collision.gameObject.GetComponent<HealthObject>();
            
            if (healthObject != null && collision.gameObject.CompareTag(attackTag))
            {
                healthObject.HealthSystem().Damage(_damage);
                
                //adding a force to impact
                collision.gameObject.GetComponentInParent<Rigidbody2D>().AddForce(player.PlayerMovement.LookDirection * impactForce, ForceMode2D.Impulse);
            }
        }

    }
}
