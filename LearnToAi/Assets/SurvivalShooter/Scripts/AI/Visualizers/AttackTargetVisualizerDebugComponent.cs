namespace Apex.UnitySurvivalShooter
{
    using AI;
    using AI.Visualization;
    using UnityEngine;

    public class AttackTargetVisualizerDebugComponent : ContextGizmoVisualizerComponent
    {
        public Color color = Color.magenta;

        protected override void DrawGizmos(IAIContext context)
        {
            var ctx = (SurvivalContext)context;

            var attackTarget = ctx.player.attackTarget;
            if (attackTarget == null)
            {
                return;
            }

            Gizmos.color = color;
            var playerPos = ctx.player.position;

            Gizmos.DrawLine(playerPos + Vector3.up, attackTarget.position + Vector3.up);
            Gizmos.DrawWireSphere(attackTarget.position, 1f);
        }
    }
}