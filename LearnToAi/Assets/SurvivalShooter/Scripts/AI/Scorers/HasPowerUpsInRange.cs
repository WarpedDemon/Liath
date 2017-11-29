namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class HasPowerUpsInRange : ContextualScorerBase
    {
        [ApexSerialization(defaultValue = 3f)]
        public float range = 3f;

        [ApexSerialization(defaultValue = false)]
        public bool not; 

        public override float Score(IAIContext context)
        {
            var c = (SurvivalContext)context;
            var powerups = c.powerups;

            var count = powerups.Count;

            for (int i = 0; i < count; i++)
            {
                var powerup = powerups[i];
                var sqrDist = (powerup.position - c.player.position).sqrMagnitude;

                if (sqrDist <= range * range)
                {
                    if (not)
                    {
                        return 0f;
                    }

                    return this.score;
                }
            }

            if (not)
            {
                return this.score;
            }

            return 0f;
        }
    }
}