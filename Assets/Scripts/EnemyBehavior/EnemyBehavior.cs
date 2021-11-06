using UnityEngine;

namespace EnemyBehavior
{
    public class EnemyBehavior : MonoBehaviour
    {
        [Header("Required Components")]
        [SerializeField] private EnemyPathFinder enemyPathFinder;
        [SerializeField] private Transform playerTarget;
        [SerializeField] private Rigidbody2D rb;

        private enum EnemyState
        {
            FreeRoam,
            ChasingTarget,
            AttackingTarget,
        
        }
        public bool canFollow = true;

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
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            enemyPathFinder.FollowTarget(canFollow, playerTarget);
        }
    
    
    
    }
}
