using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueData")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] [TextArea] private string[] dialogue;
    [SerializeField] private Response[] responses;


    //setting up getters
    public string[] Dialogue => dialogue;

    //checks if its has null or blank responses
    public bool HasResponses => Responses != null && Responses.Length > 0;
    public Response[] Responses => responses;

    
}
