using Game.Scenes;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
            Levels.LevelSceneManager.Instance.RestartLevel();
        }

        public void ShowOptions()
        {
            Options.OptionsMenu.Instance.ShowOptions();
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
        #endregion
    }
}