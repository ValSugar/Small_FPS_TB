using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private Button _newGame;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exit;
        [SerializeField] private GameObject _setingsWindow;

		private void Awake()
		{
            _newGame.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Time.timeScale = 1f;
            });
            _settingsButton.onClick.AddListener(() =>
            {
	            _setingsWindow.SetActive(!_setingsWindow.activeSelf);
            });
            _exit.onClick.AddListener(() =>
            {
                Application.Quit();
            });
        }

		private void OnDisable()
		{
			_setingsWindow.SetActive(false);
        }
    }
}
