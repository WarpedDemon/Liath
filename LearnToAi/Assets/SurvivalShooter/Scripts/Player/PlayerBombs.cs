namespace Apex.UnitySurvivalShooter
{
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerBombs : MonoBehaviour
    {
        public int damage = 500;                  // The damage inflicted by each bullet.
        public float timeBetweenBombs = 1f;       // The minimum time between each bomb use.
        public float range = 5f;                  // The bombs explosion range.
        public int startingBombs = 3;
        public Light explosionLight;
        public List<GameObject> explosions;

        private List<ParticleSystem> _explosionParticleSystems;

        private int _currentBombs;
        private float _timer;                                    // A timer to determine when to fire.
        private AudioSource _bombAudio;                           // Reference to the audio source.

        private float _effectsDisplayTime = 0.2f;                // The explosion display time.
        private float _effectStartTime;

        public int currentBombs
        {
            get
            {
                return _currentBombs;
            }

            set
            {
                _currentBombs = value;
                HUDState.UpdateBombs(_currentBombs);
            }
        }

        public bool canThrowBomb
        {
            get
            {
                if (this.currentBombs <= 0)
                {
                    return false;
                }

                if (_timer < timeBetweenBombs)
                {
                    return false;
                }

                return true;
            }
        }

        public void ThrowBomb()
        {
            _timer = 0;

            EnableEffects();

            _bombAudio.Play();

            //Find all enemies within the blast range of the bomb
            var colliders = Physics.OverlapSphere(transform.position, range, Layers.enemies);

            for (int i = 0; i < colliders.Length; i++)
            {
                var col = colliders[i];

                //Get a reference to the hit enemy and apply the damage
                var enemy = EntityManager.instance.GetLivingEntityByGameObject(col.gameObject);
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }

            this.currentBombs--;
        }

        public void AddBombs(int amount)
        {
            this.currentBombs += amount;
        }

        private void Awake()
        {
            // Set up the references.
            _bombAudio = GetComponent<AudioSource>();
        }

        private void Start()
        {
            var count = explosions.Count;

            _explosionParticleSystems = new List<ParticleSystem>();

            for (int i = 0; i < count; i++)
            {
                var prefab = explosions[i];

                var go = GameObject.Instantiate(prefab, this.transform.position + Vector3.up, prefab.transform.rotation) as GameObject;

                go.transform.SetParent(this.transform);

                _explosionParticleSystems.Add(go.GetComponent<ParticleSystem>());
            }
        }

        private void OnEnable()
        {
            this.currentBombs = startingBombs;
        }

        private void OnDisable()
        {
            DisableEffects();
        }

        private void Update()
        {
            // Add the time since Update was last called to the timer.
            _timer += Time.deltaTime;

            if (Time.time >= (_effectStartTime + _effectsDisplayTime))
            {
                DisableEffects();
            }

            Flicker();
        }

        private void Flicker()
        {
            if (explosionLight.enabled)
            {
                explosionLight.intensity = UnityEngine.Random.Range(0f, 8f);
            }
        }

        private void DisableEffects()
        {
            explosionLight.enabled = false;
        }

        private void EnableEffects()
        {
            _effectStartTime = Time.time;
            explosionLight.enabled = true;

            var count = _explosionParticleSystems.Count;

            for (int i = 0; i < count; i++)
            {
                var ps = _explosionParticleSystems[i];
                ps.Play();
            }
        }
    }
}