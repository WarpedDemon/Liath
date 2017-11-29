namespace Apex.UnitySurvivalShooter
{
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// The blackboard represents information available to everyone as opposed to the Context with is unit specific.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class Blackboard : MonoBehaviour
    {
        public static Vector3[] enemySpawnPoints { get; private set; }

        private void Awake()
        {
            var spawners = GameObject.FindObjectsOfType<EntitySpawner>();

            enemySpawnPoints = (from s in spawners
                                where s.entityType != EntityType.Player
                                select s.transform.position).ToArray();
        }
    }
}
