using UnityEngine;

// The Obstacle class represents an obstacle in the game that can change its state to 
// 'dangerous' at set intervals and affect the player when in danger mode.
public class Obstacle : MonoBehaviour
{
    public Color defaultColor = Color.white;  // The color of the obstacle when it is not dangerous
    public Color dangerColor = Color.red;      // The color of the obstacle when it is in danger mode
    public float dangerInterval = 10f;         // Time interval between danger mode activations
    public float dangerDuration = 2f;          // Duration of the danger mode when activated
    private bool isDangerous = false;           // Tracks whether the obstacle is currently dangerous
    private SpriteRenderer spriteRenderer;      // Reference to the SpriteRenderer component for color changes

    // Start is called before the first frame update
    void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Set the initial color of the obstacle to the default color
        spriteRenderer.color = defaultColor;

        // Start the repeated invocation of ActivateDangerMode method based on the dangerInterval
        InvokeRepeating(nameof(ActivateDangerMode), dangerInterval, dangerInterval);
    }

    // Activates danger mode, changing the obstacle's color and setting its dangerous state
    void ActivateDangerMode()
    {
        isDangerous = true;                          // Set the dangerous state to true
        spriteRenderer.color = dangerColor;          // Change the color of the obstacle to dangerColor
        // Schedule the DeactivateDangerMode method to be called after dangerDuration
        Invoke(nameof(DeactivateDangerMode), dangerDuration);
    }

    // Deactivates danger mode, reverting the obstacle's color and state
    void DeactivateDangerMode()
    {
        isDangerous = false;                         // Set the dangerous state to false
        spriteRenderer.color = defaultColor;        // Revert the color of the obstacle to defaultColor
    }

    // Called when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is tagged as "PlayerBullet"
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            // Destroy the player bullet upon collision with the obstacle
            Destroy(collision.gameObject);
        }
    }

    // Called when this GameObject collides with another collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the obstacle is in danger mode and if the colliding object is tagged as "Player"
        if (isDangerous && collision.gameObject.CompareTag("Player"))
        {
            // Get the Player component from the colliding GameObject
            Player player = collision.gameObject.GetComponent<Player>();
            // Check if the Player component exists
            if (player != null)
            {
                // Call the TakeDamage method on the player to reduce health by 1 point
                player.TakeDamage(1);
            }
        }
    }
}
