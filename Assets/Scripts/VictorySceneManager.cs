using UnityEngine;
using UnityEngine.SceneManagement;

public class VictorySceneManager : MonoBehaviour
{
    public void NextLevel()
    {
        string currentScene =
            PlayerPrefs.GetString("LastCompletedLevel");

        if (currentScene == "Level1")
        {
            SceneManager.LoadScene("Level2");
        }
        else if (currentScene == "Level2")
        {
            SceneManager.LoadScene("Level3");
        }
        else
        {
            SceneManager.LoadScene("LevelSelect");
        }
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}