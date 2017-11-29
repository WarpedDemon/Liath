namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    public sealed class ProximityToNearestEnemy : CustomScorer<Vector3>
    {
        [ApexSerialization(defaultValue = 14f)]
        public float desiredRange = 14f;

        public override float Score(IAIContext context, Vector3 position)
        {
            var c = (SurvivalContext)context;
            var enemies = c.enemies;
            var count = enemies.Count;
            if (count == 0)
            {
                return 0f;
            }

            var nearest = Vector3.zero;
            var shortest = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                var enemy = enemies[i];

                var distance = (position - enemy.position).sqrMagnitude;
                if (distance < shortest)
                {
                    shortest = distance;
                    nearest = enemy.position;
                }
            }

            if (nearest.sqrMagnitude == 0f)
            {
                return 0f;
            }

            var range = (position - nearest).magnitude;
            return Mathf.Max(0f, (this.score - Mathf.Abs(this.desiredRange - range)));
        }
    }
}