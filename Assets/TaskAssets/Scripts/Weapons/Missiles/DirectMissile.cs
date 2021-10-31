using Entities;
using UnityEngine;
using Utilities;

namespace Weapons.Missiles
{
	public class DirectMissile : MissileBase
	{
		public const float DISTANCE_CHECK = 100f;
		public const int EXCLUDED_LAYER = 1 << 9;

		private Vector3 _targetPosition;

		private void OnEnable()
		{
			var ray = CameraRayHelper.GetCenterCameraRay();
			if (!Physics.Raycast(ray, out RaycastHit hit, DISTANCE_CHECK, ~EXCLUDED_LAYER))
			{
				_targetPosition = _transform.position + _transform.forward * DISTANCE_CHECK;
				return;
			}

			_targetPosition = hit.point;
		}

		private void FixedUpdate()
		{
			if (Physics.Raycast(_transform.position, _transform.forward, out RaycastHit hit, DISTANCE_CHECK, ~EXCLUDED_LAYER))
				_targetPosition = hit.point;

			_transform.position = Vector3.MoveTowards(_transform.position, _targetPosition, _speed * Time.fixedDeltaTime);

			if (Vector3.Distance(_transform.position, _targetPosition) < 0.1f)
			{
				Disable();

				if (hit.collider == null)
					return;

				OnHit(hit.collider);
			}
		}

		protected override void OnHit(Collider collider)
		{
			if (collider.TryGetComponent(out ITakeDamage damageTaker))
				damageTaker.TakeDamage(_damage * _damageMultiply, DamageType.Common);
		}
	}
}