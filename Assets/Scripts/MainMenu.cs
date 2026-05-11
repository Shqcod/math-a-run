using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Tombol Start Ditekan");
        SceneManager.LoadScene("GameScene");
    }
}