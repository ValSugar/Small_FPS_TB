using PoolObjects;
using UnityEngine;

namespace Weapons.Missiles
{
	public abstract class MissileBase : MonoBehaviour
	{
		[SerializeField] protected int _damage;
		[SerializeField] protected float _speed;
		[SerializeField] protected GameObject _impactPrefab;

		protected Transform _transform; //Cached for micro-optimization
		protected float _damageMultiply;

		private void Awake()
		{
			_transform = transform;
		}

		public void SetDamageMultiply(float multiply)
		{
			_damageMultiply = multiply;
		}

		protected void Disable()
		{
			Pool.SpawnObject(_impactPrefab, _transform.position, Quaternion.identity);
			gameObject.SetActive(false);
		}

		protected virtual void OnDisable()
		{
			Pool.ReturnToPool(gameObject);
		}

		protected abstract void OnHit(Collider collider);
	}
}