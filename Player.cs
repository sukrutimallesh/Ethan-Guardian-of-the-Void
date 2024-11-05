using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    // UI elements
    public Text maxHealthTxt; // Text UI element to display the player's maximum health
    public Text killCountText; // Text UI element to display the number of enemies killed
    public Button specialPowerButton; // Button for activating the player's special power

    // Player movement and speed variables
    public float speedVelocity = 10f; // Velocity at which the bullet moves
    public float baseSpeed = 10f; // Base speed of the player
    private float currentSpeed; // Current speed of the player, which can change based on health
    private float xMovement; // Horizontal movement input
    private float yMovement; // Vertical movement input

    // Bullet shooting variables
    public GameObject bulletPrefab; // Prefab of the bullet to be instantiated
    public Transform shootPoint; // Point from where the bullet will be instantiated

    // Health variables
    public int maxHealth = 10; // Maximum health of the player
    private int initialMaxHealth; // Store the initial maximum health for calculations

    // Camera and audio variables
    public Animator camAnimator; // Animator component for camera shake effect on damage
    public AudioClip shootingSound; // Audio clip to play when the player shoots
    private AudioSource audioSource; // AudioSource component for playing sounds

    // Class for background music options
    [System.Serializable]
    public class BackgroundMusic
    {
        public string trackName; // Name of the track to display in the UI
        public AudioClip music; // The actual audio clip
        public bool isDefault; // Indicates if this track is the default one to play
    }

    // Array of background music options
    public BackgroundMusic[] backgroundTracks;
    private AudioSource backgroundSource; // AudioSource component for background music
    private int currentTrackIndex = 0; // Index of the currently playing background track
    private int enemyKillCount = 0; // Count of enemies killed by the player

    // UI element for music selection dropdown
    public TMP_Dropdown musicDropdown; // Dropdown UI for selecting background music

    // Start is called before the first frame update
    void Start()
    {
        initialMaxHealth = maxHealth; // Store the initial max health for later calculations
        UpdateSpeed(); // Set the initial speed based on max health

        // Initialize the shooting sound AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = shootingSound; // Set the shooting sound clip

        // Setup background music and music selection dropdown
        SetupBackgroundMusic();
        SetupMusicDropdown();
    }

    // Setup background music options
    void SetupBackgroundMusic()
    {
        // Create and configure the AudioSource for background music
        backgroundSource = gameObject.AddComponent<AudioSource>();
        backgroundSource.loop = true; // Set to loop the music
        backgroundSource.volume = 0.3f; // Set the volume level
        backgroundSource.playOnAwake = true; // Start playing immediately if there is a track

        // Find and play the default track, if one is set
        for (int i = 0; i < backgroundTracks.Length; i++)
        {
            if (backgroundTracks[i].isDefault)
            {
                currentTrackIndex = i; // Store the index of the default track
                break; // Exit the loop once found
            }
        }

        // If no default is set, play the first track
        if (backgroundTracks.Length > 0)
        {
            PlayBackgroundTrack(currentTrackIndex);
        }
    }

    // Setup the music selection dropdown UI
    void SetupMusicDropdown()
    {
        if (musicDropdown != null) // Ensure the dropdown is assigned
        {
            musicDropdown.ClearOptions(); // Clear any existing options

            // Create a list of track names for the dropdown
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
            foreach (var track in backgroundTracks)
            {
                options.Add(new TMP_Dropdown.OptionData(track.trackName)); // Add track names as options
            }

            musicDropdown.AddOptions(options); // Add options to the dropdown
            musicDropdown.value = currentTrackIndex; // Set the current track as the selected option
            musicDropdown.onValueChanged.AddListener(OnMusicSelectionChanged); // Register event listener
        }
    }

    // Event triggered when a new music track is selected from the dropdown
    public void OnMusicSelectionChanged(int index)
    {
        PlayBackgroundTrack(index); // Play the selected track
    }

    // Play the selected background track
    void PlayBackgroundTrack(int index)
    {
        if (index >= 0 && index < backgroundTracks.Length) // Ensure the index is valid
        {
            currentTrackIndex = index; // Update the current track index
            backgroundSource.clip = backgroundTracks[index].music; // Set the audio clip to the selected track
            backgroundSource.Play(); // Play the selected track
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (maxHealth <= 0) // Check if the player is dead
        {
            Die(); // Trigger death sequence
        }

        maxHealthTxt.text = maxHealth.ToString(); // Update health display

        // Get player input for movement
        xMovement = Input.GetAxis("Horizontal");
        yMovement = Input.GetAxis("Vertical");

        // Calculate the direction to aim towards the mouse position
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - (Vector2)transform.position; // Calculate the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate the angle in degrees
        transform.rotation = Quaternion.AngleAxis(angle, transform.forward); // Rotate player to face the mouse

        // Check if the left mouse button is pressed to shoot
        if (Input.GetMouseButtonDown(0))
        {
            Shoot(); // Call shoot method
        }
    }

    // FixedUpdate is called on a fixed time interval, useful for physics updates
    private void FixedUpdate()
    {
        // Move the player based on input and speed
        transform.position += new Vector3(xMovement, yMovement, 0f).normalized * Time.fixedDeltaTime * currentSpeed;
    }

    // New method to update speed based on health
    private void UpdateSpeed()
    {
        // Calculate speed multiplier based on the player's current health percentage
        float healthPercentage = (float)maxHealth / initialMaxHealth;

        // Speed reduces linearly with health
        // At full health: 100% of base speed
        // At 0 health: 40% of base speed
        float minSpeedMultiplier = 0.4f; // Minimum speed is 40% of base speed
        float speedMultiplier = Mathf.Lerp(minSpeedMultiplier, 1f, healthPercentage); // Calculate speed multiplier

        currentSpeed = baseSpeed * speedMultiplier; // Update current speed based on health

        // Optional: Debug log to see speed changes for troubleshooting
        Debug.Log($"Health: {maxHealth}/{initialMaxHealth}, Speed Multiplier: {speedMultiplier:F2}, Current Speed: {currentSpeed:F2}");
    }

    // Method to handle shooting bullets
    void Shoot()
    {
        // Instantiate a bullet at the shoot point with the correct rotation
        GameObject temp = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        temp.GetComponent<Rigidbody2D>().velocity = shootPoint.right * speedVelocity; // Set the bullet's velocity
        Destroy(temp, 10f); // Destroy the bullet after 10 seconds to prevent memory leaks

        // Play shooting sound if audioSource is initialized
        if (audioSource != null)
        {
            audioSource.Play(); // Play the shooting sound
        }
    }

    // Method to handle taking damage
    public void TakeDamage(int damage)
    {
        if (maxHealth <= 0) // Prevent taking damage if already dead
        {
            return;
        }
        maxHealth -= damage; // Reduce max health by the damage taken
        UpdateSpeed(); // Update speed based on new health value
        camAnimator.SetTrigger("Shake"); // Trigger camera shake effect on damage
    }

    // Method to handle collision detection
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check for collision with enemy bullets
        if (other.gameObject.tag == "EnemyBullet")
        {
            TakeDamage(1); // Take 1 point of damage
            Destroy(other.gameObject); // Destroy the enemy bullet
        }
        // Check for collision with kryptonite
        else if (other.gameObject.tag == "Kryptonite")
        {
            maxHealth = 0; // Set health to 0, triggering death
            Die(); // Call die method
        }
    }

    // Method to handle player death
    void Die()
    {
        Debug.Log(this.transform.name + " Died."); // Log death message
        FindObjectOfType<GameManager>().isGameActive = false; // Set game state to inactive
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
        Destroy(this.gameObject); // Destroy the player object
    }

    // Method to increment the kill count when an enemy dies
    public void IncrementKillCount()
    {
        enemyKillCount++; // Increase the kill count
        killCountText.text = enemyKillCount.ToString(); // Update the UI to display the new kill count
    }
    // Method to retrieve the current kill count
    public int GetKillCount()
    {
        return enemyKillCount; // Return the total number of enemies killed by the player
    }

}

