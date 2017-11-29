namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    /// <summary>
    /// Is th
    /// </summary>
    /// <seealso cref="Apex.UnitySurvivalShooter.CustomScorer{UnityEngine.Vector3}" />
    public sealed class OverRangeToAnyEnemy : CustomScorer<Vector3>
    {
        [ApexSerialization(defaultValue = 14f)]
        public float desiredRange = 14f;

        public override float Score(IAIContext context, Vector3 position)
        {
            var c = (SurvivalContext)context;
            var player = c.player;

            var enemies = c.enemies;
            var count = enemies.Count;
            if (count == 0)
            {
                return 0f;
            }

            var sqrDesiredRange = desiredRange * desiredRange;
            for (int i = 0; i < count; i++)
            {
                var enemy = enemies[i];

                var dirPlayerToEnemy = (enemy.position - player.position).OnlyXZ();
                var dirPositionToEnemy = (enemy.position - position).OnlyXZ();

                //all positions behind the enemy or closer than the desired range are not of interest
                if (Vector3.Dot(dirPlayerToEnemy, dirPositionToEnemy) < 0f || dirPositionToEnemy.sqrMagnitude < sqrDesiredRange)
                {
                    return 0f;
                }
            }

            return this.score;
        }
    }
}