using System.Collections;
using System.Collections.Generic;
using Game.HealthSystem;
using FMODUnity;
using UnityEngine;



public class MusicController : MonoBehaviour
{
    
    private StudioEventEmitter _emitter;
    // Start is called before the first frame update
    void Start()
    {
        //Get the emitter on this game object
        _emitter = GetComponent<StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        //Update FMOD Event Emitter with current HealthPercent
        _emitter.SetParameter("player_health", IntHealthSystem.HealthPercentage);
    }
}
