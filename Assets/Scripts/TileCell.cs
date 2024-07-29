// Import the core Unity namespace to access MonoBehaviour and other Unity classes
using UnityEngine;

// The TileCell class represents an individual cell in the grid.
// This script should be attached to each cell GameObject within the grid.
public class TileCell : MonoBehaviour
{
    // Public properties that represent the cell's coordinates and the tile currently occupying it
    public Vector2Int coordinates { get; set; } // Holds the X and Y coordinates of the cell within the grid
    public Tile tile { get; set; }              // Reference to the tile currently occupying the cell

    // Property to check if the cell is empty (returns true if no tile is currently in the cell)
    public bool Empty => tile == null;

    // Property to check if the cell is occupied (returns true if a tile is present in the cell)
    public bool Occupied => tile != null;
}

/*
Explanation:
------------
coordinates: Stores the cell's coordinates as a Vector2Int object for easy reference to grid positions.
tile: Holds a reference to a tile object if the cell is occupied. If null, the cell is empty.
Empty and Occupied are convenience properties that provide easy checks on the cell's status.
*/