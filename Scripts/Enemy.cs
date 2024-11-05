using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class defines enemy behavior, including chasing, attacking, shooting, and launching kryptonite
public class Enemy : MonoBehaviour
{
    public float chaseSpeed = 5f;             // Speed at which the enemy will chase the player
    public float retrieveDistance = 3f;       // Distance within which the enemy will start chasing the player
    public Transform player;                  // Reference to the player’s transform
    public float attackRadius = 1f;           // Radius within which the enemy can attack the player
    public LayerMask targetLayer;             // LayerMask to detect the player as a target

    public GameObject enemyBulletPrefab;      // Prefab for the enemy's bullet
    public GameObject kryptonitePrefab;       // Prefab for kryptonite, a special weapon
    public Transform shootPoint;              // The point from which the enemy will shoot
    public float startTime = 2f;              // Initial delay between shots
    private float timeBetweenShoot;           // Time remaining until the next shot
    public float velocity = 10f;              // Speed of the projectile
    public int maxHealth = 2;                 // Maximum health of the enemy
    public AudioClip shootingSound;           // Sound to play when the enemy shoots
    private AudioSource audioSource;          // Reference to the AudioSource component for sound playback
    private bool hasLaunchedKryptonite = false; // Ensures kryptonite is launched only once

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the time until the next shot
        timeBetweenShoot = startTime;

        // Get or add an AudioSource component to play shooting sounds
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = shootingSound;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the enemy's health is zero or less, and trigger death if true
        if (maxHealth <= 0)
        {
            Die();
        }

        // If the game is not active, stop further updates
        if (FindObjectOfType<GameManager>().isGameActive == false)
        {
            return;
        }

        // Find the player by tag and store their transform
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Calculate the direction from the enemy to the player
        Vector2 direction = player.position - transform.position;

        // Orient the enemy to face the player by adjusting the "up" vector
        transform.up = direction;

        // Check if the player is within the enemy's attack radius
        Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRadius, targetLayer);
        if (hit)
        {
            // Cast Player script to interact with player properties
            Player playerScript = player.GetComponent<Player>();

            // If player kill count is greater than 10 and kryptonite hasn't been launched yet, launch it
            if (playerScript != null && playerScript.GetKillCount() > 10 && !hasLaunchedKryptonite)
            {
                LaunchKryptonite();  // Launch kryptonite when player meets conditions
                hasLaunchedKryptonite = true; // Set flag to prevent multiple launches
            }
            // Otherwise, handle shooting logic
            else if (timeBetweenShoot <= 0f)
            {
                Shoot();  // Shoot at the player
                timeBetweenShoot = startTime;  // Reset the time between shots
            }
            else
            {
                // Decrease time between shots by the elapsed frame time
                timeBetweenShoot -= Time.deltaTime;
            }

            // If the player is outside the retrieve distance, move toward the player
            if (Vector2.Distance(transform.position, player.position) > retrieveDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            }
        }
    }

    // Method to instantiate and launch a bullet from the shootPoint
    void Shoot()
    {
        // Instantiate a bullet prefab at the shoot point with no rotation
        GameObject tempBullet = Instantiate(enemyBulletPrefab, shootPoint.position, Quaternion.identity);

        // Set the bullet's velocity in the direction the enemy is facing
        tempBullet.GetComponent<Rigidbody2D>().velocity = shootPoint.up * velocity;

        // Destroy the bullet after 5 seconds to prevent memory leaks
        Destroy(tempBullet, 5f);

        // Play the shooting sound if an AudioSource is available
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    // Visualize the attack radius in the editor for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    // Method to launch a kryptonite projectile at the player
    void LaunchKryptonite()
    {
        // Instantiate a kryptonite prefab at the shoot point with no rotation
        GameObject kryptonite = Instantiate(kryptonitePrefab, shootPoint.position, Quaternion.identity);

        // Set the kryptonite's velocity in the direction the enemy is facing
        kryptonite.GetComponent<Rigidbody2D>().velocity = shootPoint.up * velocity;

        // Destroy the kryptonite after 5 seconds to prevent memory leaks
        Destroy(kryptonite, 5f);

        // Play the shooting sound if an AudioSource is available
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    // Reduces the enemy's health by 1
    void TakeDamage()
    {
        // Only reduce health if the enemy has positive health remaining
        if (maxHealth <= 0)
        {
            return;
        }
        maxHealth -= 1;
    }

    // Handles the enemy's death and notifies the player to increase their kill count
    void Die()
    {
        // Find the player and increase their kill count if they exist
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.IncrementKillCount();
        }

        // Destroy the enemy GameObject
        Destroy(this.gameObject);
    }

    // Check for collisions with other objects, specifically player bullets
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If hit by a player's bullet, take damage and destroy the bullet
        if (collision.gameObject.tag == "PlayerBullet")
        {
            TakeDamage();
            Destroy(collision.gameObject);
        }
    }
}
