using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script handles spawning enemy objects at random predefined positions on the screen
// at regular intervals, based on a time delay.

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;     // Prefab of the enemy to spawn
    public Transform[] spawnPositions; // Array of possible spawn positions for the enemies
    public float startTime = 2f;       // Initial time interval between spawns
    private float timeBetweenSpawn;    // Time remaining until the next enemy spawn

    // Start is called before the first frame update
    void Start()
    {
        // Spawn the first enemy when the game starts
        SpawnEnemy();

        // Initialize the time between spawns to the defined start time
        timeBetweenSpawn = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game is active by referencing the GameManager; 
        // if not active, exit the Update function and stop spawning enemies
        if (FindObjectOfType<GameManager>().isGameActive == false)
        {
            return; // Early return prevents further code execution if the game is inactive
        }

        // Check if it's time to spawn a new enemy
        if (timeBetweenSpawn <= 0f)
        {
            // Reset the spawn timer to startTime for the next enemy spawn
            timeBetweenSpawn = startTime;

            // Spawn an enemy at one of the predefined positions
            SpawnEnemy();
        }
        else
        {
            // Decrease the time remaining until the next spawn by the elapsed frame time
            timeBetweenSpawn -= Time.deltaTime;
        }
    }

    // Method to spawn an enemy at a random position
    void SpawnEnemy()
    {
        // Choose a random index from the spawnPositions array to determine spawn location
        int randomPositionIndex = Random.Range(0, spawnPositions.Length);

        // Instantiate a new enemy at the randomly chosen position with no rotation (Quaternion.identity)
        Instantiate(enemyPrefab, spawnPositions[randomPositionIndex].position, Quaternion.identity);
    }
}
