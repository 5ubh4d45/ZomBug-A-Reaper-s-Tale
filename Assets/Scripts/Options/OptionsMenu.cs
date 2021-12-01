using System;
using Game.Core;
using Game.Scenes;
using Game.Score;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

namespace Game.Options
{
    public class OptionsMenu : MonoBehaviour
    {
        #region Singleton
        private static OptionsMenu _instance;
        public static OptionsMenu Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<OptionsMenu>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("OptionsMenu Instance", typeof(OptionsMenu));
                    _instance = go.GetComponent<OptionsMenu>();
                }
                return _instance;
            }
        }
        #endregion

        #region Variables
        [SerializeField] private GameObject _root;
        [SerializeField] private SceneCollection _homeScreen;
        private Canvas _optionsCanvas;
        private bool _isShown;

        private Bus _masterBus;
        private Bus _playerFxBus;
        private Bus _enemyFxBus;
        private Bus _musicBus;

        // private float _masterVolume = 1f;
        // private float _playerFxVolume = 1f;
        // private float _enemyFxVolume = 1f;
        // private float _musicVolume = 1f;
        #endregion


        #region Getters And Setters
        public bool IsShown => _isShown;
        #endregion


        #region Unity Calls
        private void Awake()
        {
            _optionsCanvas = GetComponent<Canvas>();
            if (_root == null) _root = gameObject;

            _masterBus = RuntimeManager.GetBus("bus:/Master");
            _playerFxBus = RuntimeManager.GetBus("bus:/Master/Player Fx");
            _enemyFxBus = RuntimeManager.GetBus("bus:/Master/Enemy Fx");
            _musicBus = RuntimeManager.GetBus("bus:/Master/Music");
        }
        #endregion


        #region Component Functions
        public void ShowOptions()
        {
            _isShown = true;
            Time.timeScale = 0;
            _root.SetActive(true);
        }

        public void HideOptions()
        {
            _isShown = false;
            Time.timeScale = 1;
            _root.SetActive(false);
        }

        public void Exit()
        {
            Player.Instance.Reset();
            ScoreManager.Instance.Reset();
            SceneCollectionHandler.Instance.LoadSceneCollection(_homeScreen);
            GameManager.Instance.ChangeGameState(GameState.HOME_SCREEN);
            HideOptions();
        }

        public void UpdatePlayerFxVolume(float volume)
        {
            _playerFxBus.setVolume(volume);
        }

        public void UpdateEnemyFxVolume(float volume)
        {
            _enemyFxBus.setVolume(volume);
        }

        public void UpdateMasterVolume(float volume)
        {
            _masterBus.setVolume(volume);
        }

        public void UpdateMusicVolume(float volume)
        {
            _musicBus.setVolume(volume);
        }
        #endregion
    }
}