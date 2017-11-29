namespace Apex.UnitySurvivalShooter
{
    using UnityEngine;

    /// <summary>
    /// Interface for health representations.
    /// </summary>
    public interface IHealth
    {
        int currentHealth { get; }

        void TakeDamage(int amount);

        void TakeDamage(int amount, Vector3 hitPoint);
    }
}
