namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class HealthBelowThreshold : ContextualScorerBase
    {
        [ApexSerialization(defaultValue = false)]
        public bool not = false;

        [ApexSerialization(defaultValue = 30f)]
        public float threshold = 30f;

        public override float Score(IAIContext context)
        {
            var c = (SurvivalContext)context;

            if (c.player.currentHealth < threshold)
            {
                if (not)
                {
                    return 0f;
                }

                return this.score;
            }

            if (not)
            {
                return this.score;
            }

            return 0f;
        }
    }
}