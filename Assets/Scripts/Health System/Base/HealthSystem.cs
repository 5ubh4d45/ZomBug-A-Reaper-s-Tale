namespace Game.HealthSystem
{
    /// <summary>
    /// The base class used to inherit multiple Health Systems and for referencing health Systems
    /// </summary>
    public abstract class HealthSystem
    {
        #region Variables
        protected private bool IsDead;
        public Empty OnDead;
        public Event<float> OnDamaged;
        public Event<float> OnHealed;
        #endregion


        #region Getters And Setters
        public abstract float HealthPercent { get; }
        public abstract float Health01 { get; }
        #endregion


        #region Class Functions
        public virtual void Damage(float damageAmount)
        {

        }

        public virtual void Heal(float healAmount)
        {

        }
        #endregion


        #region Constructors
        #endregion
    }
}