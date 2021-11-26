using UnityEngine;

namespace Game.End
{
    public class CameraHover : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float _speed;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        private void Update()
        {
            // Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            Vector3 randomDir = (new Vector3(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f), 0) * 2);
            Vector3 newPos = transform.position + (randomDir * _speed);
            transform.position = Vector3.Lerp(transform.position, newPos, _speed * Time.deltaTime);
        }
        #endregion


        #region Component Functions

        #endregion
    }
}