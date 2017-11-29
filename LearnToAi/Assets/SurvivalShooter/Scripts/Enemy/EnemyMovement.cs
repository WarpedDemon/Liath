namespace Apex.UnitySurvivalShooter
{
    using Units;
    using UnityEngine;

    public class EnemyMovement : MonoBehaviour
    {
        private const float MinimalMoveDiff = 0.5f;

        private IUnitFacade _unit;
        private Vector3? _navTarget;


        public Vector3? navTarget
        {
            get
            {
                return _navTarget;
            }

            set
            {
                if (value == null)
                {
                    if (_navTarget.HasValue)
                    {
                        _navTarget = null;
                        _unit.Stop();
                    }

                    return;
                }

                //We don't want to calculate a new path every frame since the target will not move much during a single frame.
                //Instead we delay updating the move order until the target has moved sufficiently to justify a new path.
                bool updateMoveOrder = !_navTarget.HasValue || (value.Value - _navTarget.Value).sqrMagnitude > MinimalMoveDiff;

                if (updateMoveOrder)
                {
                    _navTarget = value;
                    _unit.MoveTo(_navTarget.Value, false);
                }
            }
        }

        private void Awake()
        {
            // Set up the references.
            _unit = this.GetUnitFacade();
        }

        private void OnDisable()
        {
            _unit.Stop();
        }
    }
}