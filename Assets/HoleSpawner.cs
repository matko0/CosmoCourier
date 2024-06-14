using UnityEngine;
using System.Collections;

public class HoleSpawner : MonoBehaviour
{
    public GameObject holePrefab; // Reference to the hole prefab
    public Transform shipArea; // Area where holes can spawn
    public float spawnInterval = 10f; // Time between spawns
    public float minX, maxX, minY, maxY; // Bounds for hole spawning
    public int maxHoles = 10; // Maximum number of allowed holes

    private int currentHoleCount = 0; // Current number of spawned holes

    void Start()
    {
        StartCoroutine(SpawnHoles());
    }

    IEnumerator SpawnHoles()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Check if maximum holes limit reached
            if (currentHoleCount >= maxHoles)
            {
                Debug.Log("Game Over - Too many holes!");
                // Implement game over logic here (e.g., show game over screen, reset game, etc.)
                yield break; // Exit coroutine
            }

            // Calculate a random position within the defined bounds
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            Vector2 spawnPosition = new Vector2(randomX, randomY);

            // Instantiate the hole at the random position
            Instantiate(holePrefab, spawnPosition, Quaternion.identity, shipArea);

            // Increment the hole count
            currentHoleCount++;
        }
    }
}
