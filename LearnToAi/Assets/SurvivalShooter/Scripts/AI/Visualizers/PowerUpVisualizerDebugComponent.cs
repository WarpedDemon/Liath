namespace Apex.UnitySurvivalShooter
{
    using AI.Visualization;
    using UnityEngine;

    public class PowerUpVisualizerDebugComponent : ContextGizmoVisualizerComponent<SurvivalContext>
    {
        public Color color = Color.green;

        protected override void DrawGizmos(SurvivalContext context)
        {
            Gizmos.color = color;
            var playerPos = context.player.position;

            foreach (var p in context.powerups)
            {
                Gizmos.DrawLine(playerPos + Vector3.up, p.position);
                Gizmos.DrawWireSphere(p.position, 1f);
            }
        }
    }
}