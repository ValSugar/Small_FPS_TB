using System;
using System.Collections;
using System.IO;
using System.Linq;
using Entities;
using Player;
using UI;
using UnityEngine;

namespace Location
{
	public class LevelDirector : MonoBehaviour
	{
		public static LevelDirector Instance;

		[SerializeField] private PlayerMechanics _playerMechanics;
		[SerializeField] private ScoreByDamageContainer[] _scoreByDamageContainers;

		private string _saveDataPath;
		private Settings _settings;
		private int _score;

		private void Awake()
		{
			Instance = this;

			_saveDataPath = Path.Combine(Application.persistentDataPath, "Settings.tt");
			TryLoadSettings();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				InGameUI.SwitchInGameMenu();
		}

		public void AddScoreByDamageType(DamageType damageType)
		{
			var suitableContainer = _scoreByDamageContainers.FirstOrDefault(x => x.damageType == damageType);
			if (suitableContainer == null)
				return;

			_score += suitableContainer.score;
			InGameUI.SetScore(_score);
		}

		public void SetSpeedMultiply(float value, Action<float, float, float> changedSpeedCallback)
		{
			_settings.speedMultiply = value;
			_playerMechanics.SetSpeedMultiply(value, changedSpeedCallback);
		}

		public float GetSpeedMultiply() => _playerMechanics.GetSpeedMultiply();

		public void SwitchPause(bool flag)
		{
			Time.timeScale = flag ? 0f : 1f;
			_playerMechanics.SwitchLockControls(flag);
		}

		#region SAVE_LOAD

		private void TryLoadSettings()
		{
			if (!File.Exists(_saveDataPath))
			{
				_settings = new Settings();
				return;
			}

			_settings = JsonUtility.FromJson<Settings>(File.ReadAllText(_saveDataPath));
			_playerMechanics.SetSpeedMultiply(_settings.speedMultiply, null);
		}

		public void SaveSettings()
		{
			File.WriteAllText(_saveDataPath, JsonUtility.ToJson(_settings));
		}

		#endregion

		[Serializable]
		private class ScoreByDamageContainer
		{
			public DamageType damageType;
			public int score;
		}

		[Serializable]
		private class Settings
		{
			public float speedMultiply;
		}
	}
}