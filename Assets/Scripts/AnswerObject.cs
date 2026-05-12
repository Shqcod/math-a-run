using UnityEngine;
using TMPro;

public class AnswerObject : MonoBehaviour
{
    [SerializeField] private TMP_Text answerText;

    private bool isCorrect;

    private QuizEventManager quizManager;

    public void Setup(
        int value,
        bool correct,
        QuizEventManager manager
    )
    {
        answerText.text = value.ToString();

        isCorrect = correct;

        quizManager = manager;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (isCorrect)
        {
            quizManager.CorrectAnswer();
        }
        else
        {
            quizManager.WrongAnswer();
        }

        Destroy(gameObject);
    }
}