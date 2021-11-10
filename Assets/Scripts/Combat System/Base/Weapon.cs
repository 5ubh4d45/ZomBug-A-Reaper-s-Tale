using System.Linq;
using UnityEngine;

namespace Game.Combat
{
    public abstract class Weapon : MonoBehaviour, IInteractable
    {
        #region Variables
        [SerializeField] protected private float _attackDamage;
        [SerializeField] protected private float _startTimeBtwShots;
        [SerializeField] protected private LayerMask _attackLayer;
        [SerializeField] protected private Sprite _displayImage;
        [SerializeField] protected private Color _displayImageTint;
        #endregion


        #region Getters And Setters
        /// <summary>
        /// The amount of damage this weapon deals
        /// </summary>
        public float AttackDamage => _attackDamage;

        /// <summary>
        /// Is this weapon picked up by the player
        /// </summary>
        [HideInInspector] public bool IsPickedUp = false;

        /// <summary>
        /// The image displayed for this weapon as the icon on the weapon switch wheel
        /// </summary>
        public Sprite DisplayImage => _displayImage;

        /// <summary>
        /// The tint for Display Image
        /// </summary>
        public Color DisplayImageTint => _displayImageTint;
        #endregion


        #region Unity Calls
        public virtual void Update()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1).ToArray();
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<Player>(out Player player) && !IsPickedUp)
                {
                    player.Interactable = this;
                }
            }
        }
        #endregion


        #region Component Functions
        public abstract void Attack();

        public virtual void Interact(Player player)
        {
            player.PlayerCombat.PickupWeapon(this);
        }
        #endregion
    }
}