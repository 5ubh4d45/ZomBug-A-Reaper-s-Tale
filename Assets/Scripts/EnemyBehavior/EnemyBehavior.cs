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
        public bool canForgetTarget = true;
        
        [Space] [Header("Range Variables")]
        [SerializeField] private float minFreeRoamRange = 1f;
        [SerializeField] private float maxFreeRoamRange = 2f;
        [SerializeField] private float detectionRange = 20f;
        [SerializeField] private float idleWaitTime = 3f;
        [SerializeField] private float stopDistance = 1f;
        
        private Vector3 _startingPosition;
        private Vector3 _freeRoamPosition;
        private bool _onLineOfSight;
        private float _distanceFromTarget;
        private bool _reachedEndOfPath;
        private float _currentIdleTime = 0f;
        
        

        // Start is called before the first frame update
        void Start()
        {
            #region Setting up variables
            
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
            _startingPosition = transform.localPosition; //new Vector3(0,0,0);
            _freeRoamPosition = GetRoamingPosition();

            //setting up idle state
            _state = EnemyState.Idle;
            
            #endregion


        }

        // Update is called once per frame
        void FixedUpdate()
        {
            // enemyPathFinder.FollowTarget(true, playerTarget.position);
            
            //detecting the target distance constantly
            _distanceFromTarget = Vector3.Distance(playerTarget.position, rb.position);
            IsOnTarget(playerTarget.position);
            

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
                    else if(_distanceFromTarget < detectionRange && _onLineOfSight)
                    {
                        _state = EnemyState.ChasingTarget;
                    }

                    break;

                case EnemyState.ChasingTarget:
                    
                    bool onRange = _distanceFromTarget < detectionRange;
                    
                    //if player out of range & can forget the back to free roam
                    if (!onRange && canForgetTarget)
                    {
                        _state = EnemyState.Idle;
                    }
                    
                    //checks if also on line of sight
                    if(onRange && !_onLineOfSight)
                    {
                        _state = EnemyState.Idle;
                    }
                    enemyPathFinder.FollowTarget(true, playerTarget.position);

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

        private bool IsOnTarget(Vector3 targetPosition)
        {
            Vector2 direction = (Vector2) (targetPosition - transform.position);
                    
            //checks if target is onLineSight 
            RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, detectionRange);
            if (hit)
            {
                _onLineOfSight = hit.collider.CompareTag("Player");
                
            }

            return _onLineOfSight;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(rb.position, detectionRange);
        }
    }
    
}
