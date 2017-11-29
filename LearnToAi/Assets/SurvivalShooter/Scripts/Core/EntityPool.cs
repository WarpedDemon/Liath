namespace Apex.UnitySurvivalShooter
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Pools or caches entities so that the can be reused.
    /// </summary>
    public sealed class EntityPool
    {
        private static int _nextId;
        private Queue<IEntity> _pool;
        private GameObject _prefab;
        private GameObject _host;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityPool"/> class.
        /// </summary>
        /// <param name="prefab">The prefab from which to create the entity.</param>
        /// <param name="host">The host that will be the parent of entity instances.</param>
        /// <param name="initialInstanceCount">The initial instance count.</param>
        public EntityPool(GameObject prefab, GameObject host, int initialInstanceCount)
        {
            _pool = new Queue<IEntity>(initialInstanceCount);
            _prefab = prefab;
            _host = host;

            //Instantiate and queue up the initial number of entities
            for (int i = 0; i < initialInstanceCount; i++)
            {
                _pool.Enqueue(CreateInstance());
            }
        }

        public int count
        {
            get { return _pool.Count; }
        }

        /// <summary>
        /// Gets an entity from the pool and places it at the specified position and rotation.
        /// If the pool is empty a new instance is created.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <returns>The entity</returns>
        public IEntity Get(Vector3 position, Quaternion rotation)
        {
            IEntity entity;
            if (_pool.Count > 0)
            {
                entity = _pool.Dequeue();
            }
            else
            {
                entity = CreateInstance();
            }

            var t = entity.gameObject.transform;

            t.position = position;
            t.rotation = rotation;

            entity.id = _nextId++;
            entity.gameObject.SetActive(true);
            return entity;
        }

        /// <summary>
        /// Returns the specified entity to the pool.
        /// </summary>
        /// <param name="item">The entity.</param>
        public void Return(IEntity item)
        {
            item.gameObject.SetActive(false);
            _pool.Enqueue(item);
        }

        private IEntity CreateInstance()
        {
            var go = GameObject.Instantiate(_prefab);
            go.transform.SetParent(_host.transform);
            go.SetActive(false);
            return go.GetComponent<IEntity>();
        }
    }
}
