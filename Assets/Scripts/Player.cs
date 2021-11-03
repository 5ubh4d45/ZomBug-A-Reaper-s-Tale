using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this class will contain all the functionalities
public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private DialogueUI dialogueUI;

    public float MoveSpeed => moveSpeed;
    public DialogueUI DialogueUI => dialogueUI;
    public IInteractable Interactable {get; set;}



    private void Update() {

        if (Input.GetKeyDown(KeyCode.E)){

            if (Interactable != null){
                Interactable.Interact(this);
            }
        }
    }
}
