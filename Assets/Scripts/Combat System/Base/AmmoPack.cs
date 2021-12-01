using UnityEngine;

namespace Game.Combat
{
    public class AmmoPack : MonoBehaviour
    {
        #region Variables
        [SerializeField] private int _ammoIncreased;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (!collider2D.gameObject.CompareTag("Player")) return;

            Player player = collider2D.GetComponent<Player>();
            if (player.PlayerCombat.CurrentWeapon as RangedWeapon != null)
            {
                RangedWeapon weapon = player.PlayerCombat.CurrentWeapon as RangedWeapon;
                weapon.AmmoSystem.AmmoRecieved(_ammoIncreased);

                Destroy(gameObject);
            }
        }
        #endregion


        #region Component Functions

        #endregion
    }
}