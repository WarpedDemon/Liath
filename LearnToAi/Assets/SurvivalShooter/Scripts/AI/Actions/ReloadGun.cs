namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;

    [FriendlyName("Reload Gun", "Reloads the gun with a full magazine")]
    public sealed class ReloadGun : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (SurvivalContext)context;
            
            c.player.Reload();
        }
    }
}