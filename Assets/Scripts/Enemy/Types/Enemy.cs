using UnityEngine;
using Game.HealthSystem;
using Game.Score;
using Game.Pointer;
using Game.Levels;

public class Enemy : HealthObject<IntHealthSystem>
{
    #region Variables
    [Tooltip("The Amount of Score to be added for every enemy of this type")]
    [SerializeField] private int _scorePerHit;
    #endregion


    #region Getters And Setters
    public int ScorePerHit => _scorePerHit;
    #endregion


    #region Unity Calls
    protected override void Awake()
    {
        base.Awake();
        LevelManager.Instance.RegisterEnemy();
    }
    #endregion


    #region Component Functions
    public override void OnDead()
    {
        PointerManager.Instance.SetDefaultCursor();
        LevelManager.Instance.UnregisterEnemy();
        Destroy(this.gameObject);
    }

    public override void OnDamaged(float damageAmount)
    {
        ScoreManager.Instance.AddScore(_scorePerHit);
    }
    #endregion
}