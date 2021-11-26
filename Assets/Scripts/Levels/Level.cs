using UnityEngine;

namespace Game.Levels
{
    public class Level : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Transform _playerStartPos;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        private void Start()
        {
            Player.Instance.transform.position = _playerStartPos.position;
        }
        #endregion


        #region Component Functions

        #endregion
    }
}