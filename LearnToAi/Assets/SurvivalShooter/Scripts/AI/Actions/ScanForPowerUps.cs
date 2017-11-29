namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;
    using UnityEngine;

    public sealed class ScanForPowerUps : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (SurvivalContext)context;

            var player = c.player;
            var powerups = c.powerups;
            powerups.Clear();

            // Use OverlapSphere for getting all relevant colliders within scan range, filtered by the scanning layer
            var colliders = Physics.OverlapSphere(player.position, player.scanRange, Layers.powerUps);

            for (int i = 0; i < colliders.Length; i++)
            {
                var col = colliders[i];

                var powerup = EntityManager.instance.GetEntityByGameObject(col.gameObject);

                if (powerup == null)
                {
                    continue;
                }

                powerups.Add(powerup);
            }
        }
    }
}