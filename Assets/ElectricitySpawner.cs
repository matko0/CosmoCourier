using UnityEngine;
using System.Collections;

public class ElectricitySpawner : MonoBehaviour
{
    public GameObject electricityPrefab; // Reference to the electricity prefab
    public Transform shipArea; // Area where electricity can spawn
    public float spawnInterval = 10f; // Time between spawns
    public float minX, maxX, minY, maxY; // Bounds for electricity spawning

    void Start()
    {
        StartCoroutine(SpawnHoles());
    }

    IEnumerator SpawnHoles()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Calculate a random position within the defined bounds
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            Vector2 spawnPosition = new Vector2(randomX, randomY);

            // Instantiate the electricity at the random position
            Instantiate(electricityPrefab, spawnPosition, Quaternion.identity, shipArea);
        }
    }
}
