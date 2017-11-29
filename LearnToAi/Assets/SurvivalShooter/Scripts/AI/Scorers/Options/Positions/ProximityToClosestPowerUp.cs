namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    public sealed class ProximityToClosestPowerUp : CustomScorer<Vector3>
    {
        [ApexSerialization(defaultValue = 1f)]
        public float multiplier = 1f;

        public override float Score(IAIContext context, Vector3 position)
        {
            var c = (SurvivalContext)context;
            var powerups = c.powerups;
            var count = powerups.Count;
            if (count == 0)
            {
                return 0f;
            }

            var closest = Vector3.zero;
            var shortest = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                var powerup = powerups[i];

                var distance = (position - powerup.position).sqrMagnitude;
                if (distance < shortest)
                {
                    shortest = distance;
                    closest = powerup.position;
                }
            }

            var range = (position - closest).magnitude;
            return Mathf.Max(0f, (this.score - range) * multiplier);
        }
    }
}