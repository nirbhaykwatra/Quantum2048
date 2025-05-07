using UnityEngine;

/*
 * TileState Class
 * ---------------
 * Defines a specific state for a tile, such as its number and colors.
 * Marked with [CreateAssetMenu] to enable asset creation via the Unity Editor's "Create" menu.
 */
[CreateAssetMenu(menuName = "Tile State")]
public class TileState : ScriptableObject
{
    // The numerical value associated with this tile state.
    public int number;
    // The background color to use for tiles in this state.
    public Color backgroundColor;
    public Color superpositionColor;
    // The text color to use for numbers in this state.
    public Color textColor;
}