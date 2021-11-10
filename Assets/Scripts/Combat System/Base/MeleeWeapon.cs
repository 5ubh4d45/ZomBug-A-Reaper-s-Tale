using System;
using UnityEngine;

namespace Game.Combat
{
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