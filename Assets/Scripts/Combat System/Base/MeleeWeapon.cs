using System;
using UnityEngine;

namespace Game.Combat
{
    /// <summary>
    /// This class should be inherited by classes that represent melee weapons
    /// </summary>
    public abstract class MeleeWeapon : Weapon
    {
        #region Variables
        [SerializeField] protected private float _attackRange;
        #endregion 


        #region Getters And Setters

        #endregion


        #region Unity Calls

        #endregion


        #region Component Functions
        public override void Attack()
        {
            AttackMelee();
        }

        public abstract void AttackMelee();
        #endregion
    }
}