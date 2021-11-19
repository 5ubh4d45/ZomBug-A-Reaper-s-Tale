using UnityEngine;

namespace Game.Combat
{
    public class Pistol : RangedWeapon
    {
        #region Variables
        [SerializeField] private Transform _projectileEmitionPos;
        [SerializeField] private GameObject _projectile;

        private float _timeBtwShots;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        public override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.R))
            {
                _ammoSystem.Reload();
            }
            _timeBtwShots -= Time.deltaTime;
        }
        #endregion


        #region Component Functions
        public override bool AttackRanged()
        {
            if (_timeBtwShots > 0 || AmmoSystem.LoadedAmmo <= 0) return false;

            GameObject projectileClone = Instantiate(_projectile, _projectileEmitionPos.position, transform.rotation);
            projectileClone.GetComponent<Projectile>().Initialise(_attackDamage, Camera.main.ScreenToWorldPoint(Input.mousePosition), _attackTag);
            _timeBtwShots = _startTimeBtwShots;
            return true;
        }
        #endregion
    }
}