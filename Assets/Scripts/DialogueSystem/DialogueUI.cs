using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    
    public bool IsOpen {get; private set;}

    private TypewriterEffect typewriterEffect;
    private ResponseHandler responseHandler;


    private void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();

        CloseDialogueBox();
        
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        //enabling the dialogue box
        dialogueBox.SetActive(true);

        IsOpen = true;

        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents){

        responseHandler.AddResponseEvents(responseEvents);
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++){

            string dialogue = dialogueObject.Dialogue[i];

            yield return RunTypingEffect(dialogue);

            textLabel.text = dialogue;

            //check for if it has completed all dialogues or has responses and if it has, breeak off the loop
            if(i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

            //wait for next line
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        }

        //chccls if the dialogue bos has responses, if yes, it shows responses
        if (dialogueObject.HasResponses){

            responseHandler.ShowResponses(dialogueObject.Responses);
            
        } else{

            //closing the dialogue box after all dialogues
            CloseDialogueBox();
        }
    }

    private IEnumerator RunTypingEffect(string dialogue){

        typewriterEffect.Run(dialogue, textLabel);

        while (typewriterEffect.IsRunning){

            yield return null;

            if (Input.GetKeyDown(KeyCode.Space)){

                typewriterEffect.Stop();

                yield return null;
            }
        }
    }

    public void CloseDialogueBox()
    {   
        IsOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}
