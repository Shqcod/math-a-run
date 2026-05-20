using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Images")]
    public Sprite[] tutorialSprites;

    [Header("UI")]
    public Image tutorialImage;

    private int currentIndex = 0;

    void Start()
    {
        ShowImage();
    }

    public void NextImage()
    {
        currentIndex++;

        // Jika sudah gambar terakhir
        if (currentIndex >= tutorialSprites.Length)
        {
            BackToMainMenu();
            return;
        }

        ShowImage();
    }

    public void PreviousImage()
    {
        currentIndex--;

        if (currentIndex < 0)
        {
            currentIndex = 0;
        }

        ShowImage();
    }

    // Tombol Skip
    public void SkipTutorial()
    {
        BackToMainMenu();
    }

    // Tombol Back
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void ShowImage()
    {
        tutorialImage.sprite = tutorialSprites[currentIndex];
    }
}