using UnityEngine;

public class EnemySoundHolder : MonoBehaviour
{
    [Tooltip("Add String like: event:/(your trigger name) /n [NO QUOTATIONS REQUIRED]")]
    [Header("All the sound Triggers")]
    [SerializeField] private string meleeAttackSoundTrigger;
    [SerializeField] private string rangeAttackSoundTrigger;
    [Space]
    [SerializeField] private string deathSoundTrigger;
    [SerializeField] private string hitSoundTrigger;

    public string MeleeAttackSound => meleeAttackSoundTrigger;
    public string RangedAttackSound => rangeAttackSoundTrigger;
    public string DeathSound => deathSoundTrigger;
    public string HitSound => hitSoundTrigger;
}
