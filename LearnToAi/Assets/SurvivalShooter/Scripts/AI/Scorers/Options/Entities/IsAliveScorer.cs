namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;

    public sealed class IsAliveScorer : CustomScorer<LivingEntity>
    {
        public override float Score(IAIContext context, LivingEntity entity)
        {
            if (entity.currentHealth > 0)
            {
                return this.score;
            }

            return 0f;
        }
    }
}