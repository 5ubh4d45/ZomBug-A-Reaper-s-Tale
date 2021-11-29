using System;
using UnityEngine;

namespace Game.Combat
{
    public class Shotgun : RangedWeapon
    {
        #region Variables
        [SerializeField] private Transform _projectileEmitionPos;
        [SerializeField] private GameObject _projectile;
        [SerializeField] private GameObject _mapDisplay;
        [SerializeField] private GameObject _weapon;
        [SerializeField] private ShotgunAnimator _animationHandler;
        
        [Range(0f, 1f)]
        [SerializeField] private float spread;

        private float _timeBtwShots;

        private Vector3 _targetPos;
        private Vector3 _targetPos1;
        private Vector3 _targetPos2;
        private float _distance;
        private float _spreadModifier = 1f;
        private Vector3 _bullet1 = new Vector3(-1f, 1f, 0f);
        private Vector3 _bullet2 = new Vector3(1f, -1f, 0f);
        
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        private void Awake()
        {
            _animationHandler = _animationHandler == null ? GetComponent<ShotgunAnimator>() : _animationHandler;
        }
        
        public override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.R))
            {
                _ammoSystem.Reload();
            }

            _targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            _distance = Vector3.Distance(_projectileEmitionPos.position, _targetPos);
            _distance = Mathf.Clamp(_distance, 1f, 2f);
            _distance /= 2f;
            _spreadModifier = spread * _distance;
            
            _targetPos1 = _targetPos + (_bullet1 * _spreadModifier);
            _targetPos2 = _targetPos + (_bullet2 * _spreadModifier);
            
            
            _timeBtwShots -= Time.deltaTime;
        }
        #endregion


        #region Component Functions
        public override bool AttackRanged()
        {
            if (_timeBtwShots > 0 || AmmoSystem.LoadedAmmo <= 0) return false;
            
            //spwans 3 bullets
            
            //bullet 1
            GameObject projectileClone1 = Instantiate(_projectile, _projectileEmitionPos.position, transform.rotation);
            projectileClone1.GetComponent<Projectile>().Initialise(_attackDamage, _targetPos1, _attackTag);
            
            //bullet 2
            GameObject projectileClone2 = Instantiate(_projectile, _projectileEmitionPos.position, transform.rotation);
            projectileClone2.GetComponent<Projectile>().Initialise(_attackDamage, _targetPos, _attackTag);
            
            //bullet 3
            GameObject projectileClone3 = Instantiate(_projectile, _projectileEmitionPos.position, transform.rotation);
            projectileClone3.GetComponent<Projectile>().Initialise(_attackDamage, _targetPos2, _attackTag);
            
            _timeBtwShots = _startTimeBtwShots;

            _animationHandler.PlayShotgunFire();

            return true;
        }

        public override void PickUp()
        {
            _weapon.SetActive(true);
            _mapDisplay.SetActive(false);
        }

        public override void DropDown()
        {
            _weapon.SetActive(true);
            _mapDisplay.SetActive(false);
        }
        #endregion
    }
}