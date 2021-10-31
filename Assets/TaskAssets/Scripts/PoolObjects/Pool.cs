using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PoolObjects
{
	public class Pool : MonoBehaviour
	{
		public static Pool Instance;

		[SerializeField] private PrespawnContainer[] _prespawnContainers;

		private List<PoolContainer> _poolContainers;

		private void Awake()
		{
			Instance = this;

			_poolContainers = new List<PoolContainer>();

			foreach (var container in _prespawnContainers)
			{
				var pool = new PoolContainer(container.prefab);
				_poolContainers.Add(pool);
				for (var i = 0; i < container.count; i++)
				{
					var obj = Instantiate(container.prefab, Vector3.zero, Quaternion.identity, transform);
					pool.activeObjects.Add(obj);
					obj.gameObject.SetActive(false);
				}
			}
		}

		public static T SpawnObject<T>(T poolObject, Vector3 position, Quaternion rotation) where T : MonoBehaviour
		{
			var result = SpawnObject(poolObject.gameObject, position, rotation);
			return result.GetComponent<T>();
		}

		public static GameObject SpawnObject(GameObject poolObject, Vector3 position, Quaternion rotation)
		{
			var pool = Instance.GetOrCreatePool(poolObject);

			GameObject result;
			if (pool.deactiveObjects.Count > 0)
			{
				result = pool.deactiveObjects.Dequeue();
				result.transform.position = position;
				result.transform.rotation = rotation;
				result.gameObject.SetActive(true);
			}
			else
			{
				result = Instantiate(poolObject, position, rotation, Instance.transform);
			}

			pool.activeObjects.Add(result);

			return result;
		}

		public static void ReturnToPool(GameObject obj)
		{
			foreach (var container in Instance._poolContainers)
			{
				if (!container.activeObjects.Contains(obj))
					continue;

				container.activeObjects.Remove(obj);
				container.deactiveObjects.Enqueue(obj);
			}
		}

		private PoolContainer GetOrCreatePool(GameObject prefab)
		{
			var pool = _poolContainers.FirstOrDefault(x => x.prefab == prefab);
			if (pool == null)
			{
				pool = new PoolContainer(prefab);
				_poolContainers.Add(pool);
			}

			return pool;
		}

		[Serializable]
		private class PoolContainer
		{
			public GameObject prefab;
			public HashSet<GameObject> activeObjects;
			public Queue<GameObject> deactiveObjects;

			public PoolContainer(GameObject prefab)
			{
				this.prefab = prefab;
				activeObjects = new HashSet<GameObject>();
				deactiveObjects = new Queue<GameObject>();
			}
		}

		[Serializable]
		private class PrespawnContainer
		{
			public int count;
			public GameObject prefab;
		}
	}
}
