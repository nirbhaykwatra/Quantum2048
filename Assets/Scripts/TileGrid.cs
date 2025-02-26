using UnityEngine;

/*
 * TileGrid Class
 * ---------------
 * Manages the overall grid structure, providing access to rows and cells.
 * Attach this script to a GameObject representing the entire grid in Unity.
 */
public class TileGrid : MonoBehaviour
{
    // Public properties providing access to the grid's rows and individual cells.
    // Array containing all TileRow objects in the grid.
    public TileRow[] rows { get; private set; }
    // Flat array holding all TileCell objects in the grid.
    public TileCell[] cells { get; private set; }

    // Computed properties providing useful grid statistics.
    // Total number of cells in the grid.
    public int Size => cells.Length;
    // Number of rows in the grid.
    public int Height => rows.Length;
    // Number of columns, calculated using the total size and height.
    public int Width => Size / Height;

    // Called when the script instance is being loaded.
    // Initializes the rows and cells by finding them in the grid's child GameObjects.
    private void Awake()
    {
        // Retrieve all TileRow objects from child objects.
        rows = GetComponentsInChildren<TileRow>();
        // Retrieve all TileCell objects from child objects.
        cells = GetComponentsInChildren<TileCell>();

        // Assign coordinates to each cell based on its index within the flat cells array.
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].coordinates = new Vector2Int(i % Width, i / Width);
        }
    }

    // Retrieves a TileCell given its coordinates as a Vector2Int.
    public TileCell GetCell(Vector2Int coordinates)
    {
        // Delegate to the overload taking separate x and y values.
        return GetCell(coordinates.x, coordinates.y);
    }

    // Retrieves a TileCell given its x and y coordinates.
    // Returns null if the coordinates are out of bounds.
    public TileCell GetCell(int x, int y)
    {
        // Ensure the coordinates are within the valid range for the grid.
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            // Retrieve the cell at the given position in the rows array.
            return rows[y].cells[x];
        }
        else
        {
            // Return null if the coordinates are out of bounds.
            return null;
        }
    }

    // Finds the adjacent cell to a given cell in a specified direction.
    // Returns the adjacent cell or null if the coordinates go out of bounds.
    public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
    {
        // Calculate the new coordinates by adding the direction to the current cell's coordinates.
        Vector2Int coordinates = cell.coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y; // Negative direction for y-axis since Unity's grid starts from the top.

        // Retrieve the cell at the calculated coordinates.
        return GetCell(coordinates);
    }

    // Finds a random empty cell in the grid.
    // Uses a starting index and wraps around to ensure all cells are checked.
    // Returns null if no empty cells are available.
    public TileCell GetRandomEmptyCell()
    {
        // Start at a random index in the cells array.
        int index = Random.Range(0, cells.Length);
        // Keep track of the starting index to detect a complete loop.
        int startingIndex = index;

        // Loop until an empty cell is found or all cells are checked.
        while (cells[index].Occupied)
        {
            index++; // Increment the index to check the next cell.

            if (index >= cells.Length)
            {
                index = 0; // Wrap around if the index exceeds the array length.
            }

            // If the loop returns to the starting index, all cells are occupied.
            if (index == startingIndex)
            {
                return null; // Return null to indicate no empty cells are available.
            }
        }

        // Return the empty cell found.
        return cells[index];
    }
}