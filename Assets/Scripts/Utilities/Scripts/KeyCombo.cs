using System;
using UnityEngine;


namespace Utils
{
    /// <summary>
    /// A utility class for having Key combos like Shift+T, Ctrl+J
    /// </summary>
    [Serializable]
    public class KeyCombo
    {
        #region Variables
        [SerializeField] private KeyCode _key;
        [SerializeField] private bool _ctrl;
        [SerializeField] private bool _shift;
        [SerializeField] private bool _alt;

        ///<summary> 
        /// The KeyCode To be used  
        /// </summary>
        public KeyCode Key { get => _key; }
        ///<summary> 
        /// Should check for Control key?  
        /// </summary>
        public bool Ctrl { get => _ctrl; }
        ///<summary> 
        /// Should check for Shift key?
        ///  </summary>
        public bool Shift { get => _shift; }
        ///<summary> 
        /// Should check for Alt key?
        ///  </summary>
        public bool Alt { get => _alt; }

        /// <summary>
        /// Returns true if all the enabled Modifiers are pressed
        /// </summary>
        public bool AreModifiersPressed
        {
            get
            {
                bool flag = true;
                if (_alt)
                {
                    if (InputUtils.IsAlt()) flag = flag && true;
                    else return false;
                }

                if (_shift)
                {
                    if (InputUtils.IsShift()) flag = flag && true;
                    else return false;
                }

                if (_ctrl)
                {
                    if (InputUtils.IsCtrl()) flag = flag && true;
                    else return false;
                }

                return flag;
            }
        }
        #endregion


        #region Constructors
        public KeyCombo(KeyCode key = KeyCode.Tab, bool ctrl = false, bool shift = false, bool alt = false)
        {
            _key = key;
            _ctrl = ctrl;
            _shift = shift;
            _alt = alt;
        }
        #endregion


        #region Functions
        public bool IsComboPressed()
        {
            bool flag = AreModifiersPressed;

            if ((Input.GetKeyDown(_key)))
            {
                flag = flag && true;
            }
            else
            {
                return false;
            }
            return flag;
        }
        #endregion
    }
}