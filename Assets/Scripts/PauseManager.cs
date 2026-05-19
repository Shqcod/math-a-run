using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private TMP_Text warningText;
    [SerializeField] private GameObject pauseButton;

    [Header("Quiz")]
    [SerializeField] private QuizEventManager quizManager;

    private bool isPaused;

    private void Start()
    {
        pausePanel.SetActive(false);

        warningText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TryPause();
        }
    }

    public void PauseButton()
    {
        TryPause();
    }

    void TryPause()
    {
        // Tidak boleh pause saat quiz aktif
        if (quizManager.EventActive)
        {
            StopAllCoroutines();

            StartCoroutine(ShowWarning());

            return;
        }

        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    void ResetPauseState()
    {
        Time.timeScale = 1f;

        AudioListener.pause = false;

        isPaused = false;
    }

    void PauseGame()
    {
        isPaused = true;

        pausePanel.SetActive(true);

        pauseButton.SetActive(false);

        Time.timeScale = 0f;

        AudioListener.pause = true;
    }

    public void ResumeGame()
    {
        isPaused = false;

        pausePanel.SetActive(false);

        pauseButton.SetActive(true);

        Time.timeScale = 1f;

        AudioListener.pause = false;
    }

    public void RestartLevel()
    {
        ResetPauseState();

        Scene currentScene =
            SceneManager.GetActiveScene();

        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void MainMenu()
    {
        ResetPauseState();

        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator ShowWarning()
    {
        warningText.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(2f);

        warningText.gameObject.SetActive(false);
    }
}