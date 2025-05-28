using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameEvents;

/*
 * TileBoard Class
 * ---------------
 * Manages the game board by handling tile creation, movement, merging (including tunneling merges),
 * updating progress bars, and checking for game over conditions.
 *
 * Additional Details:
 * - Tunneling merges (for "tunnelling1" and "tunnelling2") skip over a "blocker" tile if the threshold conditions are met
 *   (in "tunnelling2", thresholds {2,4,8,16} are considered).
 * - On the first tunneling merge in "tunnelling1", a popup and notification sound are triggered,
 *   and a particle effect is played if assigned.
 * - The 'infoButton' remains inactive by default but can be enabled if you want to display additional instructions.
 * - The "blocker" tile in tunneling merges is not destroyed by default, though there's a commented-out line to do so.
 * - A debug warning may mention "Background Music" even though it's not used in this script, left for potential expansion.
 */
public class TileBoard : MonoBehaviour
{
    // Audio source for playing notification sounds.
    public AudioSource src;
    // Notification sound clip.
    public AudioClip notification_sound_1;
    // Reference to the tunneling particle effect prefab.
    public ParticleSystem tunnelingEffect;

    // Tile prefab for instantiating new tiles.
    [SerializeField] private Tile tilePrefab;
    // Array of tile states used to update tile appearances.
    public TileState[] tileStates;
    
    [SerializeField] private BoolEventAsset _entangleModeEvent;
    
    [Tooltip("Percentage chance of spawning a superposition tile.")]
    [SerializeField] private float _superpositionChance;


    private GameObject _canvas;
    // Reference to the grid component that manages cell layout.
    private TileGrid grid;
    // List of tiles currently present on the board.
    private List<Tile> tiles;
    private List<Tile> _entangledTiles;
    // Flag indicating if the board is waiting for tile movements/merges to finish.
    private bool waiting;
    // Counter for the number of tunneling merges performed.
    private int tunnel_merge;
    // Cached threshold value used for merge comparisons.
    private int? cachedThreshold = null;
    [SerializeField] private Image _background;

    private bool _superposition = false;
    private bool _entangleMode = false;

    // Reference to the tunneling popup GameObject.
    private GameObject tunnelingPopup;
    // Reference to the info button GameObject.
    private GameObject infoButton;
    // Reference to the PopUpSystem component for handling popups.
    PopUpSystem popUpSystem;
    // References to radial progress components used for tracking tunneling progress.
    RadialProgress radialProgress;
    RadialProgress radialProgress2;
    // GameObjects for competency counters related to tunneling.
    [SerializeField] GameObject tunnellingCompetencyCounter;
    [SerializeField] GameObject tunnellingCompetencyCounter2;

    // Called when the script instance is being loaded.
    private void Awake()
    {
        // Get the TileGrid component from child objects.
        grid = GetComponentInChildren<TileGrid>();
        // Initialize the list of tiles with a capacity of 16.
        tiles = new List<Tile>(16);
        _entangledTiles = new List<Tile>();
        // Initialize the tunneling merge counter.
        tunnel_merge = 0;
        // Get the PopUpSystem component from the PopUpManager GameObject.
        popUpSystem = FindAnyObjectByType<PopUpSystem>().GetComponent<PopUpSystem>();
        
        _entangleModeEvent.Invoke(false);
    
        // Find the Canvas GameObject in the scene.
        _canvas = FindAnyObjectByType<Canvas>().gameObject;
        
        if (_canvas != null)
        {
            // Find the "Tunnelling1" popup under the Canvas.
            tunnelingPopup = _canvas.transform.Find("Tunnelling1")?.gameObject;
            // Find the "Info Button" under the Canvas.
            infoButton = _canvas.transform.Find("Info Button")?.gameObject;

            // Get the RadialProgress components from the competency counter GameObjects.
            radialProgress = tunnellingCompetencyCounter.GetComponent<RadialProgress>();
            radialProgress2 = tunnellingCompetencyCounter2.GetComponent<RadialProgress>();
            
            if (tunnelingPopup != null && infoButton != null)
            {
                // Ensure the popup and info button start inactive.
                tunnelingPopup.SetActive(false);
                infoButton.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Either Tunnelling1, Info Button, or Background Music GameObjects were not found under Canvas!");
            }
        }
        else
        {
            Debug.LogWarning("Canvas GameObject not found in the scene!");
        }
    }

    // Clears the board by unlinking cells and destroying all existing tiles.
    public void ClearBoard()
    {
        Debug.Log("Cleared board.");
        // Unlink tiles from all grid cells.
        foreach (TileCell cell in grid.cells)
        {
            cell.tile = null;
        }

        // Destroy all tile GameObjects.
        foreach (Tile tile in tiles)
        {
            Destroy(tile.gameObject);
        }

        // Clear the tiles list.
        tiles.Clear();
    }

    // Creates a new tile on the board.
    public void CreateTile()
    {
        // Instantiate a new tile from the tile prefab as a child of the grid.
        Tile tile = Instantiate(tilePrefab, grid.transform);
        // Set its initial state.
        tile.Superposition = Random.Range(0f, 100f) <= _superpositionChance;
        // Place the tile in a random empty cell.
        tile.Spawn(grid.GetRandomEmptyCell());
        // Add the tile to the list.
        tiles.Add(tile);
    }

    public void CreateTile(TileState state, TileCell cell, bool superposition = false)
    {
        Tile tile = Instantiate(tilePrefab, grid.transform);
        tile.SetState(state);
        tile.Superposition = superposition;
        tile.Spawn(cell);
        tiles.Add(tile);
    }

    // Called once per frame to handle player input and update progress values.
    private void Update()
    {
        // Update radial progress values based on the tunneling merge counter.
        if (tunnel_merge == 1)
        {
            radialProgress.currentValue = 5;
        }
        else if (tunnel_merge >= 2 && tunnel_merge <= 6)
        {
            radialProgress.currentValue = (tunnel_merge - 1) * 20;
        }
        else if (tunnel_merge >= 7 && tunnel_merge <= 12)
        {
            radialProgress2.currentValue2 = (tunnel_merge - 6) * 20;
        }

        // Process movement input only if not waiting for ongoing changes.
        if (!waiting)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Move(Vector2Int.up, 0, 1, 1, 1);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move(Vector2Int.left, 1, 1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                Move(Vector2Int.down, 0, 1, grid.Height - 2, -1);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(Vector2Int.right, grid.Width - 2, -1, 0, 1);
            }
        }
    }

    // Moves tiles across the board in the specified direction.
    // Parameters:
    //   direction    - The movement direction as a Vector2Int.
    //   startX       - Starting index for horizontal iteration.
    //   incrementX   - Increment value for horizontal iteration.
    //   startY       - Starting index for vertical iteration.
    //   incrementY   - Increment value for vertical iteration.
    private void Move(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;

        // Iterate through grid cells based on the provided indices.
        for (int x = startX; x >= 0 && x < grid.Width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.Height; y += incrementY)
            {
                TileCell cell = grid.GetCell(x, y);

                if (cell.Occupied)
                {
                    changed |= MoveTile(cell.tile, direction);
                }
            }
        }

        // If any tile has moved or merged, wait for the changes to process.
        if (changed)
        {
            StartCoroutine(WaitForChangesCoroutine());
        }
    }

    // Attempts to move a single tile in the given direction.
    // Returns true if the tile moved or merged; otherwise, false.
    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell adjacent = grid.GetAdjacentCell(tile.cell, direction);

        // Traverse adjacent cells in the given direction.
        while (adjacent != null)
        {
            if (adjacent.Occupied)
            {
                // Check if the tile can merge with the adjacent tile.
                if (CanMerge(tile, adjacent.tile))
                {
                    MergeTiles(tile, adjacent.tile);
                    return true;
                }

                // Check for tunneling possibility.
                TileCell nextAdjacent = grid.GetAdjacentCell(adjacent, direction);
                if (GlobalData.level == "tunnelling1")
                {
                    if (nextAdjacent != null && nextAdjacent.Occupied && CanMerge(tile, nextAdjacent.tile))
                    {
                        TunnelingMergeTiles(tile, adjacent.tile, nextAdjacent.tile);
                        return true;
                    }
                }
                else if (GlobalData.level == "tunnelling2")
                {
                    if (nextAdjacent != null && nextAdjacent.Occupied && CanMerge(tile, nextAdjacent.tile, 2, 4, 8, 16))
                    {
                        TunnelingMergeTiles(tile, adjacent.tile, nextAdjacent.tile);
                        return true;
                    }
                }
                break;
            }

            newCell = adjacent;
            adjacent = grid.GetAdjacentCell(adjacent, direction);
        }

        // If a new cell is found, move the tile there.
        if (newCell != null)
        {
            tile.MoveTo(newCell);
            if (_superposition) tile.SetState(tileStates[Random.Range(0, 4)]);
            return true;
        }

        return false;
    }

    // Determines if two tiles can merge based on their states and optional threshold values.
    // Additional thresholds can be provided via the 'thresholds' parameter.
    private bool CanMerge(Tile a, Tile b, params int[] thresholds)
    {
        int selectedThreshold = 0;

        if (thresholds != null && thresholds.Length > 0)
        {
            if (!cachedThreshold.HasValue)
            {
                // Select a threshold: if only one is provided, use it; otherwise, pick one at random.
                cachedThreshold = (thresholds.Length == 1)
                    ? thresholds[0]
                    : thresholds[Random.Range(0, thresholds.Length)];
            }

            selectedThreshold = cachedThreshold.Value;
            // Debug.Log("Selected threshold: " + selectedThreshold);
        }

        return a.state == b.state &&
               !b.locked &&
               a.state.number > selectedThreshold &&
               b.state.number > selectedThreshold;
    }

    // Merges two tiles into one.
    // Tile 'a' is merged into tile 'b', which then updates its state.
    private void MergeTiles(Tile a, Tile b)
    {
        a.Superposition = false;
        tiles.Remove(a);         // Remove tile 'a' from the board.
        if (_entangledTiles.Contains(a)) _entangledTiles.Remove(a);
        a._entangledTile = null;
        a.Merge(b.cell, false);  // Merge tile 'a' into tile 'b' without tunneling.
        
        // Determine the new state for tile 'b'.
        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length - 1);
        TileState newState = tileStates[index];
        
        if (_entangledTiles.Contains(b)) _entangledTiles.Remove(b);
        b._entangledTile = null;
        b.SetState(newState);                               // Update tile 'b' with the new state.
        GameManager.Instance.IncreaseScore(newState.number);  // Increase the game score.
    }

    // Performs a tunneling merge between tiles.
    // Increments the tunneling merge counter, triggers popups and particle effects (on first merge), and merges tiles.
    private void TunnelingMergeTiles(Tile a, Tile blocker, Tile b)
    {
        a.Superposition = false;
        tunnel_merge += 1;
        if (tunnel_merge == 1 && tunnelingPopup != null && GlobalData.level == "tunnelling1")
        {
            // Activate the tunneling popup and play the notification sound for the first tunneling merge.
            popUpSystem.Tunneling();

            src.clip = notification_sound_1;
            src.Play();

            if (tunnelingEffect != null)
            {
                tunnelingEffect.Play();
            }
            else
            {
                Debug.LogWarning("Tunneling Particle Effect not assigned in the Inspector!");
            }
        }
        
        tiles.Remove(a);           // Remove the merged tile.
        if (_entangledTiles.Contains(a)) _entangledTiles.Remove(a);
        a._entangledTile = null;
        a.Merge(b.cell, true);     // Merge tile 'a' into tile 'b' with tunneling.

        // Determine the new state for tile 'b'.
        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length - 1);
        TileState newState = tileStates[index];

        b.SetState(newState);                              // Update tile 'b' with the new state.
        GameManager.Instance.IncreaseScore(newState.number); // Increase the game score.
        a.Superposition = false;
        b.Superposition = false;

        // Optionally, handle the blocker tile here (e.g., destroy it).
        // Destroy(blocker.gameObject); // Example: destroy the blocker tile.
    }

    // Returns the index of a given TileState in the tileStates array.
    private int IndexOf(TileState state)
    {
        for (int i = 0; i < tileStates.Length; i++)
        {
            if (state == tileStates[i])
            {
                return i;
            }
        }
        return -1;
    }

    // Coroutine that waits for ongoing tile changes to complete before unlocking tiles,
    // creating a new tile, and checking for game over conditions.
    private IEnumerator WaitForChangesCoroutine()
    {
        waiting = true;

        yield return new WaitForSeconds(0.1f);

        waiting = false;

        // Unlock all tiles.
        foreach (var tile in tiles)
        {
            tile.locked = false;
        }

        // Create a new tile if the board is not full.
        if (tiles.Count != grid.Size)
        {
            CreateTile();
        }

        // Check if there are no more moves, and trigger game over if so.
        if (CheckForGameOver())
        {
            GameManager.Instance.GameOver();
        }
    }

    // Checks if the board is in a game over state.
    // Returns true if no more moves or merges are possible.
    public bool CheckForGameOver()
    {
        if (tiles.Count != grid.Size)
        {
            return false;
        }

        // Check each tile to see if a merge is possible with any adjacent tile.
        foreach (var tile in tiles)
        {
            TileCell up = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            TileCell down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            TileCell left = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            TileCell right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);

            if (up != null && CanMerge(tile, up.tile))
            {
                return false;
            }
            if (down != null && CanMerge(tile, down.tile))
            {
                return false;
            }
            if (left != null && CanMerge(tile, left.tile))
            {
                return false;
            }
            if (right != null && CanMerge(tile, right.tile))
            {
                return false;
            }
        }

        return true;
    }

    public void ChangeBackgroundColor()
    {
        _background.color = _entangleMode ? new Color32(255, 255, 255, 255) : new Color32(0, 7, 111, 255);
    }

    public void ToggleEntangleMode()
    {
        _entangleMode = !_entangleMode;
        foreach (Tile entangledTile in _entangledTiles)
        {
            entangledTile._entangledTile = null;
            entangledTile.background.color = entangledTile.state.backgroundColor;
            entangledTile._entangledTile = null;
        }
        _entangledTiles.Clear();
        _entangleModeEvent.Invoke(_entangleMode);
    }

    public void AddEntangledTile(Tile tile)
    {
        if (_entangledTiles.Contains(tile) || _entangledTiles.Count >= 2) return;
        Debug.Log($"Added number {tile.state.number} tile with ID {tile.TileID}!");
        _entangledTiles.Add(tile);
        tile.background.color = Color.red;

        Debug.Log($"Entangled Tiles List:");
        foreach (Tile entangledTile in _entangledTiles)
        {
            Debug.Log($"Tile {entangledTile.TileID}");
        }

        if (_entangledTiles.IndexOf(tile) == 1)
        {
            _entangledTiles[0]._entangledTile = tile;
            tile.SetState(_entangledTiles[0].state);
            _entangledTiles[0].background.color = tile.state.backgroundColor;
            Debug.Log($"Tile ID {_entangledTiles[0].TileID} entangled with tile ID {tile.TileID}");
            _entangleMode = false;
            _entangleModeEvent.Invoke(_entangleMode);
        }
    }

    public void RemoveEntangledTile(Tile tile)
    {
        if (!_entangledTiles.Contains(tile)) return;
        Debug.Log($"Removed number {tile.state.number} tile!");
        _entangledTiles.Remove(tile);
        tile.background.color = tile.state.backgroundColor;
        tile._entangledTile = null;
    }
}
