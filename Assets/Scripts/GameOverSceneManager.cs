using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneManager : MonoBehaviour
{
    public void Retry()
    {
        string lastLevel =
            PlayerPrefs.GetString("LastPlayedLevel");

        SceneManager.LoadScene(lastLevel);
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}