using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void OptionGame()
    {
        SceneManager.LoadScene("OptionScene");
    }

    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Game Closed");
    }
}