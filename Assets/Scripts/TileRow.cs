// Import the core Unity namespace to access MonoBehaviour and other Unity classes
using UnityEngine;

// The TileRow class represents a row in the tile grid.
// This script should be attached to GameObjects that act as rows containing multiple TileCells.
public class TileRow : MonoBehaviour
{
    // Public property providing access to the array of TileCell objects in the row
    public TileCell[] cells { get; private set; } // Array of TileCells that this row contains

    // Awake is called when the script instance is being loaded.
    // Initializes the cells array by finding all TileCell components among the child objects.
    private void Awake()
    {
        // Retrieve all TileCell objects within this row's child objects
        cells = GetComponentsInChildren<TileCell>();
    }
}

/*
Explanation:
------------
Awake: Initializes the cells property by finding all TileCell components among the child GameObjects, which represent individual tiles within this row.
*/