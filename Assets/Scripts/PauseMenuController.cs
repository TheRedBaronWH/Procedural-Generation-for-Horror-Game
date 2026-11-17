using Sirenix.OdinInspector;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu = null;

    [ShowInInspector]
    [ReadOnly]
    private bool isPaused = false;
    [ShowInInspector]
    [ReadOnly]
    private bool settingsOpen = false;

    void Start()
    {
        if (_pauseMenu == null)
        {
            Debug.LogError($"{gameObject.name} : PauseMenu reference is missing");
        }
        else
        {
            _pauseMenu.SetActive(false);
        }
    }

    void Update()
    {
        if (InputReader.Instance.ReadEscape())
        {
            if (!isPaused)
            {
                _pauseMenu.SetActive(true);
                isPaused = true;
                Time.timeScale = 0f;
            }
            else
            {
                if (!settingsOpen)
                {
                    Resume();
                }
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        _pauseMenu.SetActive(false);
        isPaused = false;
    }

    public void Settings()
    {
        SceneLoader.Instance.LoadSettings();
        settingsOpen = true;
        SettingsController.OnBackClicked += OnSettingsClosed;
    }

    public void OnSettingsClosed()
    {
        settingsOpen = false;
        SettingsController.OnBackClicked -= OnSettingsClosed;
    }

    public void MainMenu()
    {
        SceneLoader.Instance.LoadMainMenu();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
