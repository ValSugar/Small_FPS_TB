using Location;
using PoolObjects;
using System;
using UnityEngine;

namespace Entities
{
	[RequireComponent(typeof(Rigidbody))]
	public class Dummy : MonoBehaviour, ITakeDamage
    {
	    [SerializeField] private float _maxHealth;

	    private float _currentHealth;

	    private bool _isDead;

		private void OnEnable()
		{
			_currentHealth = _maxHealth;
			_isDead = false;
		}

		public void TakeDamage(float damage, DamageType damageType)
		{
			_currentHealth -= damage;
			if (_currentHealth > 0 || _isDead)
				return;

			_isDead = true;
			gameObject.SetActive(false);

			LevelDirector.Instance.AddScoreByDamageType(damageType);
		}

		protected void OnDisable()
		{
			Pool.ReturnToPool(gameObject);
		}
	}
}
