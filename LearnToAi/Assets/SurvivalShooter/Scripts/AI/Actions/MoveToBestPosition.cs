namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;
    using UnityEngine;

    [FriendlyName("Move To Best Position", "Sets a move target based on the scorers and moves towards it")]
    public sealed class MoveToBestPosition : ActionWithOptions<Vector3>
    {
        public override void Execute(IAIContext context)
        {
            var c = (SurvivalContext)context;
            
            // get the highest scoring position based on the list of scorers attached to this action
            var best = this.GetBest(context, c.sampledPositions);
            if (best.sqrMagnitude == 0f)
            {
                // no valid position found
                return;
            }

            //move to the position
            c.player.MoveTo(best);
        }
    }
}