namespace Apex.UnitySurvivalShooter
{
    using UnityEngine;

    /// <summary>
    /// Represents each of the layers used for filtering ray casts etc.
    /// </summary>
    public static class Layers
    {
        public static LayerMask cover;

        public static LayerMask enemies;

        public static LayerMask powerUps;
    }
}
