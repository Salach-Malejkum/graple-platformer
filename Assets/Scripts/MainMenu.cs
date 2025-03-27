using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SceneAsset level1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnStart()
    {
        SceneManager.LoadScene(level1.name);
    }

    public void OnQuit()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
