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
        [SerializeField ]private float stopDistance = 1f;
        [SerializeField] private float nextWaypointDistance = 2f;
    
        private Path _path;
        private int _currentWaypoint = 0;
        private bool _reachedEndOfPath;
        private float _lastRepath = 0;


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

        public void FollowTarget(bool canFollow, Transform targetPosition){

            if (!canFollow){
                return;
            }
            if (Time.time > _lastRepath + repeatRate && seeker.IsDone()){

                _lastRepath = Time.time;

                seeker.StartPath(rb.position, targetPosition.position, OnPathComplete);
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

            Vector3 dir = (_path.vectorPath[_currentWaypoint] - transform.position).normalized;

            Vector3 velocity = dir * (speed * speedFactor);

            float distancefromTarget = Vector2.Distance(rb.position, targetPosition.position);

            if (distancefromTarget >= stopDistance){

                rb.position += (Vector2)velocity * Time.deltaTime;
            }


        }

        public void OnDisable(){
            seeker.pathCallback -= OnPathComplete;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(rb.position, stopDistance);
            Gizmos.DrawWireSphere(rb.position, nextWaypointDistance);
        }
    }
}
