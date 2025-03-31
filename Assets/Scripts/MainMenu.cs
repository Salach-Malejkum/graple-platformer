using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset level1;
#endif

    [SerializeField] private string level1Name;

    public void OnStart()
    {
        if (!string.IsNullOrEmpty(level1Name))
        {
            SceneManager.LoadScene(level1Name);
        }
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (level1 != null)
        {
            level1Name = level1.name;
        }
    }
#endif
}


