using PoolObjects;
using UnityEngine;

namespace Weapons
{
	public class SimpleWeapon : WeaponBase
	{
		private const float standartDamageMultiply = 1f;

		[SerializeField] private float _spreadRate;

		protected override void Fire(bool withDoubleDamage)
		{
			var damageMultiply = withDoubleDamage ? standartDamageMultiply * 2 : standartDamageMultiply;
			var rotation = _muzzle.rotation.eulerAngles;
			rotation.x += Random.Range(-_spreadRate, _spreadRate);
			rotation.y += Random.Range(-_spreadRate, _spreadRate);
			var missile = Pool.SpawnObject(_missilePrefab, _muzzle.position, Quaternion.Euler(rotation));
			missile.SetDamageMultiply(damageMultiply);
		}
	}
}