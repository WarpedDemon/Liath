namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;

    public sealed class UseBandAid : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (SurvivalContext)context;
            
            var player = c.player;

            player.UseBandAid();
        }
    }
}