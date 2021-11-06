using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyPathFollower : MonoBehaviour
{  
    [SerializeField] private EnemyPathFinder enemyPathFinder;
    [SerializeField] private Transform playerTarget;
    [SerializeField] private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemyPathFinder.FollowTarget(true,playerTarget);
    }
}
