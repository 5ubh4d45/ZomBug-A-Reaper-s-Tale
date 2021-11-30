using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Levels
{
    public class CatNPC : MonoBehaviour
    {
        #region Variables
        private SpriteRenderer _renderer;
        private List<Collider2D> _colliders;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _colliders = GetComponents<Collider2D>().ToList();
            _renderer.enabled = false;
            _colliders.ForEach((Collider2D collider) => { collider.enabled = false; });

            LevelManager.Instance.OnEnemiesKilled += EnemiesKilled;
        }

        private void OnDestroy()
        {
            LevelManager.Instance.OnEnemiesKilled -= EnemiesKilled;
        }

        private void EnemiesKilled(int level)
        {
            _renderer.enabled = true;
            _colliders.ForEach((Collider2D collider) => { collider.enabled = true; });
        }
        #endregion


        #region Component Functions

        #endregion
    }
}