using UnityEngine;

public class EventSystemChecker : MonoBehaviour
{
    private static EventSystemChecker _instance;
    
    //simply sures that only one eventSystem stay on a scene 
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
             Destroy(gameObject);
        }
    }
}
