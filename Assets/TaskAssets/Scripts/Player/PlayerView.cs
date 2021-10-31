using System;
using DG.Tweening;
using UnityEngine;

namespace Player
{
	public class PlayerView : MonoBehaviour
	{
		[SerializeField] private Transform _weaponsHolder;
		[SerializeField] private Vector3 _angleWeaponForSwap;
		[SerializeField] private float _swapDuration;

		private Sequence _weaponChangeSequence;

		public void ChangeWeapon(Action onReadyToChange, Action onEndCallback)
		{
			if (_weaponChangeSequence != null)
				_weaponChangeSequence?.Kill();

			var originalEulerAngles = _weaponsHolder.localEulerAngles;

			_weaponChangeSequence = DOTween.Sequence()
				.Append(_weaponsHolder.DOLocalRotate(_angleWeaponForSwap, _swapDuration / 2))
				.AppendCallback(onReadyToChange.Invoke)
				.Append(_weaponsHolder.DOLocalRotate(originalEulerAngles, _swapDuration / 2))
				.AppendCallback(onEndCallback.Invoke);
		}
	}
}
