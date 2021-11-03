using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform responseBouttonTemplate;
    [SerializeField] private RectTransform responseContainer;

    private DialogueUI dialogueUI;
    private ResponseEvent[] responseEvents;

    List<GameObject> tempResponseButtons = new List<GameObject>();

    private void Start() {

        responseBox.gameObject.SetActive(false);
        dialogueUI = GetComponent<DialogueUI>();
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents){

        this.responseEvents = responseEvents;
    }


    //takes responses
    public void ShowResponses(Response[] responses){

        responseBouttonTemplate.gameObject.SetActive(false);

        float responseBoxHeight = 0;


        for (int i = 0; i < responses.Length; i++)
        {
            Response response = responses[i];

            int responseIndex = i;


            //activates the new response textbutton
            GameObject responseButton = Instantiate(responseBouttonTemplate.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);

            //sets the response text
            responseButton.GetComponent<TMP_Text>().text = response.ResponseText;

            //gets the response button
            responseButton.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response, responseIndex));

            //add buttons to the list
            tempResponseButtons.Add(responseButton);

            //sets the response box height
            responseBoxHeight += responseBouttonTemplate.sizeDelta.y;

        }

        //sets the height of the response box
        responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responseBoxHeight);
        responseBox.gameObject.SetActive(true);
    }

    private void OnPickedResponse(Response response, int responseIndex){

        //disable the response box
        responseBox.gameObject.SetActive(false);

        //destroy the buttons
        foreach(GameObject responseButton in tempResponseButtons){
            Destroy(responseButton);
        }
        tempResponseButtons.Clear();

        if (responseEvents != null && responseIndex <= responseEvents.Length){

            responseEvents[responseIndex].OnPickedResponse?.Invoke();
        }

        responseEvents = null;

        if (response.DialogueObject){

            dialogueUI.ShowDialogue(response.DialogueObject);

        }
        else{

            dialogueUI.CloseDialogueBox();

        }


    }

}
