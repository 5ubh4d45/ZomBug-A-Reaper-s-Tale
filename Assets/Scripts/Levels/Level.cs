using System;
using Game.Core;
using Game.DialogueSystem;
using Ink.Runtime;
using UnityEngine;
using FMODUnity;

namespace Game.Levels
{
    public class Level : MonoBehaviour
    {
        #region Variables
        [SerializeField] private TextAsset _catDialogs;
        [SerializeField] private int _levelIndex;
        [SerializeField] private bool _isBossLevel;
        [SerializeField] private Transform _playerStartPos;
        [SerializeField] private StudioEventEmitter _bossMusicEmitter;

        private bool _hasFinished => _hasMetCat && _hasKilledEnemies;
        private bool _hasMetCat;
        private bool _hasKilledEnemies;

        public StudioEventEmitter BossEventEmitter => _bossMusicEmitter;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        private void Start()
        {
            if (_isBossLevel)
            {
                MusicController.Instance.Emitter.Stop();
                _bossMusicEmitter.Play();
            }
            LevelManager.Instance.OnEnemiesKilled += EnemiesKilled;
            Player.Instance.transform.position = _playerStartPos.position;
        }

        private void Update()
        {
            if (_isBossLevel)
                _bossMusicEmitter.SetParameter("player_health", Player.Instance.HealthSystem.HealthPercent);
        }

        private void OnDestroy()
        {
            LevelManager.Instance.OnEnemiesKilled -= EnemiesKilled;
        }

        private void EnemiesKilled(int _)
        {
            _hasKilledEnemies = true;
            // if (_isBossLevel)
            // {
            //     _hasMetCat = true;
            //     LevelManager.Instance.ChangeLevel(_levelIndex + 1);
            //     return;
            // }
            DialogueManager.Instance.OnDialogueEnd += DialogueEnded;
        }

        private void DialogueEnded(Story story)
        {
            if (!(story == _catDialogs)) return;
            _hasMetCat = true;
            DialogueManager.Instance.OnDialogueEnd -= DialogueEnded;

            if (_isBossLevel)
            {
                LevelManager.Instance.ChangeLevel(_levelIndex + 1);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.gameObject.CompareTag("Player") && _hasFinished)
            {
                LevelManager.Instance.ChangeLevel(_levelIndex + 1);
            }
        }
        #endregion


        #region Component Functions

        #endregion
    }
}