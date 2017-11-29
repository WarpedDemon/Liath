namespace Apex.UnitySurvivalShooter
{
    using UnityEngine;

    public class PlayerShooting : MonoBehaviour
    {
        public int damagePerShot = 20;                  // The damage inflicted by each bullet.
        public float timeBetweenBullets = 0.15f;        // The time between each shot.
        public float range = 100f;                      // The distance the gun can fire.
        public int startingAmmo = 16;                   // The starting ammunition, e..g what represents a full clip
        public Light faceLight;                             

        private float _timer;                                    // A timer to determine when to fire.
        private Ray _shootRay;                                   // A ray from the gun end forwards.
        private ParticleSystem _gunParticles;                    // Reference to the particle system.
        private LineRenderer _gunLine;                           // Reference to the line renderer.
        private AudioSource _gunAudio;                           // Reference to the audio source.
        private Light _gunLight;                                 // Reference to the light component.
        private float _effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.
        private int _currentAmmo;

        public int currentAmmo
        {
            get
            {
                return _currentAmmo;
            }

            set
            {
                _currentAmmo = value;
                HUDState.UpdateAmmo(_currentAmmo);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PlayerShooting"/> is/should be shooting.
        /// </summary>
        /// <value>
        ///   <c>true</c> if shooting; otherwise, <c>false</c>.
        /// </value>
        public bool shooting
        {
            get;
            set;
        }

        private void Awake()
        {
            // Set up the references.
            _gunParticles = GetComponent<ParticleSystem>();
            _gunLine = GetComponent<LineRenderer>();
            _gunAudio = GetComponent<AudioSource>();
            _gunLight = GetComponent<Light>();
            faceLight = GetComponentInChildren<Light>();
        }

        private void OnEnable()
        {
            this.currentAmmo = startingAmmo;
        }

        private void OnDisable()
        {
            this.shooting = false;
            DisableEffects();
        }

        private void Update()
        {
            // Add the time since Update was last called to the timer.
            _timer += Time.deltaTime;

            if (this.shooting && _timer > timeBetweenBullets && _currentAmmo > 0)
            {
                Shoot();
            }

            // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
            if (_timer >= timeBetweenBullets * _effectsDisplayTime)
            {
                // ... disable the effects.
                DisableEffects();
            }
        }

        public void Reload()
        {
            this.currentAmmo = startingAmmo;
        }

        public void DisableEffects()
        {
            // Disable the line renderer and the light.
            _gunLine.enabled = false;
            faceLight.enabled = false;
            _gunLight.enabled = false;
        }

        private void Shoot()
        {
            // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
            _shootRay.origin = transform.position;
            _shootRay.direction = transform.forward;

            // Due to the immense accuracy from wearing a nightcap, he only shoots if he knows he will hit.
            RaycastHit shotHit;
            if (!Physics.Raycast(_shootRay, out shotHit, range, Layers.cover | Layers.enemies))
            {
                return;
            }

            //While we do have a specific attack target we still just hit anything that gets in the way of the bullet, bullet aren't picky
            //We don't shoot at cover though
            var go = shotHit.collider.gameObject;

            var enemy = EntityManager.instance.GetLivingEntityByGameObject(go);
            if (enemy == null)
            {
                return;
            }

            // Reset the timer.
            _timer = 0f;

            // Play the gun shot audioclip.
            _gunAudio.Play();

            // Enable the lights.
            _gunLight.enabled = true;
            faceLight.enabled = true;

            // Stop the particles from playing if they were, then start the particles.
            _gunParticles.Stop();
            _gunParticles.Play();

            // Enable the line renderer and set it's first position to be the end of the gun.
            _gunLine.enabled = true;
            _gunLine.SetPosition(0, transform.position);
            _gunLine.SetPosition(1, shotHit.point);

            //Bring the hurt
            enemy.TakeDamage(damagePerShot, shotHit.point);

            this.currentAmmo--;
        }
    }
}