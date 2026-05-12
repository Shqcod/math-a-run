using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    // Tombol Menu
    public void MainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }

    // Tombol Play Again
    public void PlayAgain()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("GameScene");
    }
}