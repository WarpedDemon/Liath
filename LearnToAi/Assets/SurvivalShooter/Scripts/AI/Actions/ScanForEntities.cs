namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;
    using UnityEngine;

    [FriendlyName("Scan for Enemies", "Scans the game world for enemies and stores them in the context")]
    public sealed class ScanForEntities : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            var c = (SurvivalContext)context;
            
            var player = c.player;
            c.enemies.Clear();

            // Use OverlapSphere for getting all relevant colliders within scan range, filtered by the scanning layer
            var colliders = Physics.OverlapSphere(player.position, player.scanRange, Layers.enemies);

            for (int i = 0; i < colliders.Length; i++)
            {
                var col = colliders[i];

                if (col.isTrigger)
                {
                    continue;
                }

                var enemy = EntityManager.instance.GetLivingEntityByGameObject(col.gameObject);

                if (enemy == null)
                {
                    continue;
                }

                c.enemies.Add(enemy);
            }
        }
    }
}