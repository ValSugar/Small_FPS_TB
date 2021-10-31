using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Weapons;

namespace Player
{
	public class Inventory
	{
		private List<WeaponBase> _weapons;

		public WeaponBase InitAndGetFirstWeapon(Transform weaponsHolder)
		{
			_weapons = weaponsHolder.GetComponentsInChildren<WeaponBase>(true).ToList();
			return _weapons[0];
		}

		public WeaponBase GetNextWeapon(WeaponBase weapon)
		{
			var index = _weapons.IndexOf(weapon);
			if (++index >= _weapons.Count)
				index = 0;

			return _weapons[index];
		}
	}
}
