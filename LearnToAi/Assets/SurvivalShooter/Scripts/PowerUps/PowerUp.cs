namespace Apex.UnitySurvivalShooter
{
    using LoadBalancing;
    using UnityEngine;

    /// <summary>
    /// Represents a power-up of some sort.
    /// </summary>
    /// <seealso cref="Apex.UnitySurvivalShooter.Entity" />
    public abstract class PowerUp : Entity
    {
        public float levitateSpeed = 4f;
        private bool _levitate;

        protected abstract void OnPickup(Player p);

        private void OnEnable()
        {
            _levitate = false;
        }

        private void Update()
        {
            if (_levitate)
            {
                this.transform.position += Vector3.up * levitateSpeed * Time.deltaTime;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //If whoever enters is player unit and this has not already been picked up, do the pick up.
            if (other.tag != Tags.Player || _levitate)
            {
                return;
            }

            var player = EntityManager.instance.GetLivingEntityByGameObject(other.gameObject) as Player;
            if (player == null)
            {
                return;
            }

            OnPickup(player);

            //Once picked up, float into the air and disappear after a second.
            _levitate = true;

            LoadBalancer.defaultBalancer.ExecuteOnce(Recycle, 1f);
        }

        private void Recycle()
        {
            EntityManager.instance.Recycle(this);
        }
    }
}
