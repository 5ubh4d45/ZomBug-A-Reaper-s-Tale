using System;
using Game.Core;
using Game.Scenes;
using Game.Score;
using UnityEngine;

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
        #endregion


        #region Getters And Setters
        public bool IsShown => _isShown;
        #endregion


        #region Unity Calls
        private void Awake()
        {
            _optionsCanvas = GetComponent<Canvas>();
            SceneCollectionHandler.Instance.OnLoadCompelete += UpdateCamera;
            if (_root == null) _root = gameObject;
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

        private void UpdateCamera()
        {
            _optionsCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            _optionsCanvas.worldCamera = Camera.main;
        }
        #endregion
    }
}