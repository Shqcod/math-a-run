using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Score")]
    public TextMeshProUGUI scoreText;

    private int score = 0;
    private float timer;

    [Header("Health")]
    public int health = 3;

    public Image[] hearts;

    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Invincible")]
    public float invincibleDuration = 1f;

    private bool isInvincible = false;

    [Header("Player Visual")]
    public SpriteRenderer playerSprite;

    [Header("Game Over")]
    public GameObject gameOverPanel;

    private bool isGameOver = false;

    void Start()
    {
        gameOverPanel.SetActive(false);
        UpdateScoreUI();
        UpdateHealthUI();
    }

    void Update()
    {
        if (isGameOver)
            return;

        // Tambah score tiap 0.5 detik
        timer += Time.deltaTime;

        if (timer >= 0.5f)
        {
            timer = 0f;
            score++;
            UpdateScoreUI();
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score : " + score;
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

    void GameOver()
    {
        isGameOver = true;

        gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }
}