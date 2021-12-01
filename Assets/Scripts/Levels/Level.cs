using System;
using Game.DialogueSystem;
using Ink.Runtime;
using UnityEngine;

namespace Game.Levels
{
    public class Level : MonoBehaviour
    {
        #region Variables
        [SerializeField] private TextAsset _catDialogs;
        [SerializeField] private int _levelIndex;
        [SerializeField] private bool _isBossLevel;
        [SerializeField] private Transform _playerStartPos;

        private bool _hasFinished => _hasMetCat && _hasKilledEnemies;
        private bool _hasMetCat;
        private bool _hasKilledEnemies;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        private void Start()
        {
            LevelManager.Instance.OnEnemiesKilled += EnemiesKilled;
            Player.Instance.transform.position = _playerStartPos.position;
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