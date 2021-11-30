using System;
using Game.Core;
using Game.Scenes;
using UnityEngine;

namespace Game.Levels
{
    public class LevelSceneManager : MonoBehaviour
    {
        #region Singleton
        private static LevelSceneManager _instance;
        public static LevelSceneManager Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<LevelSceneManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("LevelSceneManager Instance", typeof(LevelSceneManager));
                    _instance = go.GetComponent<LevelSceneManager>();
                }
                return _instance;
            }
        }
        #endregion

        #region Variables
        [SerializeField] private SceneCollection[] _levels;
        [SerializeField] private SceneCollection _endScreen;
        [SerializeField] private CanvasGroup _transitionGroup;
        [SerializeField] private float _transitionTime;
        private int _currentLevelIndex;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        public void RestartLevel()
        {
            LoadLevel(LevelManager.Instance.CurrentLevelIndex);
        }

        public void LoadLevel(int levelIndex)
        {
            if (levelIndex >= _levels.Length)
            {
                LoadEndScreen();
                return;
            }

            _transitionGroup.gameObject.SetActive(true);
            _transitionGroup.LeanAlpha(1, _transitionTime / 2f).setOnComplete(() =>
            {
                SceneCollectionHandler.Instance.OnLoadCompelete += LevelLoadCompelete;
                SceneCollectionHandler.Instance.LoadSceneCollection(_levels[levelIndex], false, false);
            });
        }

        private void LevelLoadCompelete()
        {
            _transitionGroup.LeanAlpha(0, _transitionTime / 2f).setOnComplete(() =>
            {
                _transitionGroup.gameObject.SetActive(false);
                SceneCollectionHandler.Instance.OnLoadCompelete -= LevelLoadCompelete;
            });
            GameManager.Instance.ChangeGameState(GameState.GAME);
        }
        #endregion


        #region Component Functions
        public void LoadEndScreen(bool didWon = true)
        {
            SceneCollectionHandler.Instance.LoadSceneCollection(_endScreen);
            GameManager.Instance.ChangeGameState(GameState.END_SCREEEN);
            GameManager.Instance.DidWon = didWon;
        }
        #endregion
    }
}