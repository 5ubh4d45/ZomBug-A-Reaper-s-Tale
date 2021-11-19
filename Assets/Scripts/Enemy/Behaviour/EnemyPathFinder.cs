using Pathfinding;
using UnityEngine;

namespace EnemyBehavior
{
    public class EnemyPathFinder : MonoBehaviour
    {   
        [Header("Required Components")]
        [SerializeField] private Seeker seeker;
        [SerializeField] private Rigidbody2D rb;

        [Space]
        [Header("PathFinding Attributes")]
        [SerializeField] private float repeatRate = 0.5f;
        [SerializeField] private float speed = 10f;
        [SerializeField] private float nextWaypointDistance = 2f;

        private Vector3 _lookDir;
        private Path _path;
        private int _currentWaypoint = 0;
        private bool _reachedEndOfPath;
        private float _lastRepath = 0;
        
        //setting up required getters
        public bool ReachedEndofPath => _reachedEndOfPath;

        public Vector3 LookDir => _lookDir;

        // Start is called before the first frame update
        private void Start()
        {
            if (seeker == null){
                seeker = GetComponent<Seeker>();
            }
            if (rb == null){
                rb = GetComponent<Rigidbody2D>();
            }

        
        }

        private void OnPathComplete(Path p){
        
            // Debug.Log("New Path was calculated. Did it failed with an error?" + p.error);

            if (!p.error){
                _path = p;
                //resets the currentWaypoint
                _currentWaypoint = 0;
            }
        }

        public void OnDisable(){
            seeker.pathCallback -= OnPathComplete;
        }
        
        public void FollowTarget(bool canFollow, Vector3 targetPosition){

            if (!canFollow){
                return;
            }
            if (Time.time > _lastRepath + repeatRate && seeker.IsDone()){

                _lastRepath = Time.time;

                seeker.StartPath(rb.position, targetPosition, OnPathComplete);
            }

            if ( _path == null){
                return;
            }

            _reachedEndOfPath = false;
            float distanceToWaypoint;

            while(true){
            
                //checks the distance
                distanceToWaypoint = Vector3.Distance(transform.position, _path.vectorPath[_currentWaypoint]);

                if (distanceToWaypoint < nextWaypointDistance){

                    //checks if we reached end or theres another waypoint ahead
                    if (_currentWaypoint + 1 < _path.vectorPath.Count){
                        _currentWaypoint++;

                    } else{
                        
                        _reachedEndOfPath = true;
                        break;
                    }
                } else{

                    break;
                }
            }

            float speedFactor = _reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint/nextWaypointDistance) : 1f;
            
            //gets movement direction
            _lookDir = (_path.vectorPath[_currentWaypoint] - transform.position).normalized;
            
            //gets velocity
            Vector3 velocity = _lookDir * (speed * speedFactor);
            
            //moves the body
            rb.position += (Vector2)velocity * Time.deltaTime;
            
        }
        
        //gets a random direcetion
        public Vector3 GetRandomDir() => new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        
        

        private void OnDrawGizmosSelected() {
            
            Gizmos.color = Color.red;
            
            Gizmos.DrawWireSphere(rb.position, nextWaypointDistance);
        }
        
    }
}
