namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;
    using Apex.Serialization;
    using UnityEngine;

    public sealed class ProximityToPlayerSpawner : CustomScorer<Vector3>
    {
        [ApexSerialization(defaultValue = 1f)]
        public float multiplier = 1f;

        public override float Score(IAIContext context, Vector3 position)
        {
            var c = (SurvivalContext)context;

            var range = (position - c.player.spawnPoint).magnitude;
            return Mathf.Max(0f, (this.score - range) * multiplier);
        }
    }
}