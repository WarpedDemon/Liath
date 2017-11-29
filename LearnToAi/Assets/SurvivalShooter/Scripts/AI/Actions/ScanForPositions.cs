namespace Apex.UnitySurvivalShooter
{
    using System;
    using Apex.AI;
    using Apex.Serialization;
    using WorldGeometry;

    [FriendlyName("Scan for Positions", "Scanning positions and storing them in the context")]
    public sealed class ScanForPositions : ActionBase
    {
        [ApexSerialization(defaultValue = 6f), FriendlyName("Sampling Radius", "How large a radius points are sampled in, in a square with the entity in the center")]
        public float samplingRadius = 6f;

        public override void Execute(IAIContext context)
        {
            var ctx = (SurvivalContext)context;
            var unit = ctx.player.NavUnit;

            ctx.sampledPositions.Clear();

            var pos = unit.position;
            var grid = GridManager.instance.GetGrid(pos);

            Action<Cell> sampler = (cell) =>
            {
                //Note that simply checking if a cell is walkable is a simplification.
                //It works in this use case as there are no height induced obstacles and no clearance, but in more advanced use cases,
                //you likely want to check for reachability rather than walkability.
                //Also note that since attributes are not used, AttributeMask.None might as well have been passed to the IsWalkable call.
                if (cell.IsWalkable(unit.attributes))
                {
                    ctx.sampledPositions.Add(cell.position);
                }
            };

            grid.Apply(pos, this.samplingRadius, sampler);
        }
    }
}