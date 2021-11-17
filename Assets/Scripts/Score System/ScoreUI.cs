using System;
using Game.Core;
using TMPro;
using UnityEngine;

namespace Game.Score
{
    public class ScoreUI : MonoBehaviour
    {
        #region Variables
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _highScoreText;
        [SerializeField] private GameObject _scoreCanvas;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        private void Awake()
        {
            GameManager.Instance.OnGameStateChanged += UpdateActiveState;
            ScoreManager.Instance.OnScoreAdded += ScoreAdded;
            ScoreManager.Instance.OnScoreReduced += ScoreReduced;
            ScoreManager.Instance.OnNewHighScore += HighScoreChanged;

            UpdateActiveState();
            UpdateHighScore();
            UpdateScore(0);
        }
        #endregion


        #region Component Functions
        private void UpdateActiveState()
        {
            if (GameManager.Instance.GameState == GameState.GAME) _scoreCanvas.SetActive(true);
            else _scoreCanvas.SetActive(false);
        }

        private void ScoreAdded(int addAmount)
        {
            UpdateScore(addAmount);
        }

        private void ScoreReduced(int reduceAmount)
        {
            UpdateScore(reduceAmount);
        }

        private void HighScoreChanged()
        {
            UpdateHighScore();
        }

        private void UpdateHighScore()
        {
            _highScoreText.text = "HighScore: " + ScoreManager.Instance.HighScore.ToString();
        }

        private void UpdateScore(float changeAmount)
        {
            _scoreText.text = "Score: " + ScoreManager.Instance.CurrentScore.ToString();
        }
        #endregion
    }
}