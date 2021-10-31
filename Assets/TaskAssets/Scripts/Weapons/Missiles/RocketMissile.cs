using Entities;
using UnityEngine;

namespace Weapons.Missiles
{
	public class RocketMissile : DirectMissile
	{
		[SerializeField] private float _explosionRadius;
		[Range(0f, 1f)]
		[SerializeField] private float _minGeneralMultiply;
		[SerializeField] private float _impulseForceMultiply;

		protected override void OnHit(Collider collider)
		{
			var targets = Physics.OverlapSphere(_transform.position, _explosionRadius);
			foreach (var target in targets)
			{
				var closestPoint = target.ClosestPoint(target.transform.position);
				var distance = Vector3.Distance(_transform.position, closestPoint);
				var explosionMultiply = 1f - (distance / _explosionRadius);
				explosionMultiply = Mathf.Clamp(explosionMultiply, _minGeneralMultiply, 1f);

				if (target.TryGetComponent(out ITakeDamage damageTaker))
					damageTaker.TakeDamage(_damage * explosionMultiply * _damageMultiply, DamageType.Explosive);

				if (!target.TryGetComponent(out Rigidbody rigidbody))
					continue;

				var directional = closestPoint - _transform.position;
				rigidbody.AddForceAtPosition(directional * _impulseForceMultiply * explosionMultiply, _transform.position, ForceMode.Impulse);
			}
		}
	}
}
