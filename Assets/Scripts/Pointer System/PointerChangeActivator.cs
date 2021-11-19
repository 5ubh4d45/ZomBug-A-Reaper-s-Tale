using UnityEngine;

namespace Game.Pointer
{
    public class PointerChangeActivator : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Sprite[] _cursorTextures;
        [SerializeField] private int _fps;
        [SerializeField] private PointerTriggerEvents _triggerEvent;
        [SerializeField] private PointerTriggerEvents _setDefaultEvent;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        private void Awake()
        {
            if (_triggerEvent == PointerTriggerEvents.Awake) PointerManager.Instance.SetCursor(_cursorTextures, _fps);
            if (_setDefaultEvent == PointerTriggerEvents.Awake) PointerManager.Instance.SetDefaultCursor();
        }

        private void Start()
        {
            if (_triggerEvent == PointerTriggerEvents.Start) PointerManager.Instance.SetCursor(_cursorTextures, _fps);
            if (_setDefaultEvent == PointerTriggerEvents.Start) PointerManager.Instance.SetDefaultCursor();
        }

        private void OnCollisionEnter()
        {
            if (_triggerEvent == PointerTriggerEvents.OnCollisionEnter) PointerManager.Instance.SetCursor(_cursorTextures, _fps);
            if (_setDefaultEvent == PointerTriggerEvents.OnCollisionEnter) PointerManager.Instance.SetDefaultCursor();
        }

        private void OnCollisionExit()
        {
            if (_triggerEvent == PointerTriggerEvents.OnCollisionExit) PointerManager.Instance.SetCursor(_cursorTextures, _fps);
            if (_setDefaultEvent == PointerTriggerEvents.OnCollisionExit) PointerManager.Instance.SetDefaultCursor();
        }

        private void OnCollisionStay()
        {
            if (_triggerEvent == PointerTriggerEvents.OnCollisionStay) PointerManager.Instance.SetCursor(_cursorTextures, _fps);
            if (_setDefaultEvent == PointerTriggerEvents.OnCollisionStay) PointerManager.Instance.SetDefaultCursor();
        }

        private void OnCollisionEnter2D()
        {
            if (_triggerEvent == PointerTriggerEvents.OnCollisionEnter2D) PointerManager.Instance.SetCursor(_cursorTextures, _fps);
            if (_setDefaultEvent == PointerTriggerEvents.OnCollisionEnter2D) PointerManager.Instance.SetDefaultCursor();
        }

        private void OnCollisionExit2D()
        {
            if (_triggerEvent == PointerTriggerEvents.OnCollisionExit2D) PointerManager.Instance.SetCursor(_cursorTextures, _fps);
            if (_setDefaultEvent == PointerTriggerEvents.OnCollisionExit2D) PointerManager.Instance.SetDefaultCursor();
        }

        private void OnCollisionStay2D()
        {
            if (_triggerEvent == PointerTriggerEvents.OnCollisionStay2D) PointerManager.Instance.SetCursor(_cursorTextures, _fps);
            if (_setDefaultEvent == PointerTriggerEvents.OnCollisionStay2D) PointerManager.Instance.SetDefaultCursor();
        }

        private void OnTriggerEnter()
        {
            if (_triggerEvent == PointerTriggerEvents.OnTriggerEnter) PointerManager.Instance.SetCursor(_cursorTextures, _fps);
            if (_setDefaultEvent == PointerTriggerEvents.OnTriggerEnter) PointerManager.Instance.SetDefaultCursor();
        }

        private void OnTriggerExit()
        {
            if (_triggerEvent == PointerTriggerEvents.OnTriggerExit) PointerManager.Instance.SetCursor(_cursorTextures, _fps);
            if (_setDefaultEvent == PointerTriggerEvents.OnTriggerExit) PointerManager.Instance.SetDefaultCursor();
        }

        private void OnTriggerStay()
        {
            if (_triggerEvent == PointerTriggerEvents.OnTriggerStay) PointerManager.Instance.SetCursor(_cursorTextures, _fps);
            if (_setDefaultEvent == PointerTriggerEvents.OnTriggerStay) PointerManager.Instance.SetDefaultCursor();
        }

        private void OnTriggerEnter2D()
        {
            if (_triggerEvent == PointerTriggerEvents.OnTriggerEnter2D) PointerManager.Instance.SetCursor(_cursorTextures, _fps);
            if (_setDefaultEvent == PointerTriggerEvents.OnTriggerEnter2D) PointerManager.Instance.SetDefaultCursor();
        }

        private void OnTriggerExit2D()
        {
            if (_triggerEvent == PointerTriggerEvents.OnTriggerExit2D) PointerManager.Instance.SetCursor(_cursorTextures, _fps);
            if (_setDefaultEvent == PointerTriggerEvents.OnTriggerExit2D) PointerManager.Instance.SetDefaultCursor();
        }

        private void OnTriggerStay2D()
        {
            if (_triggerEvent == PointerTriggerEvents.OnTriggerStay2D) PointerManager.Instance.SetCursor(_cursorTextures, _fps);
            if (_setDefaultEvent == PointerTriggerEvents.OnTriggerStay2D) PointerManager.Instance.SetDefaultCursor();
        }

        private void OnMouseEnter()
        {
            if (_triggerEvent == PointerTriggerEvents.OnMouseEnter) PointerManager.Instance.SetCursor(_cursorTextures, _fps);
            if (_setDefaultEvent == PointerTriggerEvents.OnMouseEnter) PointerManager.Instance.SetDefaultCursor();
        }

        private void OnMouseExit()
        {
            if (_triggerEvent == PointerTriggerEvents.OnMouseExit) PointerManager.Instance.SetCursor(_cursorTextures, _fps);
            if (_setDefaultEvent == PointerTriggerEvents.OnMouseExit) PointerManager.Instance.SetDefaultCursor();
        }

        private void OnMouseDrag()
        {
            if (_triggerEvent == PointerTriggerEvents.OnMouseDrag) PointerManager.Instance.SetCursor(_cursorTextures, _fps);
            if (_setDefaultEvent == PointerTriggerEvents.OnMouseDrag) PointerManager.Instance.SetDefaultCursor();
        }

        private void OnMouseUp()
        {
            if (_triggerEvent == PointerTriggerEvents.OnMouseUp) PointerManager.Instance.SetCursor(_cursorTextures, _fps);
            if (_setDefaultEvent == PointerTriggerEvents.OnMouseUp) PointerManager.Instance.SetDefaultCursor();
        }

        private void OnMouseOver()
        {
            if (_triggerEvent == PointerTriggerEvents.OnMouseOver) PointerManager.Instance.SetCursor(_cursorTextures, _fps);
            if (_setDefaultEvent == PointerTriggerEvents.OnMouseOver) PointerManager.Instance.SetDefaultCursor();
        }

        private void OnDisable()
        {
            if (_triggerEvent == PointerTriggerEvents.OnDisable) PointerManager.Instance.SetCursor(_cursorTextures, _fps);
            if (_setDefaultEvent == PointerTriggerEvents.OnDisable) PointerManager.Instance.SetDefaultCursor();
        }

        private void OnEnable()
        {
            if (_triggerEvent == PointerTriggerEvents.OnEnable) PointerManager.Instance.SetCursor(_cursorTextures, _fps);
            if (_setDefaultEvent == PointerTriggerEvents.OnEnable) PointerManager.Instance.SetDefaultCursor();
        }
        #endregion


        #region Component Functions

        #endregion
    }
}