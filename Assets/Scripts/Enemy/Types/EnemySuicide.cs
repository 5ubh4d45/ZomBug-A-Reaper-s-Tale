using Game.Levels;
using UnityEngine;

public class EnemySuicide : MonoBehaviour
{
    public void DestroyEnemy()
    {
        LevelManager.Instance.UnregisterEnemy();
        
        Destroy(gameObject);
    }
}
