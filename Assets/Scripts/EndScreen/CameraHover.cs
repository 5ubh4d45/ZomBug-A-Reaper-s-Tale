using UnityEngine;

namespace Game.End
{
    public class CameraHover : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float _speed;
        [SerializeField] private Transform _background;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        private void Update()
        {
            // Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            Vector3 randomDir = (new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0));
            Vector3 newPos = transform.position - (randomDir * _speed) + new Vector3(78.6f, 66.9f, 0);
            _background.position = Vector3.Lerp(_background.position, newPos, _speed * Time.deltaTime);
        }
        #endregion


        #region Component Functions

        #endregion
    }
}