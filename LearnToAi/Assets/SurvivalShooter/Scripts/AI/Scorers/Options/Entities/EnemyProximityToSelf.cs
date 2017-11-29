namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    public sealed class EnemyProximityToSelf : CustomScorer<LivingEntity>
    {
        [ApexSerialization(defaultValue = 1f)]
        public float multiplier = 1f;

        public override float Score(IAIContext context, LivingEntity entity)
        {
            var c = (SurvivalContext)context;

            var distance = (entity.position - c.player.position).magnitude;
            return Mathf.Max(0f, (this.score - distance) * this.multiplier);
        }
    }
}