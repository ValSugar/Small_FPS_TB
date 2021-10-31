using System;
using System.Collections;
using UnityEngine;
using Weapons.Missiles;

namespace Weapons
{
	public abstract class WeaponBase : MonoBehaviour
	{
		[SerializeField] private string _name;
		[SerializeField] protected MissileBase _missilePrefab;
		[SerializeField] protected Transform _muzzle;
		[SerializeField] private float _fireCooldown;
		[SerializeField] private float _reloadTime;
		[SerializeField] private int _maxShotsCount;
		[SerializeField] private int _missileCountPerShot;
		[SerializeField] private bool _isAutoWeapon;

		private int _currentShotsCount;

		private bool _isReadyToFire;

		private Coroutine _reloadCoroutine;

		public Action<int, int> onMissileCountChanged;
		public string Name => _name;

		private void Awake()
		{
			_isReadyToFire = true;
			_currentShotsCount = _maxShotsCount;
		}

		private void OnEnable()
		{
			onMissileCountChanged?.Invoke(_currentShotsCount, _maxShotsCount);

			if (!_isReadyToFire)
				_reloadCoroutine = StartCoroutine(ReloadCoroutine());
		}

		public void TryFire(bool withDoubleDamage)
		{
			var hasInput = (_isAutoWeapon && Input.GetKey(KeyCode.Mouse0)) || (!_isAutoWeapon && Input.GetKeyDown(KeyCode.Mouse0));
			if (!hasInput)
				return;

			if (!_isReadyToFire)
				return;

			for (var i = 0; i < _missileCountPerShot; i++)
				Fire(withDoubleDamage);

			--_currentShotsCount;
			onMissileCountChanged?.Invoke(_currentShotsCount, _maxShotsCount);

			_isReadyToFire = false;

			_reloadCoroutine = StartCoroutine(ReloadCoroutine());
		}

		protected abstract void Fire(bool withDoubleDamage);

		private IEnumerator ReloadCoroutine()
		{
			var delay = _currentShotsCount <= 0 ? _reloadTime : _fireCooldown;

			yield return new WaitForSeconds(delay);

			_isReadyToFire = true;

			if (_currentShotsCount > 0)
				yield break;

			_currentShotsCount = _maxShotsCount;
			onMissileCountChanged?.Invoke(_currentShotsCount, _maxShotsCount);
		}

		private void OnDisable()
		{
			if (_reloadCoroutine != null)
				StopCoroutine(_reloadCoroutine);

			onMissileCountChanged = null;
		}
	}
}