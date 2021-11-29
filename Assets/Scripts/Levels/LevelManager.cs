using Game.Scenes;
using Game.Core;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Game.Levels
{
    public class LevelManager : MonoBehaviour
    {
        #region Singleton
        private static LevelManager _instance;
        public static LevelManager Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<LevelManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("LevelManager Instance", typeof(LevelManager));
                    _instance = go.GetComponent<LevelManager>();
                }
                return _instance;
            }
        }
        #endregion

        #region Variables
        private int _activeEnemiesCount;
        private int _currentLevel = 0;

        /// <summary>
        /// This Event is triggered when the player Finishes A level by killing all enemies.
        /// </summary>
        public Event<int> OnLevelFinish;
        #endregion


        #region Getters And Setters
        public int EnemiesAlive => _activeEnemiesCount;
        public int CurrentLevelIndex { get => _currentLevel; set => _currentLevel = value; }
        #endregion


        #region Unity Calls
        private void Start()
        {
            GameManager.Instance.OnGameStateChanged += UpdateState;
        }
        #endregion


        #region Component Functions

        private void UpdateState()
        {
            if (GameManager.Instance.GameState != GameState.GAME)
            {
                _activeEnemiesCount = 0;
            }
        }

        public void SkipLevel()
        {
            if (GameManager.Instance.GameState != GameState.GAME) return;

            GameObject.FindObjectsOfType<Enemy>().ToList().ForEach((Enemy enemy) =>
            {
                enemy.HealthSystem.Damage(enemy.HealthSystem.MaxHealth);
            });
        }

        public void RegisterEnemy() => _activeEnemiesCount++;
        public void UnregisterEnemy()
        {
            _activeEnemiesCount--;
            if (_activeEnemiesCount <= 0)
            {
                _currentLevel++;
                _activeEnemiesCount = 0;
                OnLevelFinish?.Invoke(_currentLevel);
            }
        }
        #endregion
    }

    public enum LevelType
    {

        LEVEL_1,
        LEVEL_2,
        LEVEL_3,
        LEVEL_4,
        LEVEL_5,
        LEVEL_6,
    }
}