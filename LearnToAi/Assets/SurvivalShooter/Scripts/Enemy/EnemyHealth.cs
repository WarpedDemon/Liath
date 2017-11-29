namespace Apex.UnitySurvivalShooter
{
    using UnityEngine;

    /// <summary>
    /// Represents the health of an enemy
    /// </summary>
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        public int startingHealth = 100;
        public float sinkSpeed = 2.5f;              // The speed at which the enemy sinks through the floor when dead.
        public AudioClip deathClip;                 // The sound to play when the enemy dies.
        
        private Animator _anim;                              // Reference to the animator.
        private AudioSource _enemyAudio;                     // Reference to the audio source.
        private ParticleSystem _hitParticles;                // Reference to the particle system that plays when the enemy is damaged.
        private CapsuleCollider _capsuleCollider;            // Reference to the capsule collider.
        private Rigidbody _rigidBody;
        private bool _isSinking;                             // Whether the enemy has started sinking through the floor.
        private float _startSinkingTime;
        private Enemy _self;

        public int currentHealth
        {
            get;
            private set;
        }

        protected void Awake()
        {
            // Setting up the references.
            _anim = GetComponent<Animator>();
            _enemyAudio = GetComponent<AudioSource>();
            _hitParticles = GetComponentInChildren<ParticleSystem>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _rigidBody = GetComponent<Rigidbody>();
            _self = GetComponent<Enemy>();
        }

        protected void OnEnable()
        {
            _isSinking = false;
            _capsuleCollider.isTrigger = false;
            _rigidBody.isKinematic = false;

            currentHealth = startingHealth;
        }

        private void Update()
        {
            // If the enemy should be sinking...
            if (_isSinking)
            {
                // ... move the enemy down by the sinkSpeed per second.
                transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);

                if (Time.time > _startSinkingTime + 2f)
                {
                    //return the enemy to the game pool
                    EntityManager.instance.Recycle(_self);
                }
            }
        }

        public void TakeDamage(int amount)
        {
            TakeDamage(amount, transform.position + Vector3.up);
        }

        public void TakeDamage(int amount, Vector3 hitPoint)
        {
            if (currentHealth <= 0)
            {
                return;
            }

            // Play the hurt sound effect.
            _enemyAudio.Play();

            // Set the position of the particle system to where the hit was sustained.
            _hitParticles.transform.position = hitPoint;

            // And play the particles.
            _hitParticles.Play();

            // Reduce the current health by the amount of damage sustained.
            this.currentHealth -= amount;

            // If the current health is less than or equal to zero...
            if (currentHealth <= 0)
            {
                // ... the enemy is dead.
                Death();
            }
        }

        private void Death()
        {
            // Turn the collider into a trigger so shots can pass through it.
            _capsuleCollider.isTrigger = true;

            // Tell the animator that the enemy is dead.
            _anim.SetTrigger("Dead");

            // Change the audio clip of the audio source to the death clip and play it (this will stop the hurt clip playing).
            _enemyAudio.clip = deathClip;
            _enemyAudio.Play();

            _self.OnDeath();

            StartSinking();
        }

        public void StartSinking()
        {
            _startSinkingTime = Time.time;

            // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
            _rigidBody.isKinematic = true;

            // The enemy should no sink.
            _isSinking = true;
        }
    }
}