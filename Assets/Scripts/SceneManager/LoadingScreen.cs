using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.Scenes
{
    public class LoadingScreen : MonoBehaviour
    {
        #region Variables
        [SerializeField] private TMP_Text _loadingText;
        [SerializeField] private float _perDotWait;
        [SerializeField] private int _maxDots;
        private int _dotCount;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        private void Awake()
        {
            StartCoroutine(WriteEffect());
        }

        private IEnumerator WriteEffect()
        {
            while (true)
            {
                if (_dotCount >= _maxDots)
                {
                    _loadingText.text = "Loading";
                }
                _loadingText.text += ".";
                _dotCount++;
                yield return new WaitForSeconds(.9f);
            }
        }
        #endregion


        #region Component Functions

        #endregion
    }
}