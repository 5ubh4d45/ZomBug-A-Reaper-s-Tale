using System.Collections;
using System.Collections.Generic;
using EnemyBehavior;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private EnemyBehavior.EnemyBehavior behavior;
    [SerializeField] private float deathAnimationTIme;

    public float DeathAnimationTime => deathAnimationTIme;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (behavior == null)
        {
            behavior = GetComponent<EnemyBehavior.EnemyBehavior>();
        }

        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
    }

    private void FixedUpdate()
    {
        FlipCheck();
    }

    private void FlipCheck()
    {
        if (behavior.IsFacingRight)
        {
            transform.localScale = new Vector3(1f, 1f ,1f);
        }
        
        if (!behavior.IsFacingRight)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        
    }

    public void PlayIdle()
    {
        anim.SetBool("CanWalk", false);
    }

    public void PlayWalk()
    {
        anim.SetBool("CanWalk", true);
    }

    public void PlayMeleeAttack()
    {
        anim.SetTrigger("MeleeAttack");
    }

    public void PlayRangedAttack()
    {
        anim.SetTrigger("RangedAttack");
    }

    public void PlayDeathAnimation()
    {
        anim.SetTrigger("Death");
    }
    
}
