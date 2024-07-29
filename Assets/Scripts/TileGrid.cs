// Import the main Unity namespace to access MonoBehaviour and other core features
using UnityEngine;

// The TileGrid class manages the overall grid structure, providing access to rows and cells.
// Attach this script to a GameObject representing the entire grid in Unity.
public class TileGrid : MonoBehaviour
{
    // Public properties providing access to the grid's rows and individual cells
    public TileRow[] rows { get; private set; }   // Array containing all TileRow objects in the grid
    public TileCell[] cells { get; private set; } // Flat array holding all TileCell objects in the grid

    // Computed properties providing useful grid statistics
    public int Size => cells.Length;              // Total number of cells in the grid
    public int Height => rows.Length;             // Number of rows in the grid
    public int Width => Size / Height;            // Number of columns, calculated using the total size and height

    // Awake is called when the script instance is being loaded.
    // Initializes the rows and cells by finding them in the grid's child GameObjects.
    private void Awake()
    {
        rows = GetComponentsInChildren<TileRow>();  // Retrieve all TileRow objects from child objects
        cells = GetComponentsInChildren<TileCell>(); // Retrieve all TileCell objects from child objects

        // Assign coordinates to each cell based on their index within the flat cells array
        for (int i = 0; i < cells.Length; i++) {
            cells[i].coordinates = new Vector2Int(i % Width, i / Width);
        }
    }

    // Method to retrieve a TileCell given its coordinates as a Vector2Int object.
    public TileCell GetCell(Vector2Int coordinates)
    {
        return GetCell(coordinates.x, coordinates.y); // Delegate to the overload taking separate x and y values
    }

    // Method to retrieve a TileCell given its x and y coordinates.
    // Returns null if the coordinates are out of bounds.
    public TileCell GetCell(int x, int y)
    {
        // Ensure the coordinates are within the valid range for the grid
        if (x >= 0 && x < Width && y >= 0 && y < Height) {
            return rows[y].cells[x]; // Retrieve the cell at the given position in the rows array
        } else {
            return null; // Return null if the coordinates are out of bounds
        }
    }

    // Method to find the adjacent cell to a given cell in a specified direction.
    // Returns the adjacent cell or null if the coordinates go out of bounds.
    public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
    {
        // Calculate the new coordinates by adding the direction to the current cell's coordinates
        Vector2Int coordinates = cell.coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y; // Negative direction for y-axis since Unity's grid starts from the top

        // Retrieve the cell at the calculated coordinates
        return GetCell(coordinates);
    }

    // Method to find a random empty cell in the grid.
    // Uses a starting index and wraps around to ensure all cells are checked.
    // Returns null if no empty cells are available.
    public TileCell GetRandomEmptyCell()
    {
        int index = Random.Range(0, cells.Length); // Start at a random index in the cells array
        int startingIndex = index;                 // Keep track of the starting index to detect a complete loop

        // Loop until an empty cell is found or all cells are checked
        while (cells[index].Occupied)
        {
            index++; // Increment the index to check the next cell

            if (index >= cells.Length) {
                index = 0; // Wrap around if the index exceeds the array length
            }

            // If the loop returns to the starting index, all cells are occupied
            if (index == startingIndex) {
                return null; // Return null to indicate no empty cells are available
            }
        }

        return cells[index]; // Return the empty cell found
    }

}

/*
Explanation:
------------
Awake: Initializes the rows and cells by finding them among the child objects. Assigns coordinates to each cell based on its index in the cells array.
GetCell: Overloads that return the corresponding TileCell based on x-y coordinates or a Vector2Int object. They return null if the coordinates are out of bounds.
GetAdjacentCell: Computes new coordinates using a direction vector and returns the adjacent cell.
GetRandomEmptyCell: Randomly selects an empty cell by wrapping around if necessary and returns null if no empty cells remain.
*/