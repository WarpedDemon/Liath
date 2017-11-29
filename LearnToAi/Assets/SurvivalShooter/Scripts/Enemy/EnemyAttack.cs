namespace Apex.UnitySurvivalShooter
{
    using System.Collections.Generic;
    using UnityEngine;

    public class EnemyAttack : MonoBehaviour
    {
        public float timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
        public int attackDamage = 10;               // The amount of health taken away per attack.

        private float _timer;                                // Timer for counting up to the next attack.
        private List<Collider> _playersInTrigger;

        private void Awake()
        {
            _playersInTrigger = new List<Collider>();
        }

        private void OnDisable()
        {
            _playersInTrigger.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            // If the entering collider is the player...
            if (other.tag == Tags.Player)
            {
                // ... the player is in range.
                _playersInTrigger.Add(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // If the exiting collider is the player...
            if (other.tag == Tags.Player)
            {
                // ... the player is no longer in range.
                _playersInTrigger.Remove(other);
            }
        }

        private void Update()
        {
            // Add the time since Update was last called to the timer.
            _timer += Time.deltaTime;

            // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
            if (_timer >= timeBetweenAttacks && _playersInTrigger.Count > 0)
            {
                // ... attack.
                Attack();
            }
        }

        private void Attack()
        {
            // Reset the timer.
            _timer = 0f;

            var count = _playersInTrigger.Count;

            //currently an enemy attacks all players in range;
            for (int i = 0; i < count; i++)
            {
                var col = _playersInTrigger[i];
                var player = EntityManager.instance.GetLivingEntityByGameObject(col.gameObject);
                if (player != null)
                {
                    player.TakeDamage(attackDamage);
                }
            }
        }
    }
}