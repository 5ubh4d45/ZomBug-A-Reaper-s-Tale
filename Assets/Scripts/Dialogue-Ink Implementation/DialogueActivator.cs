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
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        private void OnTriggerEnter2D(Collider2D collider)
        {
            Player player = collider.GetComponent<Player>();
            if (collider.CompareTag("Player") && player != null)
            {
                player.Interactable = this;
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
                }
            }
        }
        #endregion


        #region Component Functions
        public void Interact(Player player)
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