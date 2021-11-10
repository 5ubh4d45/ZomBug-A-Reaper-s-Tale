using System.Collections;
using System.Collections.Generic;
using Game.Combat;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private PlayerAimWeapon _weaponAim;
    [SerializeField] private int _maxWeapons;

    private Weapon _currentWeapon;
    private List<Weapon> _weapons;
    private GameObject _weaponHolder;

    public Event<Weapon> OnWeaponPicked;
    public Event<Weapon> OnWeaponDropped;
    public Event<Weapon> OnWeaponSwitched;
    public int CurrentWeaponIndex => _weapons?.IndexOf(_currentWeapon) == null ? -1 : _weapons.IndexOf(_currentWeapon);
    public Weapon CurrentWeapon => _currentWeapon;
    public int MaxWeapons => _maxWeapons;
    public List<Weapon> Weapons => _weapons;

    // Start is called before the first frame update
    void Awake()
    {
        _weapons = new List<Weapon>();
        _currentWeapon = GetComponentInChildren<Weapon>();
        _weaponHolder = _currentWeapon?.gameObject;
        if (_currentWeapon != null) PickupWeapon(_currentWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _currentWeapon?.Attack();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            DropWeapon();
        }

        for (int i = 1; i <= _maxWeapons; i++)
        {
            if (Input.GetKeyDown((KeyCode)(i + 48)))
            {
                SwitchWeapon(i - 1);
            }
        }
    }

    public void PickupWeapon(Weapon weapon)
    {
        if (_weapons.Contains(weapon)) return;

        // Drop the current weapon if any
        if (_weapons.Count >= _maxWeapons)
        {
            DropWeapon();
        }

        // Parent and set the new weapon
        ParentWeapon(weapon);

        _weapons.Add(_currentWeapon);
        UpdateWeaponList();
        OnWeaponPicked?.Invoke(_currentWeapon);
    }

    private void UpdateWeaponList()
    {
        for (int i = 0; i < _weapons.Count; i++)
        {
            if (i == CurrentWeaponIndex || _weapons[i] == null)
            {
                _weapons[i].IsPickedUp = true;
                continue;
            }

            _weapons[i].gameObject.SetActive(false);
            _currentWeapon.IsPickedUp = true;
        }
    }

    private void ParentWeapon(Weapon weapon)
    {
        weapon.transform.SetParent(_weaponAim.transform);
        weapon.transform.localPosition = Vector3.zero;

        _weaponHolder = weapon.gameObject;
        _currentWeapon = weapon;

        _weaponAim.aimTransform = _weaponHolder.transform;
        _weaponAim.UpdateTargetRotation();

        _currentWeapon.IsPickedUp = true;
        weapon.gameObject.SetActive(true);
    }

    public void DropWeapon()
    {
        if (_currentWeapon == null) return;

        Weapon lastWeapon = _currentWeapon;
        int index = _weapons.IndexOf(_currentWeapon);
        _weapons.RemoveAt(index);

        _weaponHolder.transform.SetParent(null);

        _weaponHolder = null;
        _currentWeapon = null;
        _weaponAim.aimTransform = _weaponAim.transform;

        OnWeaponDropped?.Invoke(lastWeapon);
    }

    public void SwitchWeapon(int index)
    {
        if (CurrentWeaponIndex == index) return;

        if (index >= _weapons.Count)
        {
            _weaponAim.aimTransform = _weaponAim.transform;
            _currentWeapon?.gameObject.SetActive(false);
            _currentWeapon = null;
            _weaponHolder = null;

            return;
        }

        Weapon weapon = _weapons[index];
        ParentWeapon(weapon);
        UpdateWeaponList();

        OnWeaponSwitched?.Invoke(weapon);
    }
}
