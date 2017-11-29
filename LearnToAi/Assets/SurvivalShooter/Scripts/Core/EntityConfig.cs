namespace Apex.UnitySurvivalShooter
{
    using System;
    using UnityEngine;

    [Serializable]
    public class EntityConfig
    {
        public GameObject prefab;

        public EntityType type;

        public int initialPoolSize;
    }
}
