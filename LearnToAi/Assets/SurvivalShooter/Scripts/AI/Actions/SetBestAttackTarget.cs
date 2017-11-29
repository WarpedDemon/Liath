namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;

    public sealed class SetBestAttackTarget : ActionWithOptions<LivingEntity>
    {
        public override void Execute(IAIContext context)
        {
            var c = (SurvivalContext)context;
            var player = c.player;

            var enemies = c.enemies;

            var best = this.GetBest(context, enemies);
            if (best != null)
            {
                // Set the attack target
                player.attackTarget = best;
            }
        }
    }
}