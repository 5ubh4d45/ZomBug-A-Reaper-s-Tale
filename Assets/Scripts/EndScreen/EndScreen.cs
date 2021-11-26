using Game.Core;
using Game.Scenes;
using Game.Score;
using TMPro;
using UnityEngine;

namespace Game.End
{
    public class EndScreen : MonoBehaviour
    {
        #region Variables
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private TextMeshProUGUI _highScore;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private string _dieTitle;
        [SerializeField] private string _winTitle;
        [SerializeField] private SceneCollection _homeScreen;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        private void Start()
        {
            _score.text = "Score: " + ScoreManager.Instance.CurrentScore.ToString();
            _highScore.text = "HighScore: " + ScoreManager.Instance.HighScore.ToString();

            _title.text = GameManager.Instance.DidWon ? _winTitle : _dieTitle;
            GameManager.Instance.DidWon = false;
        }
        #endregion


        #region Component Functions
        public void LoadHomeScreen()
        {
            Player.Instance.Reset();
            ScoreManager.Instance.Reset();
            SceneCollectionHandler.Instance.LoadSceneCollection(_homeScreen);
            GameManager.Instance.ChangeGameState(GameState.HOME_SCREEN);
        }
        #endregion
    }
}