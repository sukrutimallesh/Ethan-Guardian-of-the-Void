using UnityEngine;

// This class allows the GameObject to smoothly follow the player object in the scene
public class Follow : MonoBehaviour
{
    public Transform player;           // Reference to the player's Transform component to follow
    public float smoothSpeed = 2f;     // Speed at which the following object will move towards the player

    // Update is called once per frame
    void Update()
    {
        // Currently, this method is empty. It can be used for handling updates that don't require physics,
        // such as user inputs or UI updates, but in this script, it doesn't perform any actions.
    }

    // FixedUpdate is called at a consistent interval and is used for physics calculations
    private void FixedUpdate()
    {
        // Smoothly interpolates the position of this GameObject towards the player's position
        // using spherical linear interpolation (Slerp) for a smooth transition.
        // transform.position: The current position of the GameObject this script is attached to
        // player.position: The target position (the player's position) to follow
        // smoothSpeed * Time.fixedDeltaTime: Scales the smoothSpeed by the fixed time step,
        // ensuring consistent movement speed regardless of frame rate
        transform.position = Vector3.Slerp(transform.position, player.position, smoothSpeed * Time.fixedDeltaTime);
    }
}
