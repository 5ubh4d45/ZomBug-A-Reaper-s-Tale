using Game.HealthSystem;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Combat
{
    public class Scythe : MeleeWeapon
    {
        #region Variables
        [SerializeField] private Transform _attackPoint;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.DrawWireDisc(_attackPoint.position, new Vector3(0, 0, 1), _attackRange);
        }
#endif
        #endregion


        #region Component Functions
        public override void AttackMelee()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange);
            if (colliders.Length > 0)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    Collider2D collider = colliders[i];
                    HealthObject healthObject = collider.GetComponentInParent<HealthObject>();
                    if (healthObject != null && collider.gameObject.CompareTag(_attackTag))
                    {
                        healthObject.HealthSystem.Damage(_attackDamage);
                    }
                }
            }
        }

        public override void PickUp()
        {
        }

        public override void DropDown()
        {
        }
        #endregion
    }
}