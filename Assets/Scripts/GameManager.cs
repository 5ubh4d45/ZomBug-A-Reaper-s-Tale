using UnityEngine;

namespace Game.Core
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager Instance", typeof(GameManager));
                    _instance = go.GetComponent<GameManager>();
                }
                return _instance;
            }
        }
        #endregion

        #region Variables
        [SerializeField] private GameState _defaultState;

        private GameState _gameState = GameState.NONE;

        public Empty OnGameStateChanged;
        #endregion


        #region Getters And Setters
        public GameState GameState => _gameState;
        public bool DidWon;
        #endregion


        #region Unity Calls
        private void Awake()
        {
            ChangeGameState(_defaultState);
        }
        #endregion


        #region Component Functions
        public void ChangeGameState(GameState gameState)
        {
            if (_gameState == gameState) return;
            _gameState = gameState;
            OnGameStateChanged?.Invoke();
        }
        #endregion
    }
}