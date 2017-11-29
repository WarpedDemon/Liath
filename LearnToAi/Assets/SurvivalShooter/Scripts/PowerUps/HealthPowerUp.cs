namespace Apex.UnitySurvivalShooter
{
    /// <summary>
    /// A health power-up, i.e. a band aid.
    /// </summary>
    /// <seealso cref="Apex.UnitySurvivalShooter.PowerUp" />
    public sealed class HealthPowerUp : PowerUp
    {
        protected override void OnPickup(Player p)
        {
            p.AddBandAid(1);
        }
    }
}
