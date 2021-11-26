using System;
using Game.Combat;
using Game.Core;
using Game.HealthSystem;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject _root;
    [SerializeField] private TMP_Text _loadedAmmoText;
    [SerializeField] private TMP_Text _unloadedAmmoText;
    [SerializeField] private GameObject _ammoCounter;
    [SerializeField] private HealthBar<HeartHealthSystem> _healthBar;
    #endregion


    #region Getters And Setters
    public HealthBar<HeartHealthSystem> HealthBar => _healthBar;
    #endregion


    #region Unity Calls
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += UpdateState;
        _root.SetActive(false);
    }

    private void UpdateState()
    {
        if (GameManager.Instance.GameState == GameState.GAME)
        {
            Player.Instance.PlayerCombat.OnWeaponPicked += WeaponPicked;
            Player.Instance.PlayerCombat.OnWeaponDropped += WeaponDropped;
            Player.Instance.PlayerCombat.OnWeaponSwitched += WeaponSwitched;
            _root.SetActive(true);
        }
        else
        {
            _root.SetActive(false);

            try
            {
                Player.Instance.PlayerCombat.OnWeaponPicked -= WeaponPicked;
                Player.Instance.PlayerCombat.OnWeaponDropped -= WeaponDropped;
                Player.Instance.PlayerCombat.OnWeaponSwitched -= WeaponSwitched;
            }
            catch (System.Exception)
            {
            }
        }
    }
    #endregion


    #region Component Functions
    private void UpdateCombatUI(int changeAmount)
    {
        _loadedAmmoText.text = (Player.Instance.PlayerCombat.CurrentWeapon as RangedWeapon).AmmoSystem.LoadedAmmo.ToString();
        _unloadedAmmoText.text = (Player.Instance.PlayerCombat.CurrentWeapon as RangedWeapon).AmmoSystem.UnloadedAmmo.ToString();
    }

    private void UpdateCombatUI()
    {
        _loadedAmmoText.text = (Player.Instance.PlayerCombat.CurrentWeapon as RangedWeapon).AmmoSystem.LoadedAmmo.ToString();
        _unloadedAmmoText.text = (Player.Instance.PlayerCombat.CurrentWeapon as RangedWeapon).AmmoSystem.UnloadedAmmo.ToString();
    }

    private void WeaponPicked(Weapon weapon)
    {
        if (Player.Instance.PlayerCombat.CurrentWeapon as MeleeWeapon != null)
        {
            _loadedAmmoText.text = "\u221E";
            _unloadedAmmoText.text = "\u221E";
        }
        else if (Player.Instance.PlayerCombat.CurrentWeapon as RangedWeapon != null)
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

    private void WeaponSwitched(Weapon weapon)
    {
        if (weapon as RangedWeapon != null)
        {
            RangedWeapon rangedWeapon = weapon as RangedWeapon;
            UpdateCombatUI();
        }
        else if (Player.Instance.PlayerCombat.CurrentWeapon as MeleeWeapon != null)
        {
            _loadedAmmoText.text = "\u221E";
            _unloadedAmmoText.text = "\u221E";
        }
    }
    #endregion
}
