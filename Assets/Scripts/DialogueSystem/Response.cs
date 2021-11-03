using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Response    //contains the responces
{
    [SerializeField] private string responseText;
    [SerializeField] private DialogueObject dialogueObject;

    
    //setting up getters
    public string ResponseText => responseText;
    public DialogueObject DialogueObject => dialogueObject;


}
