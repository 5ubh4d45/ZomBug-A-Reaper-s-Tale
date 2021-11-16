using Game.Scenes;
using UnityEngine;

namespace Game.Tests
{
    public class SceneTestScript : MonoBehaviour
    {
        #region Variables
        [SerializeField] private SceneCollection _gameCollection;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls

        #endregion


        #region Component Functions
        public void LoadGame()
        {
            SceneCollectionHandler.Instance.LoadSceneCollection(_gameCollection);
        }
        #endregion
    }
}