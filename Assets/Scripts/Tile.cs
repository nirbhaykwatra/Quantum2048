// Import necessary namespaces for Unity and TextMeshPro functionality
using System.Collections; // Enables using IEnumerator for coroutines
using TMPro;             // For text rendering with TextMeshPro
using UnityEngine;        // Main Unity namespace
using UnityEngine.UI;     // For UI components like Image

// The Tile class represents a single tile in the game grid.
// This script should be attached to individual Tile objects in Unity.
public class Tile : MonoBehaviour
{
    // Encapsulated properties to access the state and cell associated with this tile
    public TileState state { get; private set; } // Stores the current state of the tile, such as color and number
    public TileCell cell { get; private set; }   // Keeps track of the cell location where the tile is placed
    public bool locked { get; set; }             // Indicates whether the tile is locked and cannot be moved

    // Private fields for UI elements of the tile
    private Image background;            // The background image of the tile
    private TextMeshProUGUI text;        // Text component for displaying numbers

    // Awake is called when the script instance is being loaded.
    // Initialize the background and text components by finding them within the Tile's GameObject.
    private void Awake()
    {
        background = GetComponent<Image>();                        // Get the Image component attached to the tile
        text = GetComponentInChildren<TextMeshProUGUI>();          // Find the TextMeshProUGUI component within child objects
    }

    // Method to set the state of the tile, updating its appearance based on the given TileState.
    public void SetState(TileState state)
    {
        this.state = state;                      // Set the internal state

        background.color = state.backgroundColor; // Apply the background color from the TileState
        text.color = state.textColor;             // Apply the text color from the TileState
        text.text = state.number.ToString();      // Update the displayed number from the TileState
    }

    // Method to spawn a tile at a given cell.
    // Sets up the relationship between the tile and the cell and moves the tile's position to the cell's position.
    public void Spawn(TileCell cell)
    {
        if (this.cell != null) {
            this.cell.tile = null; // Unlink the tile from the current cell
        }

        this.cell = cell;          // Assign the new cell to the tile
        this.cell.tile = this;     // Point the cell's tile to this tile

        // Set the position of the tile to align with the cell's position in the grid
        transform.position = cell.transform.position;
    }

    // Method to move the tile to a new cell with a smooth animation.
    // Updates cell references and triggers the animation coroutine.
    public void MoveTo(TileCell cell)
    {
        if (this.cell != null) {
            this.cell.tile = null; // Unlink the tile from the current cell
        }

        this.cell = cell;          // Assign the new cell to the tile
        this.cell.tile = this;     // Point the cell's tile to this tile

        // Start the animation coroutine to move the tile to the new cell's position
        StartCoroutine(Animate(cell.transform.position, false));
    }

    // Method to merge the tile with another tile located in a different cell.
    // The original tile is removed upon completion of the merging animation.
    public void Merge(TileCell cell)
    {
        if (this.cell != null) {
            this.cell.tile = null; // Unlink the tile from the current cell
        }

        this.cell = null;          // Clear this tile's cell reference
        cell.tile.locked = true;   // Lock the target cell to prevent further movement

        // Start the animation coroutine to move the tile to the merging cell and then remove it
        StartCoroutine(Animate(cell.transform.position, true));
    }

    // Coroutine to animate the movement of the tile to a target position.
    // Takes the target position as 'to' and whether the tile will be destroyed after merging as 'merging'.
    private IEnumerator Animate(Vector3 to, bool merging)
    {
        float elapsed = 0f;       // Time elapsed since the animation started
        float duration = 0.1f;    // Total duration of the animation

        Vector3 from = transform.position; // Starting position of the tile

        // Lerp (linearly interpolate) between the start and end positions over the animation duration
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime; // Increment elapsed time by the time since the last frame
            yield return null;         // Wait until the next frame before continuing the loop
        }

        // Ensure the tile reaches its target position
        transform.position = to;

        // If the tile is merging, destroy it after reaching the final position
        if (merging) {
            Destroy(gameObject);
        }
    }

}