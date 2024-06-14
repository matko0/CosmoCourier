using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HoleSpawner : MonoBehaviour
{
    public GameObject holePrefab; // Reference to the hole prefab
    public Transform shipArea; // Area where holes can spawn
    public float spawnInterval = 10f; // Time between spawns
    public float minX, maxX, minY, maxY; // Bounds for hole spawning
    public int maxHoles = 10; // Maximum allowed holes

    private int currentHoles = 0; // Current number of holes

    void Start()
    {
        StartCoroutine(SpawnHoles());
    }

    IEnumerator SpawnHoles()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (currentHoles == maxHoles)
            {
                SceneManager.LoadScene("gameOver"); // Replace "GameOverScene" with the name of your Game Over scene
                yield break;
            }

            // Calculate a random position within the defined bounds
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            Vector2 spawnPosition = new Vector2(randomX, randomY);

            // Instantiate the hole at the random position and increment the hole count
            Instantiate(holePrefab, spawnPosition, Quaternion.identity, shipArea);
            currentHoles++;
        }
    }

    // Method to decrease the hole count when a hole is repaired
    public void RepairHole()
    {
        if (currentHoles > 0)
        {
            currentHoles--;
        }
    }
}
