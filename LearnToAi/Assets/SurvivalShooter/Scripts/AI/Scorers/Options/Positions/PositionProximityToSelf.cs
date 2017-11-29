namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    public sealed class PositionProximityToSelf : CustomScorer<Vector3>
    {
        [ApexSerialization(defaultValue = 0.01f)]
        public float factor = 0.01f;

        public override float Score(IAIContext context, Vector3 position)
        {
            var c = (SurvivalContext)context;
            var player = c.player;
            var range = (position - player.position).magnitude;
            return Mathf.Max(0f, (this.score - range) * factor);
        }
    }
}