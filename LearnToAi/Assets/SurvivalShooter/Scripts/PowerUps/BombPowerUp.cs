namespace Apex.UnitySurvivalShooter
{
    /// <summary>
    /// A Bomb power-up
    /// </summary>
    /// <seealso cref="Apex.UnitySurvivalShooter.PowerUp" />
    public sealed class BombPowerUp : PowerUp
    {
        protected override void OnPickup(Player p)
        {
            p.AddBombs(1);
        }
   }
}
