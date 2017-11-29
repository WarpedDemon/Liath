namespace Apex.UnitySurvivalShooter
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Manages all entities in the game and provides recycling (reuse) of entity instances.
    /// It also handles player selection in scenarios where multiple <see cref="Player"/>s are in the scene.
    /// </summary>
    public class EntityManager : MonoBehaviour
    {
        [SerializeField]
        private EntityConfig[] _entityConfigs = null;

        private EntityPool[] _pools;
        private Dictionary<GameObject, IEntity> _entityLookup;
        private List<IEntity> _players;

        public static EntityManager instance { get; private set; }

        public IList<IEntity> players
        {
            get { return _players; }
        }

        private void Awake()
        {
            //There can be only one...
            instance = this;

            //Initialize the data structures
            _pools = new EntityPool[_entityConfigs.Length];
            _entityLookup = new Dictionary<GameObject, IEntity>(_entityConfigs.Sum(cfg => cfg.initialPoolSize));
            _players = new List<IEntity>(1);

            //Prepare the pools for each entity type
            //An entity type's pool is at the index corresponding directly to its type.
            for (int i = 0; i < _entityConfigs.Length; i++)
            {
                var cfg = _entityConfigs[i];
                var pool = new EntityPool(cfg.prefab, this.gameObject, cfg.initialPoolSize);
                _pools[(int)cfg.type] = pool;
            }
        }

        private void Update()
        {
            //If multiple players exist in the scene, they can be focused using the numeric keys
            for (int i = 0; i < _players.Count; i++)
            {
                if (Input.GetKeyUp(KeyCode.Alpha1 + i))
                {
                    SetFocusedPlayer(_players[i]);
                }
            }
        }

        /// <summary>
        /// Spawns an instance of the specified entity type.
        /// </summary>
        /// <param name="entityType">The type of entity to spawn.</param>
        /// <param name="position">The position to spawn at.</param>
        /// <param name="rotation">The rotation once spawned.</param>
        /// <returns>The spawned entity.</returns>
        public IEntity Spawn(EntityType entityType, Vector3 position, Quaternion rotation)
        {
            //Get the entity from its pool
            var pool = _pools[(int)entityType];
            var entity = pool.Get(position, rotation);
            entity.type = entityType;

            //Register the entity so it can be retrieved through calls to GetEntityByGameObject or GetLivingEntityByGameObject
            _entityLookup.Add(entity.gameObject, entity);
            if (entityType == EntityType.Player)
            {
                _players.Add(entity);
                if (_players.Count == 1)
                {
                    SetFocusedPlayer(_players[0]);
                }
            }

            return entity;
        }

        /// <summary>
        /// Recycles the specified entity, returning it to its pool for reuse.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Recycle(IEntity entity)
        {
            var pool = _pools[(int)entity.type];
            pool.Return(entity);
            _entityLookup.Remove(entity.gameObject);

            //If the entity is a player, test for game over condition.
            if (entity.type == EntityType.Player)
            {
                _players.Remove(entity);
                if (_players.Count == 0)
                {
                    GameOverManager.GameOver();
                }
                else
                {
                    SetFocusedPlayer(_players[0]);
                }
            }
        }

        /// <summary>
        /// Gets the entity associated with a specific game object.
        /// </summary>
        /// <param name="go">The game object of the entity.</param>
        /// <returns>The entity associated with the game object</returns>
        public IEntity GetEntityByGameObject(GameObject go)
        {
            IEntity entity;
            if (_entityLookup.TryGetValue(go, out entity))
            {
                return entity;
            }

            return null;
        }

        /// <summary>
        /// Gets the entity associated with a specific game object.
        /// </summary>
        /// <param name="go">The game object of the entity.</param>
        /// <returns>The entity associated with the game object</returns>
        public LivingEntity GetLivingEntityByGameObject(GameObject go)
        {
            return GetEntityByGameObject(go) as LivingEntity;
        }

        /// <summary>
        /// Gets the player closest to the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The closest player, or null if no more players exist.</returns>
        public LivingEntity GetClosestPlayer(Vector3 position)
        {
            LivingEntity closest = null;
            float closestDistance = Mathf.Infinity;

            var count = _players.Count;
            for (int i = 0; i < count; i++)
            {
                var e = _players[i] as LivingEntity;
                if (e.currentHealth <= 0)
                {
                    continue;
                }

                var dist = (position - e.position).sqrMagnitude;

                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closest = e;
                }
            }

            return closest;
        }

        private void SetFocusedPlayer(IEntity player)
        {
            Player.focusedPlayer = (Player)player;
            Camera.main.GetComponent<CameraFollow>().SetTarget(player.gameObject.transform);
        }
    }
}
