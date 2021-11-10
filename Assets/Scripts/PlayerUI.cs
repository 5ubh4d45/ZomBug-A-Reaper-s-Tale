using System;
using Game.Combat;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    #region Variables
    [SerializeField] private TMP_Text _loadedAmmoText;
    [SerializeField] private TMP_Text _unloadedAmmoText;
    [SerializeField] private GameObject _ammoCounter;
    private PlayerCombat _playerCombat;
    #endregion


    #region Getters And Setters

    #endregion


    #region Unity Calls
    private void Start()
    {
        _playerCombat = GetComponent<PlayerCombat>();
        _playerCombat.OnWeaponPicked += WeaponPicked;
        _playerCombat.OnWeaponDropped += WeaponDropped;
    }

    private void OnDestroy()
    {
        _playerCombat.OnWeaponPicked -= WeaponPicked;
        _playerCombat.OnWeaponDropped -= WeaponDropped;
    }
    #endregion


    #region Component Functions
    private void UpdateCombatUI(int changeAmount)
    {
        _loadedAmmoText.text = (_playerCombat.CurrentWeapon as RangedWeapon).AmmoSystem.LoadedAmmo.ToString();
        _unloadedAmmoText.text = (_playerCombat.CurrentWeapon as RangedWeapon).AmmoSystem.UnloadedAmmo.ToString();
    }

    private void UpdateCombatUI()
    {
        _loadedAmmoText.text = (_playerCombat.CurrentWeapon as RangedWeapon).AmmoSystem.LoadedAmmo.ToString();
        _unloadedAmmoText.text = (_playerCombat.CurrentWeapon as RangedWeapon).AmmoSystem.UnloadedAmmo.ToString();
    }

    private void WeaponPicked(Weapon weapon)
    {
        if (_playerCombat.CurrentWeapon as MeleeWeapon != null)
        {
            _loadedAmmoText.text = "\u221E";
            _unloadedAmmoText.text = "\u221E";
        }
        else if (_playerCombat.CurrentWeapon as RangedWeapon != null)
        {
            RangedWeapon rangedWeapon = weapon as RangedWeapon;
            rangedWeapon.AmmoSystem.OnAmmoUsed += UpdateCombatUI;
            rangedWeapon.AmmoSystem.OnAmmoRecieved += UpdateCombatUI;
            rangedWeapon.AmmoSystem.OnReload += UpdateCombatUI;
            UpdateCombatUI();
        }
        _ammoCounter.SetActive(true);
    }

    private void WeaponDropped(Weapon lastWeapon)
    {
        if (lastWeapon as RangedWeapon != null)
        {
            RangedWeapon weapon = lastWeapon as RangedWeapon;
            weapon.AmmoSystem.OnAmmoUsed -= UpdateCombatUI;
            weapon.AmmoSystem.OnAmmoRecieved -= UpdateCombatUI;
            weapon.AmmoSystem.OnReload -= UpdateCombatUI;
        }
        _ammoCounter.SetActive(false);
    }
    #endregion
}
