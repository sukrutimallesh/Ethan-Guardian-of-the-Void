using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script constrains the player's movement within the screen boundaries, 
// with additional padding if specified, and provides movement based on player input.

public class Boundaries : MonoBehaviour
{
    private Vector2 screenBounds;  // Stores the calculated screen boundaries in world coordinates
    public float widthPadding = 2.0f;   // Extra horizontal padding beyond screen edges, allowing movement slightly off-screen
    public float heightPadding = 2.0f;  // Extra vertical padding beyond screen edges, allowing movement slightly off-screen
    public float moveSpeed = 5.0f;      // Speed at which the player object moves across the screen

    // Start is called before the first frame update
    void Start()
    {
        // Calculate the screen bounds in world coordinates by converting screen dimensions to world dimensions
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // Apply the width and height padding to the calculated screen bounds
        screenBounds.x += widthPadding;
        screenBounds.y += heightPadding;
    }

    // Update is called once per frame
    void Update()
    {
        // Capture horizontal and vertical movement input from the player (e.g., WASD keys or arrow keys)
        float moveX = Input.GetAxis("Horizontal");  // Gets a value between -1 and 1 for horizontal input
        float moveY = Input.GetAxis("Vertical");    // Gets a value between -1 and 1 for vertical input

        // Combine the movement input into a vector, scaling it by moveSpeed and frame time (Time.deltaTime) for smooth movement
        Vector3 move = new Vector3(moveX, moveY, 0) * moveSpeed * Time.deltaTime;

        // Apply the movement vector to the player object's position
        transform.position += move;

        // Clamp the player's position within the screen bounds to prevent moving off-screen
        Vector3 viewPos = transform.position;

        // Limit the x position within the left and right boundaries defined by screenBounds.x
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1, screenBounds.x);

        // Limit the y position within the top and bottom boundaries defined by screenBounds.y
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1, screenBounds.y);

        // Apply the clamped position back to the player object
        transform.position = viewPos;
    }
}
