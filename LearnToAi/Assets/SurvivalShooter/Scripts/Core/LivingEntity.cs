namespace Apex.UnitySurvivalShooter
{
    using UnityEngine;

    /// <summary>
    /// Represents a living entity that can attack and take damage.
    /// </summary>
    /// <seealso cref="Apex.UnitySurvivalShooter.Entity" />
    public abstract class LivingEntity : Entity
    {
        //Pointers to other attackers of the current attackTarget
        private LivingEntity _previous;
        private LivingEntity _next;

        private LivingEntity _firstAttacker;
        private LivingEntity _attackTarget;

        private IHealth _health;

        /// <summary>
        /// Gets the current health.
        /// </summary>
        public int currentHealth
        {
            get { return _health.currentHealth; }
        }

        /// <summary>
        /// Gets or sets a reference to the entity currently being attacked by this entity.
        /// </summary>
        public LivingEntity attackTarget
        {
            get
            {
                return _attackTarget;
            }

            set
            {
                //Because we are recycling entities, comparing references can lead to unexpected results as an entity could potentially
                //respawn before this is reset by the AI. We also don't want to set attack targets for dead units.
                if ((value != null && ((_attackTarget != null && _attackTarget.id == value.id) || this.currentHealth <= 0)) || 
                    (_attackTarget == null && value == null))
                {
                    return;
                }

                //Instead of using list to maintain who is attacking who, we instead show an alternative using linked lists
                //This may be a little hard to follow, but each target holds a reference to its first attacker which in turn holds a reference
                //to the second attacker and so forth in a two way fashion (each attacker apart from the first and the last holds a reference to the one before and after it).
                if (_attackTarget != null)
                {
                    if (_previous != null)
                    {
                        _previous._next = _next;
                    }
                    else
                    {
                        _attackTarget._firstAttacker = _next;
                    }

                    if (_next != null)
                    {
                        _next._previous = _previous;
                    }

                    _previous = _next = null;
                }

                //Set the new target
                _attackTarget = value;
                if (_attackTarget != null)
                {
                    _next = _attackTarget._firstAttacker;
                    if (_next != null)
                    {
                        _next._previous = this;
                    }

                    _attackTarget._firstAttacker = this;
                }

                OnAttackTargetChanged(_attackTarget);
            }
        }

        private void Awake()
        {
            _health = this.GetComponent<IHealth>();
            OnAwake();
        }

        /// <summary>
        /// Take damage without a specific hit point - i.e. generic damage
        /// </summary>
        /// <param name="amount"></param>
        public void TakeDamage(int amount)
        {
            _health.TakeDamage(amount);
            if (_health.currentHealth <= 0)
            {
                OnDeath();
            }
        }

        /// <summary>
        /// Take damage with a specific hit point -&gt; for e.g. spawning particle system
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="hitPoint"></param>
        public void TakeDamage(int amount, Vector3 hitPoint)
        {
            _health.TakeDamage(amount, hitPoint);
            if (_health.currentHealth <= 0)
            {
                OnDeath();
            }
        }

        /// <summary>
        /// Called on awake.
        /// </summary>
        protected virtual void OnAwake()
        {
        }

        /// <summary>
        /// Called when the attack target changes.
        /// </summary>
        /// <param name="newAttackTarget">The new attack target.</param>
        protected abstract void OnAttackTargetChanged(LivingEntity newAttackTarget);

        /// <summary>
        /// Called when the attack target dies.
        /// </summary>
        protected abstract void OnAttackTargetDead();

        private void OnDeath()
        {
            var attacker = _firstAttacker;
            while (attacker != null)
            {
                var tmp = attacker._next;
                attacker.OnAttackTargetDead();
                attacker = tmp;
            }

            this.attackTarget = null;
        }
    }
}
