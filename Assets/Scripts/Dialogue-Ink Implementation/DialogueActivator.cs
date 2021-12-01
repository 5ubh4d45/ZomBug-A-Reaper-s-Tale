using Ink.Runtime;
using UnityEngine;

namespace Game.DialogueSystem
{
    /// <summary>
    /// Attach this class to Objects that are supposed to trigger a ink File.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class DialogueActivator : MonoBehaviour, IInteractable
    {
        #region Variables
        [SerializeField] private TextAsset _storyAsset;

        // [SerializeField] private SpriteRenderer _popUP;
        #endregion
        

        #region Getters And Setters
        public Story Story => new Story(_storyAsset.text);
        #endregion

        // private void Start()
        // {
        //     if (_popUP == null)
        //     {
        //         _popUP = GetComponentInChildren<SpriteRenderer>();
        //     }
        //
        //     _popUP.enabled = false;
        // }


        #region Unity Calls
        private void OnTriggerEnter2D(Collider2D collider)
        {
            Player player = collider.GetComponent<Player>();
            if (collider.CompareTag("Player") && player != null)
            {
                player.Interactable = this;
                
                
                // enables the pop UP
                // _popUP.enabled = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            //checks if the object enter is tagged with Player & has Player component
            if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
            {

                //now if its the current interactable, when exiting and resets the interactble
                if (player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this && DialogueManager.Instance.CurrentStoryAsset != _storyAsset)
                {

                    player.Interactable = null;
                    
                    // disables the pop UP
                    // _popUP.enabled = false;
                }
            }
        }
        #endregion


        #region Component Functions
        public void Interact(Player player)
        {
            ShowDialog();
        }

        public void ShowDialog()
        {
            if (DialogueManager.Instance.CurrentStoryAsset != _storyAsset && !DialogueManager.Instance.IsOpen)
            {
                DialogueManager.Instance.CurrentStoryAsset = _storyAsset;
            }
            DialogueManager.Instance.ShowNextDialogue();
        }
        
        #endregion
    }
}