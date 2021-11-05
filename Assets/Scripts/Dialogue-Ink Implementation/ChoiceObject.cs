using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.DialogueSystem
{
    /// <summary>
    /// This Is the object used to setup a choice.
    /// </summary>
    public class ChoiceObject : MonoBehaviour, IPointerDownHandler
    {
        #region Variables
        private TextMeshProUGUI _label;
        private int _index;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            DialogueManager.Instance.ChooseChoice(_index);
        }
        #endregion


        #region Component Functions
        /// <summary>
        /// Initialise the Choice object.
        /// </summary>
        public void Initialise(string text, int index)
        {
            _label = transform.GetComponentInChildren<TextMeshProUGUI>(true);
            _label.text = text;
            _index = index;
        }
        #endregion
    }
}