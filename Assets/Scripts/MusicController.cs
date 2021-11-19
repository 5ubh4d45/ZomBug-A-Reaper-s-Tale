using System.Collections;
using System.Collections.Generic;
using Game.HealthSystem;
using FMODUnity;
using UnityEngine;
using System;

namespace Game.Core
{
    public class MusicController : MonoBehaviour
    {

        private StudioEventEmitter _emitter;
        // Start is called before the first frame update
        void Awake()
        {
            //Get the emitter on this game object
            _emitter = GetComponent<StudioEventEmitter>();

            // Subscribe to the Game State changed event. 
            // To make sure the emitter only plays when we are in the game scene.
            GameManager.Instance.OnGameStateChanged += UpdateMusic;
        }

        private void OnDestroy()
        {
            //Unsubscribe to the Game state changed event to avoid null reference exceptions
            GameManager.Instance.OnGameStateChanged -= UpdateMusic;
        }

        private void UpdateMusic()
        {
            if (GameManager.Instance.GameState == GameState.GAME)
            {
                //The new game state is the Game scene, so play the emitter.
                _emitter.Play();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.GameState == GameState.GAME)
            {
                // The game state is the Game Scene. which means the player health is available to us. so update it.
                _emitter.SetParameter("player_health", Player.Instance.HealthSystem.HealthPercent);
            }
        }
    }
}
