namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    /// <summary>
    /// Is the position outside the minimum desired range to enemy spawn points.
    /// </summary>
    /// <seealso cref="Apex.UnitySurvivalShooter.CustomScorer{UnityEngine.Vector3}" />
    public sealed class OverRangeToAnyEnemySpawner : CustomScorer<Vector3>
    {
        [ApexSerialization(defaultValue = 14f)]
        public float desiredRange = 14f;

        public override float Score(IAIContext context, Vector3 position)
        {
            var enemySpawPoints = Blackboard.enemySpawnPoints;

            if (enemySpawPoints == null || enemySpawPoints.Length == 0)
            {
                return 0;
            }

            for (int i = 0; i < enemySpawPoints.Length; i++)
            {
                var sqrRange = (position - enemySpawPoints[i]).sqrMagnitude;

                if (sqrRange < desiredRange * desiredRange)
                {
                    return 0;
                }
            }

            return this.score;
        }
    }
}