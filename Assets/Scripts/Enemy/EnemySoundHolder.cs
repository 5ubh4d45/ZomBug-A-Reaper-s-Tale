using UnityEngine;

public class EnemySoundHolder : MonoBehaviour
{
    [Header("All the sound Triggers")]
    [Tooltip("Add String like: event:/(your trigger name) \n [NO QUOTATIONS REQUIRED]")]
    [SerializeField] private string meleeAttackSoundTrigger;
    [SerializeField] private string rangeAttackSoundTrigger;
    [Space]
    [SerializeField] private string deathSoundTrigger;
    [SerializeField] private string hitSoundTrigger;

    // like
    // [SerializeField] private string yourTriggerName;
    // public string YourTriggerName => yourTriggerName;
    
    public string MeleeAttackSound => meleeAttackSoundTrigger;
    public string RangedAttackSound => rangeAttackSoundTrigger;
    public string DeathSound => deathSoundTrigger;
    public string HitSound => hitSoundTrigger;
}
