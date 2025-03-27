using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private SceneAsset mainMenu;

    public void OnContinue()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        Time.timeScale = 1;
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene(mainMenu.name);
        Time.timeScale = 1;
    }
}
