namespace Apex.UnitySurvivalShooter
{
    using Units;
    using UnityEngine;

    public class PlayerAIMovement : MonoBehaviour
    {
        private Animator _anim;
        private Transform _lookAtTransform;
        private IUnitFacade _unit;

        private void Awake()
        {
            // Set up references.
            _anim = GetComponent<Animator>();
            _unit = this.GetUnitFacade();
        }

        private void OnDisable()
        {
            _unit.Stop();
        }

        private void FixedUpdate()
        {
            if (_lookAtTransform != null)
            {
                _unit.lookAt = _lookAtTransform.position;
            }

            // Animate the player.
            Animating();
        }

        public void Move(Vector3 destination)
        {
            _unit.MoveTo(destination, false);
        }

        public void Stop()
        {
            _unit.Stop();
        }

        /// <summary>
        /// Set the position to look at. Set to null is the Ai should stop looking
        /// </summary>
        /// <param name="lookAtTransform"></param>
        public void LookAt(Transform lookAtTransform)
        {
            _lookAtTransform = lookAtTransform;
            if (_lookAtTransform == null)
            {
                _unit.lookAt = null;
            }
        }

        private void Animating()
        {
            // Create a boolean that is true if either of the input axes is non-zero.
            bool walking = false;

            if (_unit.velocity.sqrMagnitude > 0f)
            {
                walking = true;
            }

            // Tell the animator whether or not the player is walking.
            _anim.SetBool("IsWalking", walking);
        }
    }
}