namespace Apex.UnitySurvivalShooter
{
    using System;
    using System.Collections.Generic;
    using AI;
    using AI.Components;
    using AI.Visualization;
    using UnityEngine;

    public class EnemyVisualizerDebugComponent : ContextGizmoVisualizerComponent
    {
        public Color color = Color.red;

        protected override void DrawGizmos(IAIContext context)
        {
            var ctx = (SurvivalContext)context;

            Gizmos.color = color;
            var playerPos = ctx.player.position;

            var enemies = ctx.enemies;
            var count = enemies.Count;
            for (int i = 0; i < count; i++)
            {
                var obs = enemies[i];

                Gizmos.DrawLine(playerPos, obs.position);
                Gizmos.DrawSphere(obs.position, 1f);
            }
        }

        protected override void GetContextsToVisualize(List<IAIContext> contextsBuffer, Guid relevantAIId)
        {
            var players = EntityManager.instance.players;
            if (players != null)
            {
                var count = players.Count;
                for (int i = 0; i < count; i++)
                {
                    contextsBuffer.Add(((IContextProvider)players[i]).GetContext(relevantAIId));
                }
            }
        }
    }
}