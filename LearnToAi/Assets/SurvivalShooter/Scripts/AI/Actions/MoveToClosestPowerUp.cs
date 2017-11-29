namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;

    public sealed class MoveToClosestPowerUp : ActionWithOptions<IEntity>
    {
        public override void Execute(IAIContext context)
        {
            var c = (SurvivalContext)context;
            
            var best = this.GetBest(context, c.powerups);
            if (best == null)
            {
                // no valid position found
                return;
            }

            c.player.MoveTo(best.position);
        }
    }
}