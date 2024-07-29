// Import the core Unity namespace to access ScriptableObject and other Unity classes
using UnityEngine;

// The TileState class defines a specific state for a tile, such as its number and colors.
// It is marked with [CreateAssetMenu] to enable asset creation via the Unity Editor's "Create" menu.
[CreateAssetMenu(menuName = "Tile State")]
public class TileState : ScriptableObject
{
    // Public fields defining the properties of a tile state
    public int number;                // The numerical value associated with this tile state
    public Color backgroundColor;     // The background color to use for tiles in this state
    public Color textColor;           // The text color to use for numbers in this state
}

/*
Explanation:
------------
[CreateAssetMenu]: Allows easy creation of TileState assets directly through Unity's "Create" menu, making it simpler to generate and configure tile states.
ScriptableObject: The TileState class inherits from ScriptableObject, enabling data to be stored independently of GameObjects.
Fields:
- number: Represents the numerical value displayed on the tile.
- backgroundColor: The color used for the tile's background.
- textColor: The color used for the number text displayed on the tile.
*/