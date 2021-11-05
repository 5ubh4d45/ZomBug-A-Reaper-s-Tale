using System.Collections.Generic;
using System.Collections;
using Ink.Runtime;
using TMPro;
using UnityEngine;

namespace Game.DialogueSystem
{
    /// <summary>
    /// This is the Base singleton class used for showing/hiding and managing dialogs
    /// </summary>
    [RequireComponent(typeof(TypeWriterEffect))]
    public class DialogueManager : MonoBehaviour
    {
        #region Singleton
        private static DialogueManager _instance;
        public static DialogueManager Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<DialogueManager>(true);
                if (_instance == null)
                {
                    GameObject go = new GameObject("DialogueManager Instance", typeof(DialogueManager));
                    _instance = go.GetComponent<DialogueManager>();
                }
                return _instance;
            }
        }
        #endregion


        #region Variables
        [SerializeField] private TextMeshProUGUI _dialogueText;
        [SerializeField] private Transform _responseBox;
        [SerializeField] private GameObject _responseTemplate;
        [SerializeField] private bool _useTypeWriterEffect;
        private Story _currentStory;
        private TextAsset _currentStoryAsset;
        private TypeWriterEffect _typeWriterEffect;
        private bool _isOpen;
        #endregion


        #region Getters And Setters
        /// <summary>
        /// The current Text asset thst's used to get the story object.
        /// </summary>
        public TextAsset CurrentStoryAsset
        {
            get => _currentStoryAsset;
            set
            {
                _currentStoryAsset = value;
                if (value != null) _currentStory = new Story(value.text);
                else if (value == null) Debug.LogWarning("Json File Wasn't Set");
            }
        }

        /// <summary>
        /// Is some dialog is being shown.
        /// </summary>
        public bool IsOpen => _isOpen;
        #endregion


        #region Unity Calls
        private void Awake()
        {
            _typeWriterEffect = GetComponent<TypeWriterEffect>();

            gameObject.SetActive(false);
        }
        #endregion


        #region Component Functions
        /// <summary>
        /// Show Next dialog of the current registered story. (if any)
        /// </summary>
        public void ShowNextDialogue()
        {
            if (_currentStory.canContinue)
            {
                string textToShow = _currentStory.Continue();
                Choice[] currentChoices = _currentStory.currentChoices.ToArray();
                gameObject.SetActive(true);
                if (_useTypeWriterEffect)
                {
                    _typeWriterEffect.Stop();
                    _typeWriterEffect.Run(textToShow, _dialogueText);
                }
                else
                {
                    _dialogueText.text = textToShow;
                }
                HandleChoices(currentChoices);

                _isOpen = true;
            }
            else
            {
                HideDialogue();
            }
        }

        private IEnumerator ShowChoice()
        {
            if (_currentStory.canContinue)
            {
                string textToShow = _currentStory.Continue();
                Choice[] currentChoices = _currentStory.currentChoices.ToArray();
                gameObject.SetActive(true);

                HandleChoices(currentChoices);

                _isOpen = true;

                if (_useTypeWriterEffect)
                {
                    _typeWriterEffect.Stop();
                    yield return _typeWriterEffect.Run(textToShow, _dialogueText);
                }
                else
                {
                    _dialogueText.text = textToShow;
                }
                yield return new WaitForSeconds(0.4f);
            }
            ShowNextDialogue();
        }

        private void HandleChoices(Choice[] choices)
        {
            // Clear Old Choices
            foreach (var child in _responseBox.GetComponentsInChildren<Transform>())
            {
                if (child == _responseBox) continue;
                Destroy(child.gameObject);
            }

            foreach (Choice choice in choices)
            {
                GameObject choiceGo = Instantiate(_responseTemplate);
                choiceGo.transform.SetParent(_responseBox);
                choiceGo.transform.localScale = Vector3.one;
                ChoiceObject obj = choiceGo.GetComponent<ChoiceObject>();
                obj.Initialise(choice.text, choice.index);
            }
        }

        /// <summary>
        /// Choose a choice from the currently registered choices. (through the passed index)
        /// </summary>
        public void ChooseChoice(int index)
        {
            _currentStory.ChooseChoiceIndex(index);
            StartCoroutine(ShowChoice());
        }

        /// <summary>
        /// Hide the dialog box.
        /// </summary>
        public void HideDialogue()
        {
            gameObject.SetActive(false);
            _isOpen = false;
            _currentStory = null;
            _currentStoryAsset = null;
        }
        #endregion
    }
}