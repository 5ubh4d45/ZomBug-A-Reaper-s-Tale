using System.Collections;
using UnityEngine;

namespace Game.Combat
{
    /// <summary>
    /// This is a simple ammo system for weapons
    /// </summary>
    [System.Serializable]
    public class AmmoSystem
    {
        #region Variables
        [SerializeField] private int _loadedAmmo;
        [SerializeField] private int _maximumAmmo;
        [SerializeField] private int _unloadedAmmo;
        [SerializeField] private float _reloadTime;

        /// <summary>
        /// This Event is reaised when Some amount of ammo is spent or used
        /// </summary>
        public Event<int> OnAmmoUsed;

        /// <summary>
        /// This Event is reaised when Some amount of ammo is recieved and added to the unloaded ammo count
        /// </summary>
        public Event<int> OnAmmoRecieved;

        /// <summary>
        /// This Event is reaised affter the weapon is reloaded
        /// </summary>
        public Empty OnReload;
        #endregion


        #region Getters And Setters
        /// <summary>
        /// The amount of ammo that is currently loaded and can be used
        /// </summary>
        public int LoadedAmmo => _loadedAmmo;

        /// <summary>
        /// The amount of ammo that is available but cannot be used as it's unloaded
        /// </summary>
        public int UnloadedAmmo => _unloadedAmmo;

        /// <summary>
        /// The amount of ammo that can be loaded at once
        /// </summary>
        public int MaximumAmmo => _maximumAmmo;

        /// <summary>
        /// The time required to reload this gun
        /// </summary>
        public float ReloadTime => _reloadTime;

        /// <summary>
        /// Is the gun bieng reloaded
        /// </summary>
        public bool IsReloading { get; private set; }
        #endregion


        #region Class Functions
        /// <summary>
        /// Uses an X Amount of ammo. where x is the passed parameter useCount.
        /// </summary>
        public void UseAmmo(int useCount)
        {
            if (_loadedAmmo < useCount) return;
            _loadedAmmo -= useCount;
            OnAmmoUsed?.Invoke(useCount);
        }

        /// <summary>
        /// This function adds an X amount of ammo to the unloaded ammo count. where x is the passed parameter recieveAmount.
        /// </summary>
        public void AmmoRecieved(int recieveAmount)
        {
            _unloadedAmmo += recieveAmount;
            OnAmmoRecieved?.Invoke(recieveAmount);
        }

        /// <summary>
        /// reloads the gun after waiting for the given reload time.
        /// </summary>
        public void Reload()
        {
            MonoBehaviour mb = GameObject.FindObjectOfType<MonoBehaviour>();
            mb.StartCoroutine(WaitForReload());
        }

        private IEnumerator WaitForReload()
        {
            IsReloading = true;

            yield return new WaitForSeconds(_reloadTime);

            if (_unloadedAmmo < (_maximumAmmo - _loadedAmmo))
            {
                _loadedAmmo += _unloadedAmmo;
                _unloadedAmmo = 0;
            }
            else
            {
                _unloadedAmmo -= (_maximumAmmo - _loadedAmmo);
                _loadedAmmo += (_maximumAmmo - _loadedAmmo);
            }

            IsReloading = false;

            OnReload?.Invoke();
        }
        #endregion


        #region Constructors
        public AmmoSystem(int maximumAmmo)
        {
            _maximumAmmo = maximumAmmo;
            _loadedAmmo = maximumAmmo;
            _unloadedAmmo = 0;
        }
        public AmmoSystem(int maximumAmmo, int loadedAmmo)
        {
            _maximumAmmo = maximumAmmo;
            _loadedAmmo = loadedAmmo;
            _unloadedAmmo = 0;
        }
        public AmmoSystem(int maximumAmmo, int loadedAmmo, int unloadedAmmo)
        {
            _maximumAmmo = maximumAmmo;
            _loadedAmmo = loadedAmmo;
            _unloadedAmmo = unloadedAmmo;
        }
        #endregion
    }
}