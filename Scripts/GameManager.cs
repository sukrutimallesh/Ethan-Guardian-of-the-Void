using UnityEngine;

// The GameManager class is responsible for managing the overall state of the game.
// It contains essential game state information that can be accessed throughout the game.
public class GameManager : MonoBehaviour
{
    // A boolean variable that indicates whether the game is currently active.
    // This can be used to control gameplay elements such as enemy spawning, player actions,
    // and any other gameplay mechanics that should only be active when the game is running.
    public bool isGameActive = true;
}
