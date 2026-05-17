using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;

    [Header("Locked Text")]
    public TMP_Text lockedText;

    private void Start()
    {
        // Level 1 selalu terbuka
        level1Button.interactable = true;

        // Cek unlock
        bool level2Unlocked =
            PlayerPrefs.GetInt("Level2Unlocked", 0) == 1;

        bool level3Unlocked =
            PlayerPrefs.GetInt("Level3Unlocked", 0) == 1;

        level2Button.interactable = level2Unlocked;
        level3Button.interactable = level3Unlocked;

        lockedText.gameObject.SetActive(false);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void LoadLevel2()
    {
        bool unlocked =
            PlayerPrefs.GetInt("Level2Unlocked", 0) == 1;

        if (unlocked)
        {
            SceneManager.LoadScene("Level2");
        }
        else
        {
            ShowLockedText();
        }
    }

    public void LoadLevel3()
    {
        bool unlocked =
            PlayerPrefs.GetInt("Level3Unlocked", 0) == 1;

        if (unlocked)
        {
            SceneManager.LoadScene("Level3");
        }
        else
        {
            ShowLockedText();
        }
    }

    void ShowLockedText()
    {
        lockedText.gameObject.SetActive(true);

        lockedText.text = "Level Masih Terkunci";
    }
}