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

    [SerializeField] private BoxCollider2D answerSpawnArea;

    private bool eventActive;

    private int correctAnswer;

    void Start()
    {
        guruPanel.SetActive(false);

        StartCoroutine(EventLoop());
    }

    IEnumerator EventLoop()
    {
        while (true)
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
            timer -= Time.deltaTime;

            timerText.text =
                Mathf.Ceil(timer).ToString();

            yield return null;
        }

        // Waktu habis
        Debug.Log("Waktu Habis");

        EndEvent();
    }

    void GenerateQuestion()
    {
        int a = Random.Range(1, 11);
        int b = Random.Range(1, 11);

        bool isAddition = Random.value > 0.5f;

        if (isAddition)
        {
            correctAnswer = a + b;

            questionText.text =
                "Berapa " + a + " + " + b + " ?";
        }
        else
        {
            // Mencegah hasil negatif
            if (a < b)
            {
                int temp = a;
                a = b;
                b = temp;
            }

            correctAnswer = a - b;

            questionText.text =
                "Berapa " + a + " - " + b + " ?";
        }
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

        // Acak posisi jawaban
        for (int i = 0; i < answers.Count; i++)
        {
            int rnd = Random.Range(0, answers.Count);

            int temp = answers[i];
            answers[i] = answers[rnd];
            answers[rnd] = temp;
        }

        foreach (int ans in answers)
        {
            Vector2 spawnPos = GetRandomPosition();

            GameObject obj =
                Instantiate(answerPrefab, spawnPos, Quaternion.identity, answerParent);

            AnswerObject answerObj = obj.GetComponent<AnswerObject>();

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

        // Tambah score di sini

        EndEvent();
    }

    public void WrongAnswer()
    {
        Debug.Log("Jawaban Salah");

        // Kurangi nyawa di sini

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