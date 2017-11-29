namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;
    using Apex.Serialization;

    public sealed class HasAttackTarget : ContextualScorerBase
    {
        [ApexSerialization(defaultValue = false)]
        public bool not = false;

        public override float Score(IAIContext context)
        {
            var c = (SurvivalContext)context;

            if (c.player.attackTarget == null)
            {
                return this.not ? this.score : 0f;
            }

            return this.not ? 0f : this.score;
        }
    }
}