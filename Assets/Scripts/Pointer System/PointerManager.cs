using UnityEngine;
using UnityEngine.UI;

namespace Game.Pointer
{
    public class PointerManager : MonoBehaviour
    {
        #region Singleton
        private static PointerManager _instance;
        public static PointerManager Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<PointerManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("PointerManager Instance", typeof(PointerManager));
                    _instance = go.GetComponent<PointerManager>();
                }
                return _instance;
            }
        }
        #endregion


        #region Variables
        [SerializeField] private Sprite[] _defaultCursors;
        [SerializeField] private int _defaultFps;
        [SerializeField] private float _cursorSpeed = 40f;

        private float _frameTimer;
        private int _currentFrame;
        private Image _renderer;
        private RectTransform _rectTranform;
        private Sprite[] _cursorTextures;
        private int _fps;
        #endregion


        #region Getters And Setters
        private int _frameCount => _cursorTextures.Length;
        private float _frameRate => 1f / _fps;
        #endregion


        #region Unity Calls
        private void Start()
        {
            _cursorTextures = _defaultCursors;
            _fps = _defaultFps;
            _renderer = GetComponent<Image>();
            _rectTranform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            Cursor.visible = false;
            _frameTimer -= Time.deltaTime;
            if (_frameTimer <= 0)
            {
                _frameTimer += _frameRate;
                _currentFrame = (_currentFrame + 1) % _frameCount;
                _renderer.sprite = _cursorTextures[_currentFrame];
            }
            Vector3 cursorPos = Input.mousePosition;
            cursorPos = new Vector3(cursorPos.x, cursorPos.y, 0);

            float pointX = cursorPos.x / Screen.width;
            float pointY = cursorPos.y / Screen.height;

            _rectTranform.position = Vector3.Lerp(transform.position, cursorPos, _cursorSpeed);
        }
        #endregion


        #region Component Functions
        public void SetCursor(Sprite[] textures, int fps)
        {
            _cursorTextures = textures;
            _fps = fps;
        }

        public void SetDefaultCursor()
        {
            SetCursor(_defaultCursors, _defaultFps);
        }

        public void SetCursorSize(float size)
        {
            _renderer.transform.localScale = new Vector2(size, size);
        }
        #endregion
    }
}