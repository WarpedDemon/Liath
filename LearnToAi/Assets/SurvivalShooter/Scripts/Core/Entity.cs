namespace Apex.UnitySurvivalShooter
{
    using UnityEngine;

    /// <summary>
    /// Base class for any entity in the game, e.g. player, enemy or power-up
    /// </summary>
    public abstract class Entity : MonoBehaviour, IEntity
    {
        /// <summary>
        /// Gets or sets the unique id. Since entities are recycled, comparing instance references will not always provide the expected result.
        /// In such cases entity identity can be established using this id.
        /// </summary>
        public int id { get; set; }

        public EntityType type { get; set; }

        public Vector3 position
        {
            get { return this.transform.position; }
            set { this.transform.position = value; }
        }
    }
}