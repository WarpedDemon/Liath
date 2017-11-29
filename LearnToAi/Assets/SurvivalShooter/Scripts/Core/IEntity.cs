namespace Apex.UnitySurvivalShooter
{
    using UnityEngine;

    /// <summary>
    /// Interface for entities
    /// </summary>
    public interface IEntity
    {
        int id { get; set; }

        EntityType type { get; set; }

        Vector3 position { get; set; }

        GameObject gameObject { get; }
    }
}