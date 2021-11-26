using UnityEngine;
using Game.HealthSystem;
using Game.Score;
using Game.Pointer;

public class Enemy : HealthObject<IntHealthSystem>
{
    #region Variables
    [Tooltip("The Amount of Score to be added for every enemy of this type")]
    [SerializeField] private int _scorePerHit;

    private float _delay;
    #endregion


    #region Getters And Setters
    public int ScorePerHit => _scorePerHit;
    #endregion


    #region Unity Calls

    #endregion


    #region Component Functions
    public override void OnDead()
    {
        PointerManager.Instance.SetDefaultCursor();
        
        DeadSetUp();

        Destroy(this.gameObject, _delay);
    }

    public override void OnDamaged(float damageAmount)
    {
        ScoreManager.Instance.AddScore(_scorePerHit);
    }

    private void DeadSetUp()
    {
        
        //playing the death animation and gets the death animation duration
        var anim = GetComponent<EnemyAnimator>();
        anim.PlayDeathAnimation();

        _delay = anim.DeathAnimationTime;
        
        GetComponent<Collider2D>().enabled = false;
        var collider2Ds = GetComponentsInChildren<Collider2D>();
        foreach (var collider2D in collider2Ds)
        {
            collider2D.enabled = false;
        }

    }
    #endregion
}