namespace Apex.UnitySurvivalShooter
{
    using UnityEngine;

    /// <summary>
    /// Maps Unity layers to the internal <see cref="Layers"/>
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public sealed class LayerMapping : MonoBehaviour
    {
        public LayerMask coverLayer;
        public LayerMask powerUpsLayer;
        public LayerMask enemyLayer;

        private void Awake()
        {
            Layers.cover = coverLayer;
            Layers.enemies = enemyLayer;
            Layers.powerUps = powerUpsLayer;
        }
    }
}