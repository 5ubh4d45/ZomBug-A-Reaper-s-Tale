using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


namespace EnemyBehavior
{
    public class EnemyBehavior : MonoBehaviour
    {
        private enum EnemyState
        {   
            Idle,
            FreeRoam,
            ChasingTarget,
            AttackingTarget,
        
        }

        private EnemyState _state;
        
        [Header("Required Components")]
        [SerializeField] private EnemyPathFinder enemyPathFinder;
        [SerializeField] private Transform playerTarget;
        [SerializeField] private Rigidbody2D rb;
        
        [Space] [Header("Behaviour Variables")]
        public bool canFollow = true;

        [SerializeField] private float minFreeRoamRange = 10f;
        [SerializeField] private float maxFreeRoamRange = 10f;
        [SerializeField] private float idleWaitTime = 3f;
        [SerializeField] private float stopDistance = 1f;
        
        private Vector3 _startingPosition;
        private Vector3 _freeRoamPosition;
        private bool _reachedEndOfPath;
        private float _currentIdleTime = 0f;
        
        

        // Start is called before the first frame update
        void Start()
        {   
            if (enemyPathFinder == null){
                enemyPathFinder = GetComponent<EnemyPathFinder>();
            }
            if (rb == null){
                rb = GetComponent<Rigidbody2D>();
            }
            if (playerTarget == null){
                playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
            }
            
            // _startingPosition = transform.position; // uncomment if full random free roam required
            
            //setting up private variables
            _startingPosition = new Vector3(0,0,0);
            _freeRoamPosition = GetRoamingPosition();
           
            
            
            
            //setting up idle state
            _state = EnemyState.Idle;
            

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            // enemyPathFinder.FollowTarget(true, playerTarget.position);

            //enemy state machine
            switch (_state)
            {
                case EnemyState.Idle:

                    if (_currentIdleTime >= idleWaitTime)
                    {  
                        // gets a new freeroam positiom & back to free roam
                        _freeRoamPosition = GetRoamingPosition();
                        _state = EnemyState.FreeRoam;
                        
                        //resets the current idle Time
                        _currentIdleTime = 0f;
                    }
                    else
                    {
                        _currentIdleTime += Time.deltaTime;
                    }
                    
                    
                    break;
                
                case EnemyState.FreeRoam:
                    
                    //gets roaming position then moves towards it
                    enemyPathFinder.FollowTarget(true, _freeRoamPosition);
                    
                    //checks if we reached our roam position
                    _reachedEndOfPath = enemyPathFinder.ReachedEndofPath;

                    if (_reachedEndOfPath)
                    {
                        //sets a new free roam position
                        _freeRoamPosition = GetRoamingPosition();
                        
                        //sets a state to idle
                        _state = EnemyState.Idle;
                    }
                    break;
                
                case  EnemyState.ChasingTarget:
                    break;
                
                case  EnemyState.AttackingTarget:
                    break;
                default:
                    _state = EnemyState.FreeRoam;
                    break;
            }
        }

        private Vector3 GetRoamingPosition()
        {
            //returns a random roaming position
            Vector3 roamingPos = _startingPosition + enemyPathFinder.GetRandomDir()
                * UnityEngine.Random.Range(minFreeRoamRange, maxFreeRoamRange);

            //sets new starting position
            // _startingPosition = _freeRoamPosition;
            
            return roamingPos;
            
        }

        
    }
    
}
