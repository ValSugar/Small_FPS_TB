using System;
using DG.Tweening;
using UI;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using Weapons;

namespace Player
{
	public class PlayerMechanics : MonoBehaviour
	{
		[SerializeField] private RigidbodyFirstPersonController _movementController;
		[SerializeField] private Transform _weaponsHolder;
		[SerializeField] private PlayerView _view;

		private Inventory _inventory;
		private WeaponBase _currentWeapon;

		private bool _crouchMode;
		private bool _readyToFire;
		private bool _isLockControls;

		private void Awake()
		{
			_inventory = new Inventory();
			_readyToFire = false;
		}

		private void Start()
		{
			SetWeapon(_inventory.InitAndGetFirstWeapon(_weaponsHolder));
		}

		private void Update()
		{
			if (_isLockControls)
				return;

			if (Input.GetKeyDown(KeyCode.Q))
				SetWeapon(_inventory.GetNextWeapon(_currentWeapon));

			if (Input.GetKeyDown(KeyCode.C))
				_crouchMode = _movementController.SwitchCrouchMode();

			if (_readyToFire)
				_currentWeapon.TryFire(_crouchMode);
		}

		private void SetWeapon(WeaponBase newWeapon)
		{
			_readyToFire = false;
			_view.ChangeWeapon(OnWeaponReadyToChange, () => _readyToFire = true);

			void OnWeaponReadyToChange()
			{
				if (_currentWeapon != null)
					_currentWeapon.gameObject.SetActive(false);

				_currentWeapon = newWeapon;
				InGameUI.SetWeapon(_currentWeapon);
				_currentWeapon.gameObject.SetActive(true);
			}
		}

		public void SetSpeedMultiply(float value, Action<float, float, float> changedSpeedCallback)
		{
			var movementSettings = _movementController.movementSettings;
			movementSettings.SpeedMultiply = value;
			changedSpeedCallback?.Invoke(
				movementSettings.ForwardSpeed * value,
				movementSettings.BackwardSpeed * value,
				movementSettings.StrafeSpeed * value);
		}

		public float GetSpeedMultiply() => _movementController.movementSettings.SpeedMultiply;

		public void SwitchLockControls(bool flag) => _isLockControls = flag;
	}
}
