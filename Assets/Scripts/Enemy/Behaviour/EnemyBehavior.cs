using UnityEngine;


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
        private enum EnemyType
        {
            IsRanged,
            IsMelee,
        }

        private EnemyState _state;

        #region InspectorVariables
        
        [Header("Required Components")]
        [SerializeField] private EnemyPathFinder enemyPathFinder;
        [SerializeField] private Transform playerTarget;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private EnemyCombat enemyCombat;
        
        [Space] [Header("Behaviour Variables")]
        public bool canFollow = true;
        public bool canForgetTarget = true;
        [SerializeField] private EnemyType enemyType;
        [SerializeField] private bool showRangeCircle = false;
        
        [Space] [Header("Range Variables")]
        [SerializeField] private float minFreeRoamRange = 1f;
        [SerializeField] private float maxFreeRoamRange = 2f;
        [SerializeField] private float detectionRange = 20f;
        [SerializeField] private float idleWaitTime = 3f;
        [SerializeField] private float attackRange = 1f;
        
        #endregion

        #region PrivateVariables

        private Vector3 _startingPosition;
        private Vector3 _freeRoamPosition;
        private bool _onLineOfSight;
        private float _distanceFromTarget;
        private bool _reachedEndOfPath;
        private float _currentIdleTime = 0f;
        
        #endregion
        

        // Start is called before the first frame update
        private void Start()
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
            
            
            //setting up private variables
            _startingPosition = transform.position; // uncomment if full random free roam required
            // _startingPosition = transform.localPosition; //uncomment if limited free roam required
            
            _freeRoamPosition = GetRoamingPosition();

            //setting up idle state
            _state = EnemyState.Idle;
            
            #endregion
            
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            //detecting the target distance constantly
            var position = playerTarget.position;
            _distanceFromTarget = Vector3.Distance(position, rb.position);
            IsOnTarget(position);
            

            //enemy state machine
            switch (_state)
            {
                case EnemyState.Idle:
                    
                    //idle animations here
                    
                    if (_currentIdleTime >= idleWaitTime)
                    {  
                        // gets a new freeroam position & back to free roam
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
                    //running animations here
                    
                    enemyPathFinder.FollowTarget(true, _freeRoamPosition);

                    //checks if we reached our roam position
                    _reachedEndOfPath = enemyPathFinder.ReachedEndofPath;

                    if (_reachedEndOfPath)
                    {
                        //sets a state to idle
                        _state = EnemyState.Idle;
                    }
                    else if(_distanceFromTarget < detectionRange && _onLineOfSight)
                    {
                        _state = EnemyState.ChasingTarget;
                    }

                    break;

                case EnemyState.ChasingTarget:
                    
                    bool onRange = _distanceFromTarget <= detectionRange;
                    bool onAttackRange = _distanceFromTarget <= attackRange;
                    
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
                    // checks for attack status and if in range and was on line of sight switch to attack mode
                    if (onAttackRange && _onLineOfSight)
                    {
                        _state = EnemyState.AttackingTarget;
                    }
                    
                    //running animations here
                    enemyPathFinder.FollowTarget(true, playerTarget.position);

                    break;
                
                case  EnemyState.AttackingTarget:

                    switch (enemyType)
                    {
                        case EnemyType.IsMelee: //if its melee it'll follow constantly
                            
                            enemyPathFinder.FollowTarget(true, playerTarget.position);
                            //attack logic and animations here
                            
                            break;
                        case EnemyType.IsRanged: //if ranged it'll stop and shoot

                            bool withinAttackRange = _distanceFromTarget <= attackRange;
                            if (withinAttackRange)
                            {
                                enemyPathFinder.FollowTarget(false, playerTarget.position);
                                //attack logic and animations here
                                
                                enemyCombat.RangedAttack(transform.position, playerTarget.position);
                                
                            }
                            else
                            {
                                enemyPathFinder.FollowTarget(true, playerTarget.position);
                                //run animations here
                            }
                            break;
                        default:
                            enemyPathFinder.FollowTarget(true, playerTarget.position);
                            //run animations here
                            break;
                    }
                    
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
                * Random.Range(minFreeRoamRange, maxFreeRoamRange);

            //sets new starting position (Un comment if you wan full random roam)
            // _startingPosition = _freeRoamPosition;
            
            return roamingPos;
            
        }

        private void IsOnTarget(Vector3 targetPosition)
        {
            Vector2 direction = targetPosition - transform.position;
                    
            //checks if target is onLineSight 
            // RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, detectionRange);
            
            // instead of a single ray, it'll use a cylindrical ray for better detection
            RaycastHit2D hit = Physics2D.CircleCast(rb.position, 1f, direction, detectionRange);

            if (hit)
            {
                _onLineOfSight = hit.collider.CompareTag("Player");

            }

            // return _onLineOfSight;
        }

        private void OnDrawGizmosSelected()
        {
            if (!showRangeCircle) return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(rb.position, detectionRange);
            Gizmos.DrawWireSphere(rb.position, attackRange);
        }
    }
    
}
