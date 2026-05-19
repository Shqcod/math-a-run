using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QuizEventManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject guruPanel;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_Text timerText;

    [Header("Quiz Timing")]
    [SerializeField] private float eventInterval = 10f;
    [SerializeField] private float answerDuration = 8f;

    [Header("Answer Spawn")]
    [SerializeField] private Transform answerParent;
    [SerializeField] private GameObject answerPrefab;
    [SerializeField] private float minimumAnswerDistance = 2f;
    [SerializeField] private Transform player;
    [SerializeField] private float minimumDistanceFromPlayer = 3f;

    [SerializeField] private BoxCollider2D answerSpawnArea;

    [Header("Game Manager")]
    [SerializeField] private GameManager gameManager;

    public bool EventActive => eventActive;
    private bool eventActive;

    public enum LevelType
    {
        Level1,
        Level2,
        Level3
    }

    [Header("Level")]
    [SerializeField] private LevelType currentLevel;

    private int correctAnswer;

    void Start()
    {
        guruPanel.SetActive(false);

        StartCoroutine(EventLoop());
    }

    IEnumerator EventLoop()
    {
        while (!gameManager.IsLevelFinished())
        {
            yield return new WaitForSeconds(eventInterval);

            yield return StartCoroutine(StartQuizEvent());
        }
    }

    IEnumerator StartQuizEvent()
    {
        eventActive = true;

        guruPanel.SetActive(true);

        GenerateQuestion();

        SpawnAnswers();

        float timer = answerDuration;

        while (timer > 0)
        {
            // Jika quiz sudah selesai
            if (!eventActive)
            {
                yield break;
            }

            timer -= Time.deltaTime;

            timerText.text =
                Mathf.Ceil(timer).ToString();

            yield return null;
        }

        // Waktu habis
        Debug.Log("Waktu Habis");
        gameManager.TakeDamage();
        gameManager.CompleteQuestion();
        EndEvent();
    }

    void GenerateQuestion()
    {
        Debug.Log("Current Level : " + currentLevel);

        switch(currentLevel)
        {
            case LevelType.Level1:
                GenerateLevel1Question();
                break;

            case LevelType.Level2:
                GenerateLevel2Question();
                break;

            case LevelType.Level3:
                GenerateLevel3Question();
                break;
        }
    }

    void GenerateLevel1Question()
    {
        Debug.Log("LEVEL 2 QUESTION");

        int a = Random.Range(1, 11);
        int b = Random.Range(1, 11);

        bool isAddition = Random.value > 0.5f;

        if (isAddition)
        {
            correctAnswer = a + b;

            questionText.text =
                a + " + " + b;
        }
        else
        {
            if (a < b)
            {
                int temp = a;
                a = b;
                b = temp;
            }

            correctAnswer = a - b;

            questionText.text =
                a + " - " + b;
        }
    }

    void GenerateLevel2Question()
    {
        int operation = Random.Range(0, 4);

        int a = 0;
        int b = 0;

        switch(operation)
        {
            // PENJUMLAHAN
            case 0:

                correctAnswer =
                    Random.Range(1, 51);

                a =
                    Random.Range(1, correctAnswer);

                b =
                    correctAnswer - a;

                questionText.text =
                    a + " + " + b;

                break;

            // PENGURANGAN
            case 1:

                correctAnswer =
                    Random.Range(-25, 26);

                b =
                    Random.Range(1, 26);

                a =
                    correctAnswer + b;

                questionText.text =
                    a + " - " + b;

                break;

            // PERKALIAN
            case 2:

                a = Random.Range(1, 10);
                b = Random.Range(1, 10);

                correctAnswer = a * b;

                questionText.text =
                    a + " × " + b;

                break;

            // PEMBAGIAN
            case 3:

                b = Random.Range(1, 11);

                correctAnswer =
                    Random.Range(1, 11);

                a = correctAnswer * b;

                // Pastikan ≤ 100
                while (a > 100)
                {
                    b = Random.Range(1, 11);

                    correctAnswer =
                        Random.Range(1, 11);

                    a = correctAnswer * b;
                }

                questionText.text =
                    a + " ÷ " + b;

                break;
        }
    }

    void GenerateLevel3Question()
    {
        Debug.Log("LEVEL 3 QUESTION");

        bool useMultiply =
            Random.value > 0.5f;

        bool heavyFirst =
            Random.value > 0.5f;

        string heavyOp =
            useMultiply ? "×" : "÷";

        string lightOp =
            Random.value > 0.5f ? "+" : "-";

        int a = 0;
        int b = 0;
        int c = 0;

        int heavyResult = 0;

        // =========================
        // OPERASI BERAT
        // =========================

        if (useMultiply)
        {
            a = Random.Range(1, 10);
            b = Random.Range(1, 10);

            heavyResult = a * b;
        }
        else
        {
            b = Random.Range(1, 11);

            heavyResult =
                Random.Range(1, 11);

            a = heavyResult * b;

            while (a > 100)
            {
                b = Random.Range(1, 11);

                heavyResult =
                    Random.Range(1, 11);

                a = heavyResult * b;
            }
        }

        // =========================
        // OPERASI RINGAN
        // =========================

        if (lightOp == "+")
        {
            c = Random.Range(1, 51);

            if (heavyFirst)
            {
                // (a × b) + c
                correctAnswer =
                    heavyResult + c;
            }
            else
            {
                // c + (a × b)
                correctAnswer =
                    c + heavyResult;
            }
        }
        else
        {
            c = Random.Range(1, 26);

            if (heavyFirst)
            {
                // (a × b) - c
                correctAnswer =
                    heavyResult - c;
            }
            else
            {
                // c - (a × b)
                correctAnswer =
                    c - heavyResult;
            }
        }

        string expression;

        // =========================
        // FORMAT SOAL
        // =========================

        if (heavyFirst)
        {
            expression =
                "(" + a + " " + heavyOp + " " + b + ") "
                + lightOp + " " + c;
        }
        else
        {
            expression =
                c + " " + lightOp + " "
                + "(" + a + " " + heavyOp + " " + b + ")";
        }

        questionText.text = expression;

        Debug.Log("Question : " + expression);
        Debug.Log("Correct Answer : " + correctAnswer);
    }

    void SpawnAnswers()
    {
        ClearAnswers();

        List<int> answers = new List<int>();

        answers.Add(correctAnswer);

        while (answers.Count < 3)
        {
            int wrong = Random.Range(1, 11);

            if (!answers.Contains(wrong))
            {
                answers.Add(wrong);
            }
        }

        // Acak jawaban
        for (int i = 0; i < answers.Count; i++)
        {
            int rnd = Random.Range(0, answers.Count);

            int temp = answers[i];
            answers[i] = answers[rnd];
            answers[rnd] = temp;
        }

        List<Vector2> usedPositions = new List<Vector2>();

        foreach (int ans in answers)
        {
            Vector2 spawnPos =
                GetNonOverlappingPosition(usedPositions);

            usedPositions.Add(spawnPos);

            GameObject obj =
                Instantiate(
                    answerPrefab,
                    spawnPos,
                    Quaternion.identity,
                    answerParent
                );

            AnswerObject answerObj =
                obj.GetComponent<AnswerObject>();

            bool isCorrect = ans == correctAnswer;

            answerObj.Setup(ans, isCorrect, this);
        }
    }

    Vector2 GetRandomPosition()
    {
        Bounds bounds = answerSpawnArea.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(randomX, randomY);
    }

    Vector2 GetNonOverlappingPosition(List<Vector2> usedPositions)
    {
        Bounds bounds = answerSpawnArea.bounds;

        int attempts = 0;

        while (attempts < 50)
        {
            float randomX =
                Random.Range(bounds.min.x, bounds.max.x);

            float randomY =
                Random.Range(bounds.min.y, bounds.max.y);

            Vector2 newPos =
                new Vector2(randomX, randomY);

            bool invalidPosition = false;

            // Cek terlalu dekat dengan jawaban lain
            foreach (Vector2 pos in usedPositions)
            {
                if (Vector2.Distance(newPos, pos)
                    < minimumAnswerDistance)
                {
                    invalidPosition = true;
                    break;
                }
            }

            // Cek terlalu dekat dengan player
            if (!invalidPosition &&
                player != null)
            {
                if (Vector2.Distance(
                    newPos,
                    player.position)
                    < minimumDistanceFromPlayer)
                {
                    invalidPosition = true;
                }
            }

            // Posisi valid
            if (!invalidPosition)
            {
                return newPos;
            }

            attempts++;
        }

        // fallback
        return Vector2.zero;
    }

    void ClearAnswers()
    {
        foreach (Transform child in answerParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void CorrectAnswer()
    {
        Debug.Log("Jawaban Benar");
        gameManager.AddCorrectScore();
        gameManager.Heal(1);
        gameManager.CompleteQuestion();
        EndEvent();
    }

    public void WrongAnswer()
    {
        Debug.Log("Jawaban Salah");
        gameManager.TakeDamage();
        gameManager.CompleteQuestion();
        EndEvent();
    }

    void EndEvent()
    {
        eventActive = false;
        guruPanel.SetActive(false);
        ClearAnswers();
    }

    private void OnDrawGizmos()
    {
        if(answerSpawnArea == null) return;

        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(
            answerSpawnArea.bounds.center,
            answerSpawnArea.bounds.size
        );
    }
}