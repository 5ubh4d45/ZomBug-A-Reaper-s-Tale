using UnityEngine;
using System;


//attaches to the npc/dialogue gameobjects and handles the responses
public class DialogueResponseEvents : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private ResponseEvent[] events;

    public DialogueObject DialogueObject => dialogueObject;

    public ResponseEvent[] Events => events;

    public void OnValidate() {
        
        //returns if there are no responses
        if (dialogueObject == null) return;
        if (dialogueObject.Responses == null) return;


        //returns if already validated
        // if (events != null && events.Length == dialogueObject.Responses.Length) return;

        //if there are no events create a new Response events array 
        if (events == null){

            events = new ResponseEvent[dialogueObject.Responses.Length];
        }
        else //if has one resizes it to match dialogue reesponse length
        {
            Array.Resize(ref events, dialogueObject.Responses.Length);
        }


        //validates the responses
        for (int i = 0; i < dialogueObject.Responses.Length; i++){

            Response response = dialogueObject.Responses[i];

            if (events[i] != null){

                events[i].name = response.ResponseText; //setting the Response event name
                continue;
            }
        }

    }


}
