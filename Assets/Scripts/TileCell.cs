using Sirenix.OdinInspector;
using UnityEngine;

/*
 * TileCell Class
 * --------------
 * Represents an individual cell in the grid.
 * Attach this script to each cell GameObject within the grid.
 */
public class TileCell : MonoBehaviour
{
    // Public properties representing the cell's coordinates and the tile currently occupying it.
    // Coordinates: Holds the X and Y positions of the cell within the grid.
    [ShowInInspector] public Vector2Int coordinates { get; set; }
    // Tile: Reference to the tile currently occupying the cell.
    public Tile tile { get; set; }

    // Convenience property that returns true if no tile is present (i.e., the cell is empty).
    public bool Empty => tile == null;

    // Convenience property that returns true if a tile is present in the cell.
    public bool Occupied => tile != null;
}