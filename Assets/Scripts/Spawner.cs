using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Obstacle")]
    [SerializeField] private GameObject[] obstaclePrefabs;

    [Header("Spawn Timing")]
    [SerializeField] private float minSpawnTime = 2f;

    [SerializeField] private float maxSpawnTime = 5f;

    [Header("Spawn Amount")]
    [SerializeField] private int minSpawnAmount = 1;

    [SerializeField] private int maxSpawnAmount = 3;

    [Header("Movement")]
    [SerializeField] private Vector2 moveDirection = Vector2.left;

    [SerializeField] private float obstacleSpeed = 5f;

    [Header("Spawn Area Type")]
    [SerializeField] private SpawnType spawnType;

    public bool canSpawn = false;

    private float currentSpawnTime;

    private float timer;

    private BoxCollider2D spawnArea;

    public enum SpawnType
    {
        Right,
        Top,
        Bottom
    }

    private void Awake()
    {
        spawnArea = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        SetRandomSpawnTime();
    }

    private void Update()
    {
        SpawnLoop();
    }

    private void SpawnLoop()
    {
        // Jika spawner OFF
        if (!canSpawn)
        {
            timer = 0f;
            return;
        }

        timer += Time.deltaTime;

        // Waktu spawn tercapai
        if (timer >= currentSpawnTime)
        {
            SpawnMultiple();

            timer = 0f;

            SetRandomSpawnTime();
        }
    }

    // Dipanggil SpawnManager saat spawner aktif
    public void ForceSpawn()
    {
        SpawnMultiple();

        timer = 0f;

        SetRandomSpawnTime();
    }

    private void SetRandomSpawnTime()
    {
        currentSpawnTime =
            Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void SpawnMultiple()
    {
        int spawnAmount =
            Random.Range(minSpawnAmount, maxSpawnAmount + 1);

        for (int i = 0; i < spawnAmount; i++)
        {
            SpawnSingle();
        }
    }

    private void SpawnSingle()
    {
        // Random obstacle
        GameObject obstacleToSpawn =
            obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        Bounds bounds = spawnArea.bounds;

        // Random posisi dalam seluruh area collider
        float randomX =
            Random.Range(bounds.min.x, bounds.max.x);

        float randomY =
            Random.Range(bounds.min.y, bounds.max.y);

        Vector2 spawnPos =
            new Vector2(randomX, randomY);

        // Spawn obstacle
        GameObject spawnedObstacle =
            Instantiate(obstacleToSpawn, spawnPos, Quaternion.identity);

        // Gerakkan obstacle
        Rigidbody2D rb =
            spawnedObstacle.GetComponent<Rigidbody2D>();

        rb.linearVelocity =
            moveDirection.normalized * obstacleSpeed;

        // Destroy obstacle
        Destroy(spawnedObstacle, 5f);
    }

    // Visual area spawn
    private void OnDrawGizmos()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();

        if (box == null) return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(
            box.bounds.center,
            box.bounds.size
        );
    }
}