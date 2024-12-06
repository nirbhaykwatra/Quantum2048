using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    public AudioSource src;
    public AudioClip notification_sound_1;
    // Add a reference to the Tunneling particle effect prefab
    public ParticleSystem tunnelingEffect;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private TileState[] tileStates;

    private TileGrid grid;
    private List<Tile> tiles;
    private bool waiting;
    private int tunnel_merge_1;
    private int tunnel_merge_2;
    private GameObject tunnelingPopup;
    private GameObject infoButton;
    PopUpSystem popUpSystem;
    RadialProgress radialProgress;
    RadialProgress radialProgress2;
    [SerializeField] GameObject tunnellingCompetencyCounter;
  

    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>(16);
        tunnel_merge_1 = 0;
        tunnel_merge_2 = 0;
        popUpSystem = GameObject.Find("PopUpManager").GetComponent<PopUpSystem>();
    
        // Find the Canvas first
        GameObject canvas = GameObject.Find("Canvas");
        
        if (canvas != null)
        {
            // Find Tunnelling1 under the Canvas
            tunnelingPopup = canvas.transform.Find("Tunnelling1")?.gameObject;

            // Find Info Button under the Canvas
            infoButton = canvas.transform.Find("Info Button")?.gameObject;

            // Find the RadialProgress and RadialProgress2 GameObjects under the Canvas
            radialProgress = tunnellingCompetencyCounter.GetComponent<RadialProgress>();
            radialProgress2 = tunnellingCompetencyCounter.GetComponent<RadialProgress>();

            // If RadialProgress and RadialProgress2 GameObjects are not found, log a warning
            if (radialProgress == null)
            {
                Debug.LogWarning("RadialProgress GameObject not found under Canvas!");
            }
            if (radialProgress2 == null)
            {
                Debug.LogWarning("RadialProgress2 GameObject not found under Canvas!");
            }
            
            if (tunnelingPopup != null && infoButton != null)
            {
                tunnelingPopup.SetActive(false); // Ensure it starts inactive
                infoButton.SetActive(false); // Ensure it starts inactive
            }
            else
            {
                Debug.LogWarning("Either Tunnelling1 or Info Button GameObjects were not found under Canvas!");
            }
            
        }
        else
        {
            Debug.LogWarning("Canvas GameObject not found in the scene!");
        }
    
    }

    public void ClearBoard()
    {
        foreach (var cell in grid.cells) {
            cell.tile = null;
        }

        foreach (var tile in tiles) {
            Destroy(tile.gameObject);
        }

        tiles.Clear();
    }

    public void CreateTile()
    {
        Tile tile = Instantiate(tilePrefab, grid.transform);
        tile.SetState(tileStates[0]);
        tile.Spawn(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }

    private void Update()
    {
        if (GlobalData.level == "tunnelling1")
        {
            if (tunnel_merge_1 > 1)
            {
                radialProgress.currentValue = (tunnel_merge_1 - 1) * 20;
            }
            else
            {
                radialProgress.currentValue = 5;
            }
        }
        else if (GlobalData.level == "tunnelling2")
        {
            if (tunnel_merge_2 > 1)
            {
                radialProgress2.currentValue = (tunnel_merge_2 - 1) * 20;
            }
            else
            {
                radialProgress2.currentValue = 5;
            }
        }


        if (!waiting)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                Move(Vector2Int.up, 0, 1, 1, 1);
            } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                Move(Vector2Int.left, 1, 1, 0, 1);
            } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                Move(Vector2Int.down, 0, 1, grid.Height - 2, -1);
            } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                Move(Vector2Int.right, grid.Width - 2, -1, 0, 1);
            }
        }
    }

    private void Move(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;

        for (int x = startX; x >= 0 && x < grid.Width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.Height; y += incrementY)
            {
                TileCell cell = grid.GetCell(x, y);

                if (cell.Occupied) {
                    changed |= MoveTile(cell.tile, direction);
                }
            }
        }

        if (changed) {
            StartCoroutine(WaitForChangesCoroutine());
        }
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell adjacent = grid.GetAdjacentCell(tile.cell, direction);

        Debug.Log("Current level: " + GlobalData.level); // Debug log

        while (adjacent != null)
        {
            if (adjacent.Occupied)
            {
                if (CanMerge(tile, adjacent.tile))
                {
                    MergeTiles(tile, adjacent.tile);
                    return true;
                }

                // Check for tunneling possibility
                TileCell nextAdjacent = grid.GetAdjacentCell(adjacent, direction);
                if (GlobalData.level == "tunnelling1")
                {
                    if (nextAdjacent != null && nextAdjacent.Occupied && CanMerge(tile, nextAdjacent.tile))
                    {
                        Debug.Log("Tunneling1: Executing TunnelingMergeTiles");
                        TunnelingMergeTiles(tile, adjacent.tile, nextAdjacent.tile);
                        return true;
                    }
                }
                else if (GlobalData.level == "tunnelling2")
                {
                    if (nextAdjacent != null && nextAdjacent.Occupied && CanMerge(tile, nextAdjacent.tile))
                    {
                        Debug.Log("Tunneling2: Executing TunnelingMergeTilesHigher");
                        TunnelingMergeTilesHigher(tile, adjacent.tile, nextAdjacent.tile);
                        return true;
                    }
                }
                break;
            }

            newCell = adjacent;
            adjacent = grid.GetAdjacentCell(adjacent, direction);
        }

        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }

        return false;
    }


    private bool CanMerge(Tile a, Tile b)
    {
        return a.state == b.state && !b.locked;
    }

    private void MergeTiles(Tile a, Tile b)
    {
        tiles.Remove(a);
        a.Merge(b.cell, false);

        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length - 1);
        TileState newState = tileStates[index];

        b.SetState(newState);
        GameManager.Instance.IncreaseScore(newState.number);
    }

    private void TunnelingMergeTiles(Tile a, Tile blocker, Tile b)
    {
        if (tunnel_merge_1 == 0 && tunnelingPopup != null)
        {
            // Activate the popup and play notification sound for the first time
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
        tunnel_merge_1 = tunnel_merge_1 + 1;
    
        tiles.Remove(a);
        a.Merge(b.cell, true);

        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length - 1);
        TileState newState = tileStates[index];

        b.SetState(newState);
        GameManager.Instance.IncreaseScore(newState.number);

        // Destroy(blocker.gameObject); // Example to destroy the blocker tile
    }

    private void TunnelingMergeTilesHigher(Tile a, Tile blocker, Tile b)
    {
        if (tunnel_merge_2 == 0 && tunnelingPopup != null)
        {
            // Activate the popup and play notification sound for the first time
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

        tunnel_merge_2 += 1; // Update tunnel_merge_2 instead of tunnel_merge_1

        tiles.Remove(a);
        a.Merge(b.cell, true);

        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length - 1);
        TileState newState = tileStates[index];

        b.SetState(newState);
        GameManager.Instance.IncreaseScore(newState.number);

        // Optionally destroy the blocker
        // Destroy(blocker.gameObject);
    }


    private int IndexOf(TileState state)
    {
        for (int i = 0; i < tileStates.Length; i++)
        {
            if (state == tileStates[i]) {
                return i;
            }
        }

        return -1;
    }

    private IEnumerator WaitForChangesCoroutine()
    {
        waiting = true;

        yield return new WaitForSeconds(0.1f);

        waiting = false;

        foreach (var tile in tiles) {
            tile.locked = false;
        }

        if (tiles.Count != grid.Size) {
            CreateTile();
        }

        if (CheckForGameOver()) {
            GameManager.Instance.GameOver();
        }
    }

    public bool CheckForGameOver()
    {
        if (tiles.Count != grid.Size) {
            return false;
        }

        foreach (var tile in tiles)
        {
            TileCell up = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            TileCell down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            TileCell left = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            TileCell right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);

            if (up != null && CanMerge(tile, up.tile)) {
                return false;
            }

            if (down != null && CanMerge(tile, down.tile)) {
                return false;
            }

            if (left != null && CanMerge(tile, left.tile)) {
                return false;
            }

            if (right != null && CanMerge(tile, right.tile)) {
                return false;
            }
        }

        return true;
    }
}