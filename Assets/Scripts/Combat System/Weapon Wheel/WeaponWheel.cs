using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Scenes;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Combat
{
    /// <summary>
    /// This class represents a Weapon wheel for switching between weapons.
    /// </summary>
    public class WeaponWheel : MonoBehaviour
    {
        #region Singleton
        private static WeaponWheel _instance;
        public static WeaponWheel Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<WeaponWheel>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("WeaponWheel Instance", typeof(WeaponWheel));
                    _instance = go.GetComponent<WeaponWheel>();
                }
                return _instance;
            }
        }
        #endregion


        #region Variables
        [Header("References")]
        [SerializeField] private WeaponWheelButton _buttonObject;
        [SerializeField] private GameObject _weaponWheelParent;
        [SerializeField] private PlayerCombat _playerCombat;

        [Space]

        [Header("UI")]
        [SerializeField] private float _gapWidth;
        [SerializeField] private ColorBlock _colors;

        [Space]

        [Header("Input")]
        [Range(0f, 1f)]
        [SerializeField] private float _minimumHieght;
        [Range(0f, 1f)]
        [SerializeField] private float _maximumHieght;

        private WeaponWheelButton _hoveredWheelButton;
        private WeaponWheelButton _selectedWheelButton;
        private List<WeaponWheelButton> _weaponWheelButtons;
        private int _activeElementindex;
        #endregion


        #region Getters And Setters
        /// <summary>
        /// Is The Weapon Wheel Opened
        /// </summary>
        public bool IsOpened { get => _weaponWheelParent.activeInHierarchy; }

        /// <summary>
        /// The Amount of degrees given for each Item.
        /// </summary>
        public float StepLength => 360 / _playerCombat.MaxWeapons;
        #endregion


        #region Unity Calls
        private void Awake()
        {
            SceneCollectionHandler.Instance.OnLoadCompelete += Initialise;
        }

        private void Initialise()
        {
            _weaponWheelButtons = new List<WeaponWheelButton>();
            if (_playerCombat == null) _playerCombat = Player.Instance.PlayerCombat;
            _playerCombat.OnWeaponDropped += UpdateWheel;
            _playerCombat.OnWeaponPicked += UpdateWheel;
            _playerCombat.OnWeaponSwitched += UpdateWheel;
            UpdateWheel(null);
        }

        private void Update()
        {
            _hoveredWheelButton = null;

            HandleWheelState();

            if (!IsOpened) return;

            UpdateActiveElement();

            UpdateSelectedElement();

            UpdateButtonStates();
        }
        private void OnDestroy()
        {
            _playerCombat.OnWeaponDropped -= UpdateWheel;
            _playerCombat.OnWeaponPicked -= UpdateWheel;
        }

        private void OnDrawGizmos()
        {
            Handles.DrawWireDisc(_weaponWheelParent.transform.position, new Vector3(0, 0, 1), _maximumHieght * Screen.height);
            Handles.DrawWireDisc(_weaponWheelParent.transform.position, new Vector3(0, 0, 1), _minimumHieght * Screen.height);
        }
        #endregion


        #region Component Functions
        /// <summary>
        /// Recreates the weapon wheel by destroying the old one and creating a new one.
        /// </summary>
        public void UpdateWheel(Weapon _)
        {
            foreach (Transform child in _weaponWheelParent.transform)
            {
                Destroy(child.gameObject);
            }
            _weaponWheelButtons = new List<WeaponWheelButton>();

            InstantiateWheel();

            UpdateButtonStates();
        }

        private void InstantiateWheel()
        {
            for (int i = 0; i < _playerCombat.MaxWeapons; i++)
            {
                Quaternion rotation = Quaternion.Euler(0, 0, i * StepLength);

                WeaponWheelButton button = Instantiate(_buttonObject, transform.position, rotation);
                RectTransform rectTransform = button.GetComponent<RectTransform>();
                Image image = button.GetComponent<Image>();

                button.transform.SetParent(_weaponWheelParent.transform);
                button.transform.name = $"Weapon Wheel Button {i}";

                float fillAmount = StepLength / 360 - _gapWidth / 360f;
                image.fillAmount = fillAmount;

                Weapon weapon = _playerCombat.Weapons?.Count > i ? _playerCombat.Weapons?[i] : null;

                button.IconUI.sprite = weapon?.DisplayImage;
                button.IconUI.color = weapon?.DisplayImage == null ? new Color(0, 0, 0, 0) : weapon.DisplayImageTint;

                _weaponWheelButtons.Add(button);
            }
        }

        private void HandleWheelState()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (_weaponWheelParent.activeInHierarchy)
                {
                    Close();
                }
                else if (!_weaponWheelParent.activeInHierarchy)
                {
                    Open();
                }
            }
        }

        private void UpdateActiveElement()
        {
            float mouseAngle = NormalizeAngle(Vector3.SignedAngle(Vector3.up, Input.mousePosition - transform.position, Vector3.forward) + StepLength);
            _activeElementindex = (int)(mouseAngle / StepLength);

            float sqrDistance = Mathf.Pow(_weaponWheelParent.transform.position.x - Input.mousePosition.x, 2) + Mathf.Pow(_weaponWheelParent.transform.position.y - Input.mousePosition.y, 2);
            if (sqrDistance > Mathf.Pow(_maximumHieght * Screen.height, 2)) return;
            if (sqrDistance < Mathf.Pow(_minimumHieght * Screen.height, 2)) return;

            _hoveredWheelButton = _weaponWheelButtons[_activeElementindex];
        }

        private void UpdateSelectedElement()
        {
            if (Input.GetMouseButtonDown(0) && _hoveredWheelButton != null)
            {
                _selectedWheelButton = _hoveredWheelButton;
                _playerCombat.SwitchWeapon(_activeElementindex);
                Close();
            }
        }

        private void UpdateButtonStates()
        {
            for (int i = 0; i < _weaponWheelButtons.Count; i++)
            {
                WeaponWheelButton button = _weaponWheelButtons[i];
                button.ButtonUI.color = _colors.normalColor;

                if (_hoveredWheelButton == button)
                {
                    button.ButtonUI.color = _colors.highlightedColor;
                }

                if (_selectedWheelButton == button || i == _playerCombat.CurrentWeaponIndex)
                {
                    button.ButtonUI.color = _colors.selectedColor;
                }
            }
        }

        private float NormalizeAngle(float a) => (a + 360f) % 360f;

        /// <summary>
        /// open the weapon wheel
        /// </summary>
        public void Open() => _weaponWheelParent.SetActive(true);

        /// <summary>
        /// close the weapon wheel
        /// </summary>
        public void Close() => _weaponWheelParent.SetActive(false);
        #endregion
    }
}