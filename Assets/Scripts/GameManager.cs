using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Score")]
    public TextMeshProUGUI scoreText;

    private int score = 0;
    private float timer;

    [Header("Health")]
    public int health = 3;
    public int maxHealth = 3;

    public Image[] hearts;

    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Invincible")]
    public float invincibleDuration = 1f;

    private bool isInvincible = false;

    [Header("Player Visual")]
    public SpriteRenderer playerSprite;

    [Header("Question Progress")]
    public int totalQuestions = 10;
    private int currentQuestion = 0;
    private int scorePerQuestion;
    public TextMeshProUGUI questionProgressText;

    [SerializeField] private int currentLevel = 1;
    private bool levelFinished = false;
    private bool isGameOver = false;

    void Start()
    {
        PlayerPrefs.SetString(
            "LastPlayedLevel",
            SceneManager.GetActiveScene().name
        );
        scorePerQuestion = Mathf.RoundToInt(100f / totalQuestions);
        UpdateScoreUI();
        UpdateHealthUI();
    }

    void Update()
    {
        if (isGameOver)
            return;
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score : " + score;
    }

    public void AddCorrectScore()
    {
        score += scorePerQuestion;

        // Supaya tidak lebih dari 100
        if (score > 100)
            score = 100;

        UpdateScoreUI();
    }

    public void TakeDamage()
    {
        // Jika sedang invincible atau game over
        if (isGameOver || isInvincible)
            return;

        health--;

        UpdateHealthUI();

        // Mulai invincible
        StartCoroutine(InvincibleCoroutine());

        if (health <= 0)
        {
            GameOver();
        }
    }

    private System.Collections.IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;

        float elapsed = 0f;

        while (elapsed < invincibleDuration)
        {
            // Kedip
            playerSprite.enabled = false;
            yield return new WaitForSeconds(0.1f);

            playerSprite.enabled = true;
            yield return new WaitForSeconds(0.1f);

            elapsed += 0.2f;
        }

        playerSprite.enabled = true;

        isInvincible = false;
    }

    public void Heal(int amount)
    {
        health += amount;

        if (health > maxHealth)
            health = maxHealth;

        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    public bool IsLevelFinished()
    {
        return levelFinished;
    }

    void Victory()
    {
        levelFinished = true;

        Time.timeScale = 1f;

        PlayerPrefs.SetString(
            "LastCompletedLevel",
            SceneManager.GetActiveScene().name
        );

        if (currentLevel == 1)
        {
            PlayerPrefs.SetInt("Level2Unlocked", 1);
        }
        else if (currentLevel == 2)
        {
            PlayerPrefs.SetInt("Level3Unlocked", 1);
        }

        SceneManager.LoadScene("VictoryScene");
    }

    public void CompleteQuestion()
    {
        currentQuestion++;

        questionProgressText.text =
            currentQuestion + " / " + totalQuestions;

        if (currentQuestion >= totalQuestions)
        {
            Victory();
        }
    }

    void GameOver()
    {
        isGameOver = true;

        Time.timeScale = 1f;

        SceneManager.LoadScene("GameOverScene");
    }
}