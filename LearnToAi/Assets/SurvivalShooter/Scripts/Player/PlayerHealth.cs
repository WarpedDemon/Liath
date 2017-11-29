namespace Apex.UnitySurvivalShooter
{
    using LoadBalancing;
    using UnityEngine;

    /// <summary>
    /// Represents the player's health
    /// </summary>
    public class PlayerHealth : MonoBehaviour, IHealth
    {
        public int startingHealth = 100;
        public int startingBandAids = 3;
        public AudioClip deathClip;                              
        public GameObject zPrefab;

        private ParticleSystem Zs;
        private Animator _anim;                                 
        private AudioSource _playerAudio;                        
        private Player _player;                                 
                                                                                                        
        private int _currentBandAids;
        private int _currentHealth;

        public int currentHealth
        {
            get
            {
                return _currentHealth;
            }

            private set
            {
                _currentHealth = Mathf.Max(0, value);
                HUDState.UpdateHealth(_currentHealth);
            }
        }

        public int currentBandAids
        {
            get
            {
                return _currentBandAids;
            }

            set
            {
                _currentBandAids = value;
                HUDState.UpdateBandAids(_currentBandAids);
            }
        }
        
        protected void Awake()
        {
            // Setting up the references.
            _anim = GetComponent<Animator>();
            _playerAudio = GetComponent<AudioSource>();
            _player = GetComponent<Player>();

            //Damage indicator
            var go = GameObject.Instantiate(zPrefab, this.transform.position + (Vector3.up * 1), zPrefab.transform.rotation) as GameObject;
            go.transform.SetParent(this.transform);
            Zs = go.GetComponent<ParticleSystem>();
            Zs.transform.position += Vector3.up;
        }

        private void OnEnable()
        {
            // Set the initial health of the player.
            this.currentHealth = startingHealth;
            this.currentBandAids = startingBandAids;
        }

        public void TakeDamage(int amount, Vector3 hitPoint)
        {
            TakeDamage(amount);
        }

        public void TakeDamage(int amount)
        {
            if (currentHealth <= 0)
            {
                return;
            }

            Zs.Play();

            // Reduce the current health by the damage amount.
            currentHealth -= amount;

            // Play the hurt sound effect.
            _playerAudio.Play();

            // If the player has lost all it's health and the death flag hasn't been set yet...
            if (currentHealth <= 0)
            {
                // Tell the animator that the player is dead.
                _anim.SetTrigger("Die");

                // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
                _playerAudio.clip = deathClip;
                _playerAudio.Play();

                _player.OnDeath();

                //Recycle the entity
                LoadBalancer.defaultBalancer.ExecuteOnce(() => EntityManager.instance.Recycle(_player), 3f);
            }
        }

        public void UseBandAid()
        {
            //Only heal if alive and damaged
            if (this.currentHealth == this.startingHealth || this.currentHealth <= 0)
            {
                return;
            }

            this.currentBandAids--;

            // Heal to full.
            this.currentHealth = startingHealth;
        }

        public void AddBandAid(int amount)
        {
            this.currentBandAids += amount;
        }
    }
}