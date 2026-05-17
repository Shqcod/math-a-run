using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Spawner rightSpawner;
    [SerializeField] private Spawner topSpawner;
    [SerializeField] private Spawner bottomSpawner;

    [SerializeField] private float switchTime = 5f;

    private float timer;
    private int currentSpawner;
    private QuizEventManager quizManager;
    private GameManager gameManager;

    private void Start()
    {
        quizManager =
            Object.FindFirstObjectByType<QuizEventManager>();

        gameManager =
            Object.FindFirstObjectByType<GameManager>();

        ActivateSpawner(0);
    }

    private void Update()
    {
        // Stop semua spawn jika level selesai
        if (gameManager != null &&
            gameManager.IsLevelFinished())
        {
            rightSpawner.canSpawn = false;
            topSpawner.canSpawn = false;
            bottomSpawner.canSpawn = false;

            return;
        }

        // Stop spawn manager saat quiz aktif
        if (quizManager != null &&
            quizManager.EventActive)
        {
            rightSpawner.canSpawn = false;
            topSpawner.canSpawn = false;
            bottomSpawner.canSpawn = false;

            return;
        }

        timer += Time.deltaTime;

        if (timer >= switchTime)
        {
            timer = 0f;

            currentSpawner++;

            if (currentSpawner > 2)
                currentSpawner = 0;

            ActivateSpawner(currentSpawner);
        }
    }

    private void ActivateSpawner(int index)
    {
        // Matikan semua spawner
        rightSpawner.canSpawn = false;
        topSpawner.canSpawn = false;
        bottomSpawner.canSpawn = false;

        switch (index)
        {
            // Right Spawner
            case 0:

                rightSpawner.canSpawn = true;
                rightSpawner.ForceSpawn();

                break;

            // Top Spawner
            case 1:

                topSpawner.canSpawn = true;
                topSpawner.ForceSpawn();

                break;

            // Bottom Spawner
            case 2:

                bottomSpawner.canSpawn = true;
                bottomSpawner.ForceSpawn();

                break;
        }
    }
}