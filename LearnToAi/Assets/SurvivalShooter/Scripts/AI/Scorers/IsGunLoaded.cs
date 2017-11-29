namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class IsGunLoaded : ContextualScorerBase
    {
        [ApexSerialization(defaultValue = false)]
        public bool not = false;

        public override float Score(IAIContext context)
        {
            var c = (SurvivalContext)context;

            var player = c.player;

            if (player.currentAmmo <= 0)
            {
                return this.not ? this.score : 0f;
            }

            return this.not ? 0f : this.score;
        }
    }
}