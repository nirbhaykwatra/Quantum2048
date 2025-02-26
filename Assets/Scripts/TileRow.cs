using UnityEngine;

/*
 * TileRow Class
 * -------------
 * Represents a row in the tile grid.
 * Attach this script to GameObjects that act as rows containing multiple TileCells.
 */
public class TileRow : MonoBehaviour
{
    // Public property providing access to the array of TileCell objects in the row.
    public TileCell[] cells { get; private set; } // Array of TileCells that this row contains.

    // Called when the script instance is being loaded.
    // Initializes the cells array by finding all TileCell components among the child objects.
    private void Awake()
    {
        // Retrieve all TileCell objects within this row's child objects.
        cells = GetComponentsInChildren<TileCell>();
    }
}