using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEditor.VersionControl;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public enum BossState
    {
        Idle,
        BackOff,
        Chase,
        RangedAttack,
        MeleeAttack,
        Dead
    }

    #region InspectorVariables

    [Header("Required Componenets")]
    [SerializeField] private CatBoss boss;
    
    #endregion

    #region PrivateVariables

    public BossState _state;

    #endregion

    #region GettersSetters
    

    #endregion
    
    
    // Start is called before the first frame update
    private void Start()
    {   
        //setting up components
        if (boss == null)
        {
            boss = GetComponent<CatBoss>();
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        //boss state machine
        switch (_state)
        {
            case BossState.Idle:
                
                //idle animations here
                boss.BossAnimator.PlayIdle();

                _state = BossState.Chase;
                break;
            
            case BossState.Chase:

                
                if (boss.BossCombat.InAttackRange)
                {
                    _state = BossState.RangedAttack;
                }

                if (boss.BossCombat.InBackOffRange)
                {
                    _state = BossState.BackOff;
                }
                
                if (boss.BossCombat.InMeleeRange)
                {
                    _state = BossState.MeleeAttack;
                }
                
                //if waay out of range
                if (!boss.BossCombat.InRange)
                {
                    boss.BossCombat.JumpAttack();
                }
                
                boss.BossMovement.Chase(true);
                
                break;
            
            case BossState.BackOff:

                if (boss.BossCombat.InAttackRange)
                {
                    _state = BossState.RangedAttack;
                }

                if (boss.BossCombat.InMeleeRange)
                {
                    _state = BossState.MeleeAttack;
                }
                
                
                boss.BossMovement.BackOff(true);
                
                break;
            
            case BossState.MeleeAttack:
                
                //backoff if player gets too close
                if (!boss.BossCombat.InMeleeRange)
                {
                    boss.BossMovement.BackOff(true);
                    
                }

                if (boss.BossCombat.InBackOffRange)
                {
                    _state = BossState.RangedAttack;
                }

                if (boss.BossMovement.AtMinMeleeDistance)
                {
                    //play melee attack
                    boss.BossCombat.MeleeAttack();
                    
                    boss.BossMovement.BackOff(true);
                }
                
                boss.BossMovement.Chase(true);
                
                
                break;
            
            case BossState.RangedAttack:
                
                
                // if not in range, chase
                if (!boss.BossCombat.InRange)
                {
                    _state = BossState.Chase;
                }
                // if in backoff range
                if (boss.BossCombat.InBackOffRange)
                {
                    _state = BossState.BackOff;
                    
                }
                
                // attack animation and logic here
                // boss.BossAnimator.PlayJumpAttack();
                boss.BossCombat.RangedAttack();
                
                break;
            
            case BossState.Dead:
                
                boss.BossMovement.Chase(false);
                boss.BossMovement.BackOff(false);
                
                break;
            
            default:
                _state = BossState.Idle;
                break;
        }
    }
    
}
