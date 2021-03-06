using System;
using System.Collections;
using System.Collections.Generic;
using Game.Combat;
using Game.Core;
using Game.DialogueSystem;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private PlayerAimWeapon _weaponAim;
    [SerializeField] private int _maxWeapons;
    [SerializeField] private Player player;
    [SerializeField] private Scythe2 _scythe;
    [SerializeField] private Sprite _scytheDisplay;

    private Weapon _currentWeapon;
    private List<Weapon> _weapons;
    private GameObject _weaponHolder;


    public Event<Weapon> OnWeaponPicked;
    public Event<Weapon> OnWeaponDropped;
    public Event<Weapon> OnWeaponSwitched;
    public Sprite ScytheDisplay => _scytheDisplay;
    public int CurrentWeaponIndex => _weapons?.IndexOf(_currentWeapon) == null ? -1 : _weapons.IndexOf(_currentWeapon);
    public Weapon CurrentWeapon => _currentWeapon;
    public int MaxWeapons => _maxWeapons;
    public List<Weapon> Weapons => _weapons;
    public bool HasGun
    {
        get
        {
            return _currentWeapon as RangedWeapon != null;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        _weapons = new List<Weapon>();
        _currentWeapon = GetComponentInChildren<Weapon>();
        _weaponHolder = _currentWeapon?.gameObject;
        if (_currentWeapon != null) PickupWeapon(_currentWeapon);
    }

    private void Start()
    {
        player = GetComponent<Player>();
        if (_scythe == null)
        {
            _scythe = GetComponentInChildren<Scythe2>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameState != GameState.GAME) return;

        if (Input.GetMouseButtonDown(0) && !DialogueManager.Instance.IsOpen && !WeaponWheel.Instance.IsOpened)
        {
            if (player.IsDead) return;

            _currentWeapon?.Attack();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (player.IsDead) return;

            MeleeAttack1();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            if (player.IsDead) return;

            MeleeAttack2();
        }

        for (int i = 1; i <= _maxWeapons; i++)
        {
            if (Input.GetKeyDown((KeyCode)(i + 48)) || Input.GetKeyDown((KeyCode)i + 256))
            {
                SwitchWeapon(i - 1);
            }
        }
    }

    public void PickupWeapon(Weapon weapon)
    {
        if (_weapons.Contains(weapon)) return;

        // Drop the current weapon if any
        if (_weapons.Count >= _maxWeapons - 1)
        {
            DropWeapon();
        }

        // Parent and set the new weapon
        ParentWeapon(weapon);

        _weapons.Add(_currentWeapon);
        _currentWeapon.PickUp();
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
        _currentWeapon.IsPickedUp = false;
        _currentWeapon.DropDown();

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
        Debug.Log(index - 1 >= _weapons.Count || index == 0);
        if (CurrentWeaponIndex + 1 == index) return;

        if (index - 1 >= _weapons.Count || index == 0)
        {
            _weaponAim.aimTransform = _weaponAim.transform;
            _currentWeapon?.gameObject.SetActive(false);
            _currentWeapon = null;
            _weaponHolder = null;
            OnWeaponSwitched?.Invoke(null);

            return;
        }

        Weapon weapon = _weapons[index - 1];
        ParentWeapon(weapon);
        UpdateWeaponList();

        OnWeaponSwitched?.Invoke(weapon);
    }

    private void MeleeAttack1()
    {
        //setting up the different dmg outputs for different attacks
        _scythe.Damage = _scythe.Melee1Damage;

        player.PlayerAnimator.PlayMeleeAttack1();

        IEnumerator stopMovement = player.PlayerMovement.StopMovement(0.5f);
        StartCoroutine(stopMovement);
    }

    private void MeleeAttack2()
    {

        //setting up the different dmg outputs for different attacks
        _scythe.Damage = _scythe.Melee2Damage;

        player.PlayerAnimator.PlayMeleeAttack2();

        IEnumerator stopMovement = player.PlayerMovement.StopMovement(1f);
        StartCoroutine(stopMovement);
    }

    public void Reset()
    {
        _weaponAim.aimTransform = _weaponAim.transform;
        foreach (var child in _weapons)
        {
            Destroy(child.gameObject);
        }
        _currentWeapon = null;
        _weaponHolder = null;
        _weapons = new List<Weapon>();
    }
}
