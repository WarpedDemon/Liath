namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;

    public sealed class CanThrowBomb : ContextualScorerBase
    {
        public override float Score(IAIContext context)
        {
            var c = (SurvivalContext)context;

            if (c.player.canThrowBomb)
            {
                return this.score;
            }

            return 0f;
        }
    }
}