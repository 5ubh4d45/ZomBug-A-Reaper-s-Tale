using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

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
    
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath;
    private float lastRepath = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (seeker == null){
            seeker = GetComponent<Seeker>();
        }
        if (rb == null){
            rb = GetComponent<Rigidbody2D>();
        }

        
    }

    public void OnPathComplete(Path p){
        
        // Debug.Log("New Path was calculated. Did it failed with an error?" + p.error);

        if (!p.error){
            path = p;
            //resets the currentWaypoint
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {   
        // FollowTarget(true, playerTarget);
    }

    public void FollowTarget(bool canFollow, Transform targetPosition){

        if (!canFollow){
            return;
        }
        if (Time.time > lastRepath + repeatRate && seeker.IsDone()){

            lastRepath = Time.time;

            seeker.StartPath(rb.position, targetPosition.position, OnPathComplete);
        }

        if ( path == null){

            return;
        }

        reachedEndOfPath = false;
        float distanceToWaypoint;

        while(true){
            
            //checks the distance
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);

            if (distanceToWaypoint < nextWaypointDistance){

                //checks if we reached end or theres another waypont ahead
                if (currentWaypoint + 1 < path.vectorPath.Count){
                    currentWaypoint++;

                } else{
                
                    reachedEndOfPath = true;
                    break;
                }
            } else{

                break;
            }
        }

        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint/nextWaypointDistance) : 1f;

        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;

        Vector3 velocity = dir * speed * speedFactor;

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
