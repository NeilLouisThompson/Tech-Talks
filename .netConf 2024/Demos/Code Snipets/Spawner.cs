using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float SpawnTimout = 1f;
    public Transform EnemyPrefab;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, SpawnTimout);
    }

    void SpawnEnemy()
    {
        Instantiate(EnemyPrefab, transform.position, transform.rotation);

    }
}
