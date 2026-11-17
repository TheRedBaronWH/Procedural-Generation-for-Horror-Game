using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum Scene
    {
        Master = 0,
        MainMenu = 1,
        Map = 2,
    }

    public enum UIScene
    {
        GameUI = 3,
        SettingsUI = 4,
    }

    [SerializeField] private List<Scene> _scenesWithOtherCamera = new();

    public static SceneLoader Instance { get; private set; }

    [ShowInInspector]
    [ReadOnly]
    private Scene _activeScene = Scene.Master;

    [ShowInInspector]
    [ReadOnly]
    private List<UIScene> _activeUIScenes = new();

    [SerializeField] private GameObject _mainCamera = null;
    [SerializeField] private GameObject _loadingHider = null;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError($"{gameObject.name} : Multiple instances of SceneLoader detected. Destroying duplicate.");
        }
    }

    void Start()
    {
        if (_loadingHider == null)
        {
            Debug.LogError($"{gameObject.name} : LoadingHider reference is missing");
        }

        if(_mainCamera == null)
        {
            Debug.LogError($"{gameObject.name} : MainCamera reference is missing");
        }

        LoadScene(Scene.MainMenu);
    }

    public void LoadMainMenu()
    {
        LoadScene(Scene.MainMenu);
    }

    public void LoadMap()
    {
        LoadScene(Scene.Map);
        LoadUIScene(UIScene.GameUI);
    }

    public void LoadSettings()
    {
        LoadUIScene(UIScene.SettingsUI);
    }

    public void UnloadSettings()
    {
        UnloadUIScene(UIScene.SettingsUI);
    }

    private async void LoadScene(Scene scene)
    {
        if (scene == _activeScene || scene == Scene.Master) return;
        _loadingHider.SetActive(true);

        if(_scenesWithOtherCamera.Contains(scene))
        {
            _mainCamera.SetActive(false);
        }
        else
        {
            _mainCamera.SetActive(true);
        }

        if (_activeScene != Scene.Master)
        {
            await SceneManager.UnloadSceneAsync((int)_activeScene);
        }
        if (_activeUIScenes.Count > 0)
        {
            foreach (var uiScene in _activeUIScenes)
            {
                await SceneManager.UnloadSceneAsync((int)uiScene);
            }
            _activeUIScenes.Clear();
        }

        await SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);
        _loadingHider.SetActive(false);
        _activeScene = scene;
    }

    private async void LoadUIScene(UIScene scene)
    {
        if (_activeUIScenes.Contains(scene)) return;
        await SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);
        _activeUIScenes.Add(scene);
    }

    private async void UnloadUIScene(UIScene scene)
    {
        if (!_activeUIScenes.Contains(scene)) return;
        await SceneManager.UnloadSceneAsync((int)scene);
        _activeUIScenes.Remove(scene);
    }
}

#if UNITY_EDITOR
    [InitializeOnLoadAttribute]
    public static class DefaultSceneLoader
    {
        static DefaultSceneLoader()
        {
            EditorApplication.playModeStateChanged += LoadDefaultScene;
        }

        static void LoadDefaultScene(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            }

            if (state == PlayModeStateChange.EnteredPlayMode)
            {
            EditorSceneManager.LoadScene(0);
            }
        }
    }
#endif
