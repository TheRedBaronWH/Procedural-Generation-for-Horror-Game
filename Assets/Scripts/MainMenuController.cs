using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneLoader.Instance.LoadMap();
    }

    public void Settings()
    {
        SceneLoader.Instance.LoadSettings();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
