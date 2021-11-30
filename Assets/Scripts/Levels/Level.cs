using System;
using UnityEngine;

namespace Game.Levels
{
    public class Level : MonoBehaviour
    {
        #region Singleton
        private static Level _current;
        public static Level Current
        {
            get
            {
                if (_current == null) _current = FindObjectOfType<Level>();
                if (_current == null)
                {
                    GameObject go = new GameObject("Level Instance", typeof(Level));
                    _current = go.GetComponent<Level>();
                }
                return _current;
            }
        }
        #endregion

        #region Variables
        [SerializeField] private int _levelIndex;
        [SerializeField] private Transform _playerStartPos;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        private void Start()
        {
            Player.Instance.transform.position = _playerStartPos.position;
        }

        private void OnTriggerEnter2D(Collider2D collision2D)
        {
            if (LevelManager.Instance.EnemiesAlive <= 0)
            {
                LevelManager.Instance.ChangeLevel(_levelIndex + 1);
            }
        }
        #endregion


        #region Component Functions

        #endregion
    }
}