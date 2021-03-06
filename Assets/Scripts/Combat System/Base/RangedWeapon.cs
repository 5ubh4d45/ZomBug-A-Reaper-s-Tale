using UnityEngine;

namespace Game.Combat
{
    /// <summary>
    /// This class should be inherited by classes that represent a Ranged Weapon.
    /// </summary>
    public abstract class RangedWeapon : Weapon
    {
        #region Variables
        [SerializeField] protected private AmmoSystem _ammoSystem;
        #endregion


        #region Getters And Setters
        public AmmoSystem AmmoSystem => _ammoSystem;
        #endregion


        #region Unity Calls
        #endregion


        #region Component Functions
        public override void Attack()
        {
            if (!AttackRanged()) return;
            _ammoSystem.UseAmmo(1);
        }

        public abstract bool AttackRanged();
        #endregion
    }
}