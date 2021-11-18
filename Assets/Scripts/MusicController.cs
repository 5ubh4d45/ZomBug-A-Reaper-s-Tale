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

            //GameManager.Instance.OnGameStateChanged += UpdateMusic;
        }

        //private void OnDestroy()
        //{
        //    GameManager.Instance.OnGameStateChanged -= UpdateMusic;
        //}

        //private void UpdateMusic()
        //{
        //    if (GameManager.Instance.GameState == GameState.GAME)
        //    {
        //        _emitter.Event = "event:/MUSIC_level1";
        //        _emitter.Play();
        //    }
        //}

        // Update is called once per frame
        void Update()
        {
            _emitter.SetParameter("player_health", Player.Instance.HealthSystem.HealthPercent);
        }
    }
}
