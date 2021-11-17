using UnityEngine;

namespace Game.Score
{
    public class ScoreManager : MonoBehaviour
    {
        #region Singleton
        private static ScoreManager _instance;
        public static ScoreManager Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<ScoreManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("ScoreManager Instance", typeof(ScoreManager));
                    _instance = go.GetComponent<ScoreManager>();
                }
                return _instance;
            }
        }
        #endregion

        #region Variables
        private int _currentScore;
        private int _highScore;

        /// <summary>
        /// This event Is triggered whenever PlayerScore is increased.
        /// The float passed is the amount that was added.
        /// </summary>
        public Event<int> OnScoreAdded;

        /// <summary>
        /// This event Is triggered whenever PlayerScore is decreased.
        /// The float passed is the amount that was decreased.
        /// </summary>
        public Event<int> OnScoreReduced;

        /// <summary>
        /// This Event is triggered when the player dies Score Manager New Highscore.
        /// </summary>
        public Empty OnNewHighScore;
        #endregion


        #region Getters And Setters
        public int CurrentScore => _currentScore;
        public int HighScore => _highScore;
        #endregion


        #region Unity Calls

        #endregion


        #region Component Functions
        public void AddScore(Enemy enemy)
        {
            _currentScore += enemy.ScorePerHit;

            if (_highScore < _currentScore)
            {
                _highScore = _currentScore;
                OnNewHighScore?.Invoke();
            }

            OnScoreAdded?.Invoke(enemy.ScorePerHit);
        }

        public void AddScore(int scoreGain)
        {
            _currentScore += scoreGain;

            if (_highScore < _currentScore)
            {
                _highScore = _currentScore;
                OnNewHighScore?.Invoke();
            }

            OnScoreAdded?.Invoke(scoreGain);
        }

        public void ReduceScore(int reduceAmount)
        {
            _currentScore -= reduceAmount;
            if (_currentScore <= 0) _currentScore = 0;
            OnScoreReduced?.Invoke(reduceAmount);
        }
        #endregion
    }
}