using Unity.AI;
using Unity.Multiplayer.PlayMode;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    private float minDistance = 10f;
    private float maxDistance = 15f;
    GameObject player;
    public GameObject[] enemyPrefabs; 

    public int maxEnemies = 8;
    private int currentEnemies = 0;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 2f, 5f);
        
    }

    private Vector3 GenerateEnemy(Vector3 playerPos)
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized * 
            Random.Range(minDistance, maxDistance);

        Vector3 spawnPos = playerPos + new Vector3(randomCircle.x, 0, randomCircle.y);
        return spawnPos;
    }

    private bool OnNavMesh(Vector3 playerPos, out Vector3 result)
    {
        for (int i = 0; i < 10; i++) // 여러 번 시도
        {
            Vector3 randomPos = GenerateEnemy(playerPos);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, 2.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }

    void SpawnEnemy()
    {
        if (player == null) return;
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;
        if (currentEnemies >= maxEnemies) return;

        Vector3 spawnPos;

        if (OnNavMesh(player.transform.position, out spawnPos))
        {
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

            // Enemy 스크립트 연결
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.spawner = this;
            }

            currentEnemies++;
        }
    }

    public void OnEnemyKilled()
    {
        currentEnemies = Mathf.Max(0, currentEnemies - 1);
    }
}
