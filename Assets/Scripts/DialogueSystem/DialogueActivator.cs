using UnityEngine;


//attached to the npc or objects which then displays the dialogues
public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private SpriteRenderer dialoguePopUP;

    public void UpdateDialogueObject(DialogueObject dialogueObject)
    {

        this.dialogueObject = dialogueObject;
    }


    private void Start()
    {
        dialoguePopUP.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        //checks if the object enter is tagged with Player & has Player component
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {

            player.Interactable = this;

            ShowPopUp();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //checks if the object enter is tagged with Player & has Player component
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {

            //now if its the current interactable, when exiting and resets the interactble
            if (player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
            {

                player.Interactable = null;

                ClosePopUp();
            }
        }
    }

    //activates the dialogue box and feeds the dialoguData whwnever activated
    public void Interact(Player player)
    {

        //checks for every responses available, then matches with the corresponding dialogue object
        foreach (DialogueResponseEvents responseEvents in GetComponents<DialogueResponseEvents>())
        {

            if (responseEvents.DialogueObject == dialogueObject)
            {

                // player.DialogueUI.AddResponseEvents(responseEvents.Events);
                break;
            }
        }

        // player.DialogueUI.ShowDialogue(dialogueObject);
    }

    private void ShowPopUp()
    {
        dialoguePopUP.gameObject.SetActive(true);
    }
    private void ClosePopUp()
    {
        dialoguePopUP.gameObject.SetActive(false);
    }


}

