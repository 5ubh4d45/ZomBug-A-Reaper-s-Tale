using UnityEngine;

namespace Game.HealthSystem
{
    public class Medikit : MonoBehaviour
    {
        #region Variables
        [SerializeField] private int _healAmount;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (!collider2D.gameObject.CompareTag("Player")) return;

            Player player = collider2D.GetComponent<Player>();
            HeartHealthSystem healthSystem = player.HealthSystem as HeartHealthSystem;

            if (healthSystem.Health == healthSystem.MaxHealth) return;

            healthSystem.Heal(_healAmount);

            Destroy(gameObject);
        }
        #endregion


        #region Component Functions

        #endregion
    }
}