namespace Apex.UnitySurvivalShooter
{
    using UnityEngine;

    /// <summary>
    /// Represents an enemy
    /// </summary>
    public class Enemy : LivingEntity
    {
        public float spawnPowerUpChance = 0.2f;     // The chance of spawning a power up upon death
        public int scoreValue = 10;                 // The amount added to the player's score when the enemy dies.

        private EnemyAttack _enemyAttack;
        private EnemyMovement _enemyMovement;

        protected override void OnAwake()
        {
            _enemyAttack = GetComponent<EnemyAttack>();
            _enemyMovement = GetComponent<EnemyMovement>();
        }

        private void OnEnable()
        {
            _enemyAttack.enabled = true;
            _enemyMovement.enabled = true;
        }

        private void Update()
        {
            this.attackTarget = EntityManager.instance.GetClosestPlayer(transform.position);
            if (this.attackTarget != null)
            {
                _enemyMovement.navTarget = this.attackTarget.position;
            }
            else
            {
                _enemyMovement.navTarget = null;
            }
        }

        public void OnDeath()
        {
            // Turn off attack
            _enemyAttack.enabled = false;
            _enemyMovement.enabled = false;

            //enemies may spawn power ups on death
            SpawnPowerUp();

            // Increase the score by the enemy's score value.
            HUDState.UpdateScore(scoreValue);
        }

        protected override void OnAttackTargetChanged(LivingEntity newAttackTarget)
        {
            //If no more player targets exist, enter idle mode
            if (newAttackTarget == null)
            {
                _enemyAttack.enabled = false;
                _enemyMovement.enabled = false;

                GetComponent<Animator>().SetTrigger("PlayerDead");
            }
        }

        protected override void OnAttackTargetDead()
        {
            this.attackTarget = EntityManager.instance.GetClosestPlayer(transform.position);
        }

        private void SpawnPowerUp()
        {
            var random = UnityEngine.Random.Range(0f, 1f);

            if (random > spawnPowerUpChance)
            {
                return;
            }

            var index = UnityEngine.Random.Range(0, 2);

            var entityType = (index == 0) ? EntityType.Health : EntityType.Bomb;

            EntityManager.instance.Spawn(entityType, this.transform.position + Vector3.up, this.transform.rotation);
        }
    }
}