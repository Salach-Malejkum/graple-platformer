using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenu : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset mainMenu;
#endif

    [SerializeField] private string mainMenuName;

    public void OnContinue()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnMainMenu()
    {
        if (!string.IsNullOrEmpty(mainMenuName))
        {
            SceneManager.LoadScene(mainMenuName);
            Time.timeScale = 1;
        }

    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (mainMenu != null)
        {
            mainMenuName = mainMenu.name;
        }
    }
#endif
}

