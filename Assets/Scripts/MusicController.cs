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
        #region Singleton
        private static MusicController _instance;
        public static MusicController Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<MusicController>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("MusicController Instance", typeof(MusicController));
                    _instance = go.GetComponent<MusicController>();
                }
                return _instance;
            }
        }
        #endregion

        private StudioEventEmitter _emitter;

        public StudioEventEmitter Emitter => _emitter;

        // Start is called before the first frame update
        void Awake()
        {
            //Get the emitter on this game object
            _emitter = GetComponent<StudioEventEmitter>();

            // Subscribe to the Game State changed event. 
            // To make sure the emitter only plays when we are in the game scene.
            GameManager.Instance.OnGameStateChanged += UpdateMusic;
            Player.Instance.HealthSystem.OnHealed += PlayHealFX;
            Player.Instance.HealthSystem.OnDead += PlayDeathFX;
        }

        private void PlayDeathFX()
        {
            //play dying sound
            RuntimeManager.PlayOneShot("event:/SFX_player_die");
        }

        private void PlayHealFX(float healAmount)
        {
            //play heal sound
            RuntimeManager.PlayOneShot("event:/SFX_heal");
        }

        private void OnDestroy()
        {
            //Unsubscribe to the Game state changed event to avoid null reference exceptions
            GameManager.Instance.OnGameStateChanged -= UpdateMusic;
            Player.Instance.HealthSystem.OnHealed -= PlayHealFX;
        }

        private void UpdateMusic()
        {
            if (GameManager.Instance.GameState == GameState.GAME)
            {
                //The new game state is the Game scene, so play the emitter.
                _emitter.Play();
            }
            else if (GameManager.Instance.GameState != GameState.GAME)
            {
                _emitter.Stop();
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
