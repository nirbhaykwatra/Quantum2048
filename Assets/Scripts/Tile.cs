using System.Collections;
using System.Collections.Generic;
using GameEvents; // Enables use of IEnumerator for coroutines
using TMPro;                // For text rendering with TextMeshPro
using UnityEngine;          // Main Unity namespace
using UnityEngine.UI;       // For UI components like Image
using Sirenix.OdinInspector;

/*
 * Tile Class
 * ----------
 * Represents a single tile in the game grid.
 * Attach this script to individual Tile objects in Unity.
 */
public class Tile : MonoBehaviour
{
    // Encapsulated properties to access the state and cell associated with this tile.
    // Stores the current state of the tile, such as color and number.
    public TileState state { get; private set; }
    // Keeps track of the cell location where the tile is placed.
    public TileCell cell { get; private set; }
    // Indicates whether the tile is locked and cannot be moved.
    public bool locked { get; set; }
    
    [HideInInspector] public Tile _entangledTile;
    private TileState[] _tileStates;
    
    private Animator _animator;

    public Animator Animator => _animator;

    // Private fields for UI elements of the tile.
    // The background image of the tile.
    [HideInInspector] public Image background;
    [HideInInspector] public Image superpositionOverlay;
    [HideInInspector] public Image entanglementOverlay;
    // Text component for displaying numbers.
    private TextMeshProUGUI text;
    public bool entangleMode { get; set; }
    
    [TitleGroup("Properties")]
    [ShowInInspector][ReadOnly] public int TileID { get; private set; }
    
    [TitleGroup("Properties")]
    [ShowInInspector][ReadOnly] public bool Superposition { get; set; }
    
    [TitleGroup("Properties")]
    [ShowInInspector][ReadOnly] public bool Entangled { get; set; }
    
    [TitleGroup("Events")]
    [ShowInInspector] public TileEventAsset entangleEvent;
    [TitleGroup("Events")]
    [ShowInInspector] public TileEventAsset disentangleEvent;
    // Called when the script instance is being loaded.
    // Initializes the background and text components by finding them within the Tile's GameObject.
    private void Awake()
    {
        TileID = Random.Range(0, 9999);
        background = GetComponent<Image>();                       // Get the Image component attached to the tile.
        _animator = GetComponent<Animator>();
        foreach (Image image in GetComponentsInChildren<Image>())
        {
            if (image.gameObject.name == "Superposition")
            {
                superpositionOverlay = image;
                superpositionOverlay.enabled = false;
            }
            else if (image.gameObject.name == "Entangled")
            {
                entanglementOverlay = image;
                entanglementOverlay.enabled = false;
            }
        }
        text = GetComponentInChildren<TextMeshProUGUI>();           // Find the TextMeshProUGUI component within child objects.
        _tileStates = FindAnyObjectByType<TileBoard>().tileStates;
    }

    private void Update()
    {
        superpositionOverlay.enabled = Superposition;
    }

    // Sets the state of the tile, updating its appearance based on the given TileState.
    public void SetState(TileState state)
    {
        this.state = state;                        // Set the internal state.
        background.color = state.backgroundColor;  // Apply the background color from the TileState.
        //if (Superposition) background.color = state.superpositionColor;
        text.color = state.textColor;              // Apply the text color from the TileState.
        text.text = state.number.ToString();       // Update the displayed number from the TileState.
    }

    // Spawns a tile at a given cell.
    // Sets up the relationship between the tile and the cell and moves the tile's position to the cell's position.
    public void Spawn(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null; // Unlink the tile from the current cell.
        }

        this.cell = cell;         // Assign the new cell to the tile.
        this.cell.tile = this;    // Point the cell's tile to this tile.

        // Set the position of the tile to align with the cell's position in the grid.
        transform.position = cell.transform.position;
        
        if (Superposition)
        {
            SetState(_tileStates[Random.Range(0, 4)]);
        }
        else
        {
            SetState(_tileStates[0]);
        }
    }

    // Moves the tile to a new cell with a smooth animation.
    // Updates cell references and triggers the animation coroutine.
    public void MoveTo(TileCell cell)
    {
        if (_entangledTile)
        {
            SetState(_entangledTile.state);
        }
        if (Superposition && !_entangledTile)
        {
            SetState(_tileStates[Random.Range(0, 4)]);
        }
        if (this.cell != null)
        {
            this.cell.tile = null; // Unlink the tile from the current cell.
        }

        this.cell = cell;         // Assign the new cell to the tile.
        this.cell.tile = this;    // Point the cell's tile to this tile.

        // Start the animation coroutine to move the tile to the new cell's position.
        StartCoroutine(Animate(cell.transform.position, false, false));
        
    }

    // Merges the tile with another tile located in a different cell.
    // The original tile is removed upon completion of the merging animation.
    // Takes in a bool to detect if tunneling has occurred.
    public void Merge(TileCell cell, bool tunneling)
    {
        Disentangle();
        Superposition = false;
        if (this.cell != null)
        {
            this.cell.tile = null; // Unlink the tile from the current cell.
        }

        this.cell = null;          // Clear this tile's cell reference.
        cell.tile.locked = true;   // Lock the target cell to prevent further movement.

        // Start the animation coroutine to move the tile to the merging cell and then remove it.
        StartCoroutine(Animate(cell.transform.position, true, tunneling));
    }

    // Animates the movement of the tile to a target position.
    // 'to' is the target position, 'merging' indicates if the tile will be destroyed after merging,
    // and 'tunnel' detects if tunneling has occurred.
    private IEnumerator Animate(Vector3 to, bool merging, bool tunnel)
    {
        float elapsed = 0f;       // Time elapsed since the animation started.
        float duration = 0.1f;    // Total duration of the animation.

        Vector3 from = transform.position; // Starting position of the tile.

        // Lerp (linearly interpolate) between the start and end positions over the animation duration.
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime; // Increment elapsed time by the time since the last frame.
            yield return null;         // Wait until the next frame before continuing the loop.
        }

        // Ensure the tile reaches its target position.
        transform.position = to;

        if (tunnel)
        {
            Superposition = false;
            // Play an animation or visual effect to indicate tunneling.
            Debug.Log("Tunneling Occurred");
        }

        // If the tile is merging, destroy it after reaching the final position.
        if (merging)
        {
            Superposition = false;
            Destroy(gameObject);
        }
    }

    public void EntangleButton()
    {
        if (!entangleMode) return;
        
        if (!Entangled)
        {
            Entangle();
        }
        else
        {
            Disentangle();
        }
    }

    public void Entangle()
    {
        Entangled = true;
        entanglementOverlay.enabled = true;
        entangleEvent.Invoke(this);
    }

    public void Disentangle()
    {
        _entangledTile = null;
        Entangled = false;
        entanglementOverlay.enabled = false;
        disentangleEvent.Invoke(this);
    }

    // NOTE: Consider creating a separate function for tunneling animations in the future.
}