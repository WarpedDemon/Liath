namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;

    public sealed class IsCurrentTargetScorer : CustomScorer<LivingEntity>
    {
        public override float Score(IAIContext context, LivingEntity entity)
        {
            var c = (SurvivalContext)context;
            if (c.player.attackTarget == entity)
            {
                return this.score;
            }

            return 0f;
        }
    }
}