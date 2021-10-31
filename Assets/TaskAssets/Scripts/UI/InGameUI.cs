using Location;
using TMPro;
using UnityEngine;
using Weapons;

namespace UI
{
    public class InGameUI : MonoBehaviour
    {
        public static InGameUI Instance;

        [SerializeField] private TMP_Text _scoreCount;
        [SerializeField] private TMP_Text _weaponName;
        [SerializeField] private TMP_Text _missileCount;
        [SerializeField] private GameObject _inGameMenu;

        private void Awake()
		{
			Instance = this;
		}

		public static void SetWeapon(WeaponBase weapon)
		{
			Instance._weaponName.text = weapon.Name;
			weapon.onMissileCountChanged = Instance.OnMissileCountChanged;
		}

		public static void SetScore(int count)
		{
			Instance._scoreCount.text = $"Score: {count}";
		}

		public static void SwitchInGameMenu()
		{
			Instance._inGameMenu.SetActive(!Instance._inGameMenu.activeSelf);
			LevelDirector.Instance.SwitchPause(Instance._inGameMenu.activeSelf);
			Cursor.lockState = Instance._inGameMenu.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
		}

		private void OnMissileCountChanged(int currentCount, int maxCount)
		{
			_missileCount.text = $"{currentCount}/{maxCount}";
		}
    }
}
