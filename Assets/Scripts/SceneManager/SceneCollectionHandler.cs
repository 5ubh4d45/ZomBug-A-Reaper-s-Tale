using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Scenes
{
    public class SceneCollectionHandler : MonoBehaviour
    {
        #region Singleton
        private static SceneCollectionHandler _instance;
        public static SceneCollectionHandler Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<SceneCollectionHandler>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("SceneManager Instance", typeof(SceneCollectionHandler));
                    _instance = go.GetComponent<SceneCollectionHandler>();
                }
                return _instance;
            }
        }
        #endregion

        #region Variables
        [SerializeField] private SceneCollection[] _sceneCollections;
        [SerializeField] private int _defaultSceneIndex;

        [Space]

        [Header("Loading Screen")]
        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private Slider _progressBar;

        private SceneCollection _lastCollection;
        private SceneCollection _currentCollection;
        private List<AsyncOperation> _operations;
        private bool _isLoading;

        public Empty OnLoadCompelete;
        #endregion


        #region Getters And Setters
        public SceneCollection[] SceneCollections => _sceneCollections;
        public SceneCollection CurrentCollection => _currentCollection;
        public bool IsLoading => _isLoading;
        #endregion


        #region Unity Calls
        private void Awake()
        {
            LoadSceneCollection(_defaultSceneIndex);
        }
        #endregion


        #region Component Functions
        public void LoadSceneCollection(int collectionIndex)
        {
            _operations = new List<AsyncOperation>();
            if (collectionIndex > _sceneCollections.Length || collectionIndex == -1) return;
            if (_sceneCollections[collectionIndex] == null) return;

            _loadingScreen.SetActive(true);

            SceneCollection collection = _sceneCollections[collectionIndex];
            _lastCollection = _currentCollection;


            foreach (var sceneRef in collection.SceneReferences)
            {
                string sceneName = sceneRef.ScenePath.Split('/').Last();
                sceneName = sceneName.Substring(0, sceneName.Length - 6);
                AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

                _operations.Add(operation);
            }
            UnloadCurrentLoadedCollection(false);

            _currentCollection = collection;
            StartCoroutine(UpdateSceneLoadProgress());
        }

        public void LoadSceneCollection(SceneCollection collection)
        {
            _operations = new List<AsyncOperation>();
            if (collection == null) return;

            _loadingScreen.SetActive(true);
            _lastCollection = _currentCollection;


            foreach (var sceneRef in collection.SceneReferences)
            {
                string sceneName = sceneRef.ScenePath.Split('/').Last();
                sceneName = sceneName.Substring(0, sceneName.Length - 6);
                AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

                _operations.Add(operation);
            }

            UnloadCurrentLoadedCollection(false);

            _currentCollection = collection;
            StartCoroutine(UpdateSceneLoadProgress());
        }

        public void LoadSceneCollection(SceneCollection collection, bool forceUnload = false, bool updateSceneLoadProgress = true)
        {
            if (collection == null) return;

            _loadingScreen.SetActive(updateSceneLoadProgress);
            _lastCollection = _currentCollection;


            foreach (var sceneRef in collection.SceneReferences)
            {
                string sceneName = sceneRef.ScenePath.Split('/').Last();
                sceneName = sceneName.Substring(0, sceneName.Length - 6);
                AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

                _operations.Add(operation);
            }

            UnloadCurrentLoadedCollection(forceUnload, updateSceneLoadProgress);

            _currentCollection = collection;
            StartCoroutine(UpdateSceneLoadProgress());
        }

        public IEnumerator UpdateSceneLoadProgress()
        {
            _isLoading = true;
            foreach (var operation in _operations)
            {
                while (!operation.isDone)
                {
                    float progress = 0;
                    _operations.ForEach((AsyncOperation oepration) =>
                    {
                        progress += operation.progress / _operations.Count;
                    });

                    _progressBar.value = progress;

                    yield return null;
                }

            }

            _loadingScreen.SetActive(false);
            _isLoading = false;

            SceneReference activeSceneRef = _currentCollection.SceneReferences[_currentCollection.ActiveSceneIndex];
            Scene activeScene = SceneManager.GetSceneByPath(activeSceneRef.ScenePath);
            SceneManager.SetActiveScene(activeScene);

            Pointer.PointerManager.Instance.SetCursorSize(_currentCollection.CursorSize);

            OnLoadCompelete?.Invoke();
        }

        public void UnloadCurrentLoadedCollection(bool forceUnload = false, bool updateProgress = true)
        {
            if (_currentCollection == null) return;

            foreach (var sceneRef in _currentCollection.SceneReferences)
            {
                if (sceneRef.IsPersistent && !forceUnload) continue;

                Scene scene = SceneManager.GetSceneByPath(sceneRef.ScenePath);
                AsyncOperation operation = SceneManager.UnloadSceneAsync(scene.buildIndex);

                _operations.Add(operation);
            }

            _currentCollection = null;
            if (updateProgress) StartCoroutine(UpdateSceneLoadProgress());
        }
        #endregion
    }
}