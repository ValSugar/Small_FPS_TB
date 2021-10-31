using System;
using Location;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace UI
{
	public class SettingsView : MonoBehaviour
	{
		[SerializeField] private TMP_Text _forwardSpeedLabel;
		[SerializeField] private TMP_Text _backSpeedLabel;
		[SerializeField] private TMP_Text _strafeSpeedLabel;
		[SerializeField] private TMP_Text _minSpeedLabel;
		[SerializeField] private TMP_Text _maxSpeedLabel;
		[SerializeField] private TMP_Text _speedMultiplyLabel;
		[SerializeField] private Slider _speedSlider;
		[SerializeField] private Button _saveSettingsButton;

		private void Awake()
		{
			_minSpeedLabel.SetText(_speedSlider.minValue.ToString());
			_maxSpeedLabel.SetText(_speedSlider.maxValue.ToString());

			_speedSlider.onValueChanged.AddListener(OnSpeedMultiplyChange);

			_saveSettingsButton.onClick.AddListener(() =>
			{
				LevelDirector.Instance.SaveSettings();
			});
		}

		private void OnEnable()
		{
			_speedSlider.value = LevelDirector.Instance.GetSpeedMultiply();
		}

		private void OnSpeedMultiplyChange(float value)
		{
			LevelDirector.Instance.SetSpeedMultiply(value, SetSpeedLabels);
			_speedMultiplyLabel.SetText((Mathf.Round(value * 100) / 100).ToString());
		}

		private void SetSpeedLabels(float forward, float back, float strafe)
		{
			_forwardSpeedLabel.SetText($"Forward speed: {Mathf.Round(forward * 10) / 10}");
			_backSpeedLabel.SetText($"Back speed: {Mathf.Round(back * 10) / 10}");
			_strafeSpeedLabel.SetText($"Strafe speed: {Mathf.Round(strafe * 10) / 10}");
		}
	}
}