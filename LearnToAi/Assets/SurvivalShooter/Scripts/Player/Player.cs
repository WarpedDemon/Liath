namespace Apex.UnitySurvivalShooter
{
    using System;
    using Apex.AI;
    using Apex.AI.Components;
    using Units;
    using UnityEngine;

    /// <summary>
    /// Component representing the player. Instantiates the context and holds player specific variables
    /// </summary>
    public class Player : LivingEntity, IContextProvider
    {
        public float scanRange;                     //The range inside which to scan for enemies and power-ups

        private SurvivalContext _context;
        private PlayerShooting _playerShooting;
        private PlayerHealth _playerHealth;
        private PlayerAIMovement _playerAIMovement;
        private PlayerBombs _playerBombs;
        private UtilityAIComponent _playerAI;
        private IUnitFacade _navUnit;

        public static Player focusedPlayer
        {
            get;
            set;
        }

        public IUnitFacade NavUnit
        {
            get { return _navUnit; }
        }

        public int currentAmmo
        {
            get { return _playerShooting.currentAmmo; }
        }

        public int currentBombs
        {
            get { return _playerBombs.currentBombs; }
        }

        public bool canThrowBomb
        {
            get { return _playerBombs.canThrowBomb; }
        }

        public int currentBandAids
        {
            get { return _playerHealth.currentBandAids; }
        }

        public Vector3 spawnPoint
        {
            get;
            private set;
        }

        public Vector3 gunTipOffset
        {
            get { return _playerShooting.transform.position - this.transform.position; }
        }

        public IAIContext GetContext(Guid aiId)
        {
            return _context;
        }

        protected override void OnAwake()
        {
            _context = new SurvivalContext(this);

            _navUnit = this.GetUnitFacade();
            _playerShooting = this.GetComponentInChildren<PlayerShooting>();
            _playerHealth = this.GetComponent<PlayerHealth>();
            _playerAIMovement = this.GetComponent<PlayerAIMovement>();
            _playerBombs = this.GetComponentInChildren<PlayerBombs>();
            _playerAI = this.GetComponent<UtilityAIComponent>();
        }

        private void OnEnable()
        {
            _playerShooting.enabled = true;
            _playerAIMovement.enabled = true;
            _playerAI.enabled = true;
            this.spawnPoint = this.transform.position;
        }

        public void StartFiring()
        {
            _playerShooting.shooting = true;
        }

        public void StopFiring()
        {
            _playerShooting.shooting = false;
        }

        public void MoveTo(Vector3 destination)
        {
            _playerAIMovement.Move(destination);
        }

        public void StopMoving()
        {
            _playerAIMovement.Stop();
        }

        public void Reload()
        {
            _playerShooting.Reload();
        }

        public void ThrowBomb()
        {
            _playerBombs.ThrowBomb();
        }

        public void AddBombs(int amount)
        {
            _playerBombs.AddBombs(amount);
        }

        public void UseBandAid()
        {
            _playerHealth.UseBandAid();
        }

        public void AddBandAid(int amount)
        {
            _playerHealth.AddBandAid(amount);
        }

        public void OnDeath()
        {
            // Turn off the movement and shooting scripts.
            _playerAIMovement.enabled = false;
            _playerShooting.enabled = false;
            _playerAI.enabled = false;
        }

        protected override void OnAttackTargetChanged(LivingEntity newAttackTarget)
        {
            //When a new attack target is set, we want to turn towards the target.
            var t = newAttackTarget != null ? newAttackTarget.gameObject.transform : null;
            _playerAIMovement.LookAt(t);
        }

        protected override void OnAttackTargetDead()
        {
            //When our target dies, stop shooting
            this.attackTarget = null;
            _playerShooting.shooting = false;
        }
    }
}